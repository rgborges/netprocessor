using System;
using System.Data;
using System.Diagnostics;
using NetProcessor.Data.Result.Interfaces;

namespace NetProcessor.Data.DataFrame;


//usage var data = new DataFrame(new [] { "Name" }, new [] { "Gabriel", "Rafael", "Teresa"}  )
// data["Name"].Top(10);
// data["Name"].Apply( x => Hash(x)); 

/// <summary>
/// This dataframe object does not support other types than objects. The support of other steps will be implemented in the future.
/// </summary>


public class DataFrame : IDisposable
{
      private readonly int _rowCount = 0;
      private int _columnCount = 0;
      private readonly List<string> _columnNames = new List<string>();
      public Dictionary<string, object[]> Data { get; }
      /// <summary>
      /// Returns the matrix shape. ex(1,3), (4,4)
      /// </summary>
      public (int, int) Shape
      {
            get
            {
                  return (_rowCount, _columnCount);
            }
      }
      /// <summary>
      /// Return the columns name stored in the dataframe.
      /// </summary>
      public string[] ColumnsName => _columnNames.ToArray();
      /// <summary>
      /// Return the number of rows stored in the dataframe.
      /// </summary>
      public int Rows => _rowCount;
      /// <summary>
      /// Return the number of columns stored in the dataframe.
      /// </summary>
      public int Columns => _columnCount;

      /// <summary>
      /// Return the row index element as a List<object>()
      /// </summary>
      /// <param name="row"></param>
      public object this[int row]
      {
            get
            {
                  var rowList = new List<object>();

                  foreach (var column in _columnNames)
                  {
                        var rowValue = Data[column][row];
                        rowList.Add(rowValue);
                  }
                  return rowList;
            }
      }
      /// <summary>
      /// The number of the current page
      /// </summary>
      public int Page { get; private set; }
      /// <summary>
      /// Total page size
      /// </summary>
      public int PageSize { get; private set; }
      /// <summary>
      /// Return the values of the given column.
      /// </summary>
      /// <param name="columnName"></param>
      ///
      public int NumberOfPages { get; private set; }

      public object this[string columnName]
      {
            get
            {
                  return this.Data[columnName];
            }
      }

      public DataFrame(int rows, int columns, Dictionary<string, object[]> data)
      {
            _rowCount = rows;
            _columnCount = columns;
            Page = 1;
            NumberOfPages = 1;
            PageSize = rows;
            Data = data;
      }

      public List<(int, int)> Pages { get; set; }

      public DataFrame(int rows, int columns, string[] columnNames, Dictionary<string, object[]> data, int pageSize)
      {
            _rowCount = rows;
            _columnCount = columns;
            Page = 1;
            PageSize = pageSize;
            Pages = new List<(int, int)>();

            NumberOfPages = (int)Math.Ceiling((double)rows / pageSize);

            int cursor = 0;
            for (int page = 0; page < NumberOfPages; page++)
            {
                  int start = cursor;
                  int end = Math.Min(cursor + pageSize - 1, rows - 1);
                  Pages.Add((start, end));
                  cursor += pageSize;
            }

            Data = data;
      }

      public DataFrame(string title, object[] values)
      {
            Data = new Dictionary<string, object[]>() {
                  { title, values },
            };

            _columnCount = 1;
            _columnNames.Add(title);
            _rowCount = values.Length;
      }
      /// <summary>
      /// Add a new column to the dataframe.
      /// </summary>
      /// <param name="title"></param>
      /// <param name="values"></param>
      /// <returns></returns>
      public DataFrame AddColumn(string title, object[] values)
      {
            if (values.Length != _rowCount)
            {
                  throw new ArgumentException("Invalid vector to be added to data frame");
            }
            Data.Add(title, values);
            _columnNames.Add(title);
            _columnCount++;
            return this;
      }
      public IEnumerable<object> Top(int numItems)
      {
            if (numItems >= _rowCount)
            {
                  throw new InvalidOperationException("Top number out of bounds");
            }
            return GetRows(0, numItems);
      }
      public IEnumerable<object> GetRows(int startRow, int endRow)
      {
            var rows = new List<object>();

            if (startRow < 0 || startRow >= _rowCount)
            {
                  throw new InvalidOperationException("Start row out of range.");
            }

            for (int rowindex = startRow; rowindex <= endRow; rowindex++)
            {
                  var rowList = new List<object>();
                  foreach (var column in _columnNames)
                  {
                        rowList.Add(Data[column][rowindex]);
                  }
                  rows.Add(rowList);
            }
            return rows;
      }
      public Dictionary<string, object[]> GetColumns(params string[] columnNames)
      {
            var result = new Dictionary<string, object[]>();

            foreach (var columnName in columnNames)
            {
                  if (Data.ContainsKey(columnName))
                  {
                        result.Add(columnName, Data[columnName]);
                  }
                  else
                  {
                        throw new ArgumentException($"Column '{columnName}' does not exist in the DataFrame.");
                  }
            }

            return result;
      }
      /// <summary>
      ///Remove a column from the dataframe. 
      /// </summary>
      /// <param name="columnName"></param>
      public void RemoveColumn(string columnName)
      {
            if (Data.ContainsKey(columnName))
            {
                  Data.Remove(columnName);
                  _columnNames.Remove(columnName);
                  _columnCount--;
            }
            else
            {
                  throw new ArgumentException($"Column '{columnName}' does not exist in the DataFrame.");
            }
      }

      public DataFrame Filter(string columnName, Predicate<object> predicate)
      {
            if (!Data.ContainsKey(columnName))
            {
                  throw new ArgumentException($"Column '{columnName}' does not exist in the DataFrame.");
            }

            var filteredData = new Dictionary<string, List<object>>();
            foreach (var column in _columnNames)
            {
                  filteredData[column] = new List<object>();
            }

            for (int i = 0; i < _rowCount; i++)
            {
                  if (predicate(Data[columnName][i]))
                  {
                        foreach (var column in _columnNames)
                        {
                              filteredData[column].Add(Data[column][i]);
                        }
                  }
            }

            var resultData = filteredData.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.ToArray());
            return new DataFrame(resultData.First().Value.Length, _columnCount, resultData);
      }
      public void Dispose()
      {
            this.Data.Clear();
      }

      /// <summary>
      /// Generates a DataFrame from a CSV file.
      /// </summary>
      /// <param name="path">The file path</param>
      /// <param name="header">If the use headers or not. the dafault option is true</param>
      /// <param name="columns">Override the columns name</param>
      /// <returns>Returns a dataframe</returns>
      /// <exception cref="FileNotFoundException"></exception> <summary>
      public static DataFrame FromCSV(string path, bool header = true, char delimiter = ',', string[] columns = null)
      {
            //TODO: Implement CSV reader

            int rowCount = 0, columnCount = 0;

            var fileInfo = new FileInfo(path);

            if (!fileInfo.Exists)
            {
                  throw new FileNotFoundException("File not found.");
            }

            if (fileInfo.Length > 10 * 1024 * 1024)
            {
                  Debug.WriteLine("Warning: The file is larger than 10MB. Consider using a more efficient method for large files.");
                  throw new NotSupportedException("The file is larger than 10MB. Use the instance format");
            }


            var lines = File.ReadAllLines(path);

            if (header)
            {
                  columns = lines[0].Split(delimiter);
                  columnCount = columns.Length;
            }

            rowCount = lines.Length - 1;

            string[,] data = new string[rowCount, columnCount];
            // make a function to parse the string data splited by ',' to data multidimensional array
            for (int i = 1; i <= rowCount; i++)
            {
                  //skip header values
                  var line = lines[i].Split(',');
                  for (int j = 0; j < columnCount; j++)
                  {
                        data[i - 1, j] = line[j];
                  }
            }

            var resultData = new Dictionary<string, object[]>();

            for (int i = 0; i < columnCount; i++)
            {
                  var rowData = new object[rowCount];
                  for (int j = 0; j < rowCount; j++)
                  {
                        rowData[j] = data[j, i];
                  }
                  resultData.Add(columns[i], rowData);
            }

            return new DataFrame(rowCount, columnCount, resultData);
      }

      public static IEnumerable<DataFrame> FromCSVInChunks(string path,int chunkSize = 100, bool header = true, char delimiter = ',', string[] columns = null)
      {
            int rowCount = 0, columnCount = 0;

            var fileInfo = new FileInfo(path);

            if (!fileInfo.Exists)
            {
                  throw new FileNotFoundException("File not found.");
            }

            if (fileInfo.Length > 10 * 1024 * 1024)
            {
                  Debug.WriteLine("Warning: The file is larger than 10MB. Consider using a more efficient method for large files.");
            }

            bool firstTime = true;
            foreach (string[] lines in File.ReadLines(fileInfo.FullName).Chunk(chunkSize))
            {
                  if (firstTime)
                  {
                        if (header)
                        {
                              columns = lines[0].Split(delimiter);
                              columnCount = columns.Length;
                        }
                        firstTime = false;
                  }
                  rowCount = lines.Length - 1;
                  string[,] data = new string[rowCount, columnCount];
                  // make a function to parse the string data splited by ',' to data multidimensional array
                  for (int i = 1; i <= rowCount; i++)
                  {
                        //skip header values
                        var line = lines[i].Split(',');
                        for (int j = 0; j < columnCount; j++)
                        {
                              data[i - 1, j] = line[j];
                        }
                  }

                  var resultData = new Dictionary<string, object[]>();

                  for (int i = 0; i < columnCount; i++)
                  {
                        var rowData = new object[rowCount];
                        for (int j = 0; j < rowCount; j++)
                        {
                              rowData[j] = data[j, i];
                        }
                        resultData.Add(columns[i], rowData);
                  }

                  yield return new DataFrame(rowCount, columnCount, resultData);
            }

      }



}