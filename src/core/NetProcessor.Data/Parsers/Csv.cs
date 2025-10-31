using System.Collections;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace NetProcessor.Data;

public static class CSV
{
      public static DataProcessingOperation ReadAll<T>(string filepath, char delimiter = ',', bool smallCaseCompare = false, bool strictType = false)
      {
            try
            {
                  var result = new DataProcessingOperation();

                  var fileInfo = new FileInfo(filepath);
/// In Linux we don't have the distinction of the file using the format.
                  if (fileInfo.Extension != ".csv")
                  {
                        throw new InvalidDataException("The file is not a csv file.");
                  }
                  if (!fileInfo.Exists)
                  {
                        throw new FileNotFoundException("The specified file was not found in filesystem.");
                  }

                  var content = File.ReadAllLines(fileInfo.FullName);

                  var fileHeaders = content[0].Split(delimiter);

                  var typeColumns = GenerateColumnsAndTypes<T>(smallCaseCompare).Keys.ToArray<string>();

                  bool areEqual = StructuralComparisons.StructuralEqualityComparer.Equals(typeColumns, fileHeaders);

                  var equalColumns = typeColumns.Intersect(fileHeaders);

                  if (!areEqual && !equalColumns.Any())
                  {
                        foreach (string s in typeColumns)
                        {
                              result.AddError(string.Format("{0}:{1}",
                              "Column from file doesn't match with type provided: ",
                               s
                               ));
                        }
                        return result;
                  }

                  DataProcessingOperation parserResult;

                  parserResult = ParseFromString<T>(ref content, ref fileHeaders);

                  if (!parserResult.Result.Success)
                  {
                        foreach (string s in parserResult.Errors)
                        {
                              result.AddError(s);
                        }
                  }
                  result.Finish(parserResult.GetResult().GetData());

                  return result;
            }
            catch
            {
                  throw;
            }

      }
      private static DataProcessingOperation ParseFromString<T>(ref string[] content, ref string[] columns)
      {
            var result = new DataProcessingOperation();
            result.Start();
            var dataResult = new List<T>(content.Length);
            var properties = typeof(T).GetProperties();
            try
            {
                  for (int i = 0; i < content.Length; i++)
                  {

                        T generic = (T)Activator.CreateInstance<T>();

                        if (i == 0) { continue; }

                        var lineParsingResult = LineParserFunction(content[i]);

                        var valueList = new List<string>();

                        foreach (var item in lineParsingResult)
                        {
                              if (item.Item3 is not null)
                              {
                                    valueList.Add(item.Item3);
                              }
                        }

                        if (valueList.Count != columns.Length)
                        {
                              result.AddError($"The columns idenfied through type mismatch of identified values line {i}");
                              continue;
                        }

                        for (int j = 0; j < properties.Length; j++)
                        {
                              properties[j].SetValue(generic, TryConvert(valueList[j], properties[j].PropertyType.Name));
                        }

                        dataResult.Add(generic);
                  }
                  result.Finish(dataResult);
                  
                  return result;
            }
            catch
            {
                  throw;
            }
      }
      private static object TryConvert(string s, string propTypeName)
      {
            double tmpDouble;
            int tmpInt;
            Int16 tmpInt16;
            Int64 tmpInt64;
            long tmpLong;
            bool tmpBool;

            switch (propTypeName)
            {
                  case "String":
                        return s;
                  case "Double":
                        if (double.TryParse(s, out tmpDouble))
                        {
                              return tmpDouble;
                        }
                        return 0;
                  case "Int16":
                        if (Int16.TryParse(s, out tmpInt16))
                        {
                              return tmpInt16;
                        }
                        return s;
                  case "Int32":
                        if (Int32.TryParse(s, out tmpInt))
                        {
                              return tmpInt;
                        }
                        return 0;
                  case "Int64":
                        if (Int64.TryParse(s, out tmpInt64))
                        {
                              return tmpInt64;
                        }
                        return 0;
                  case "Long":
                        if (long.TryParse(s, out tmpLong))
                        {
                              return tmpLong;
                        }
                        return 0;
                  case "Boolean":
                        if (bool.TryParse(s, out tmpBool))
                        {
                              return tmpBool;
                        }
                        return false;
                  default:
                        return s;
            }
      }
      private static List<Tuple<int, CsvTokens, string>> LineParserFunction(string line)
      {
            var result = new List<Tuple<int, CsvTokens, string>>();
            int carriedge = 0;

            CsvTokens tmp = CsvTokens.Undefined;

            bool initiateStatement = false;

            var rule = new Dictionary<char, CsvTokens>();
            rule.Add(',', CsvTokens.EndOfColumn);
            rule.Add('"', CsvTokens.InitiateStatement);
            rule.Add('\n', CsvTokens.EndOfLine);

            for (int i = 0; i < line.Length; i++)
            {
                  char c = line[i];
                  if (rule.ContainsKey(c))
                  {
                        if (rule.TryGetValue(c, out tmp))
                        {
                              if (tmp == CsvTokens.InitiateStatement)
                              {
                                    if (initiateStatement)
                                    {
                                          initiateStatement = false;
                                    }
                                    else
                                    {
                                          initiateStatement = true;
                                    }
                                    continue;
                              }
                              if (initiateStatement)
                              {
                                    continue;
                              }
                              result.Add(Tuple.Create(i, tmp, line.Substring(carriedge, (i - carriedge))));
                        }
                        carriedge = i + 1;
                  }
                  if (i == line.Length - 1)
                  {
                        result.Add(Tuple.Create(i, tmp, line.Substring(carriedge, (i - carriedge))));
                  }
            }
            return result;
      }
      private static Dictionary<string, Type> GenerateColumnsAndTypes<T>(bool forceSmallCase = false)
      {
            try
            {
                  var result = new Dictionary<string, Type>();
                  var type = typeof(T);
                  var properties = type.GetProperties();
                  foreach (var property in properties)
                  {
                        if (forceSmallCase)
                        {
                              result.Add(property.Name.ToLower(), property.PropertyType);
                              continue;
                        }
                        result.Add(property.Name, property.PropertyType);
                  }
                  return result;
            }
            catch
            {
                  throw;
            }
      }
      public class DataResult
      {
            public DataTable ToDataTable()
            {
                  throw new NotImplementedException();
            }
            public List<T> ToList<T>()
            {
                  throw new NotImplementedException();
            }
            public T[][] ToMatrix<T>()
            {
                  throw new NotImplementedException();
            }

      }

}