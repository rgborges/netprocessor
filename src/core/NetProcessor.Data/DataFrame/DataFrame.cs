using System;
using System.Data;
using System.Diagnostics;
using NetProcessor.Data.Result.Interfaces;

namespace NetProcessor.Data.DataFrame;


//usage var data = new DataFrame(new [] { "Name" }, new [] { "Gabriel", "Rafael", "Teresa"}  )
// data["Name"].Top(10);
// data["Name"].Apply( x => Hash(x)); 


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
      /// Return the values of the given column.
      /// </summary>
      /// <param name="columnName"></param>
      public object this[string columnName]
      {
            get
            {
                  return this.Data[columnName];
            }
      }
      public DataFrame(int rows, 
      int columns,
      Dictionary<string, object[]> data )
      {
            _rowCount = rows;
            _columnCount = columns;

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
      public void Dispose()
      {
            this.Data.Clear();
      }
      public static DataFrame FromCSV(string path, bool header = true, string[] columns = null)
      {
            //TODO: Implement CSV reader
            int rowCount = 0, columnCount = 0;

            var fileInfo = new FileInfo(path);

            if (!fileInfo.Exists)
            {
                  throw new FileNotFoundException("File not found.");
            }
            var lines = File.ReadAllLines(path);      

            if (header)
            {
                  columns = lines[0].Split(',');
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

            return  new DataFrame(rowCount, columnCount, resultData);
      }
}