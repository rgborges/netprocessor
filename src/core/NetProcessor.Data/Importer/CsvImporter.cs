using System.Collections;
using System.Reflection;
using BgSoftLab.Results;
using NetProcessor.Data.Importer;

namespace NetProcessor.Data;

public class CsvParser<TRecord>
{
      private readonly FileInfo _fileInfo;
      private bool _useInvarianCulture;
      private readonly FileImporterOptions _fileImportOptions;
      public CsvParser(FileInfo fileInfo, bool useInvarianCulture)
      {
            _fileInfo = fileInfo;
            _useInvarianCulture = useInvarianCulture;
            _fileImportOptions = new FileImporterOptions();
      }
      public CsvParser(FileInfo fileInfo, Action<FileImporterOptions> configAction)
      {
            _fileInfo = fileInfo;
            if (_fileImportOptions is null)
            {
                  _fileImportOptions = new FileImporterOptions();
            }
            configAction(_fileImportOptions);
      }
      private Dictionary<string, Type> GenerateColumnsAndTypes()
      {
            try
            {
                  var result = new Dictionary<string, Type>();
                  var type = typeof(TRecord);
                  var properties = type.GetProperties();
                  foreach (var property in properties)
                  {
                        if (_fileImportOptions.UseSmallCasePropertiesComparison)
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
      public ParserResult ReadAll()
      {
            try
            {
                  var result = new ParserResult();
                  result.Start();
                  if (_fileInfo is null)
                  {
                        result.FinishWithError(_fileInfo, "File info was not specified");
                        return result;
                  }

                  if (_fileInfo.Extension != ".csv")
                  {
                        result.FinishWithError(_fileInfo, "The file extentions is invalid");
                        return result;
                  }
                  string file = _fileInfo?.FullName;
                  if (!_fileInfo.Exists)
                  {
                        result.FinishWithError(_fileInfo, "The file doesn't exist");
                        return result;
                  }
                  var content = File.ReadAllLines(file);

                  var fileHeaders = content[0].Split(_fileImportOptions.ColumnDelimiterChar);
                  //TODO: Verify if columns match with the headers
                  var typeColumns = this.GenerateColumnsAndTypes().Keys.ToArray<string>();

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

                  //TODO parser

                  var data = ParseFromString(ref content, ref fileHeaders);

                  result.Finish(data);

                  return result;
            }
            catch
            {
                  throw;
            }
      }
      private ICollection<TRecord> ParseFromString(ref string[] content, ref string[] columns)
      {
            var result = new List<TRecord>(content.Length);
            var properties = typeof(TRecord).GetProperties();

            try
            {
                  for (int i = 0; i < content.Length; i++)
                  {
                        //Condition to skip heder
                        TRecord generic = (TRecord)Activator.CreateInstance<TRecord>();

                        if (i == 0) { continue; }
                        //TODO: In this case the description has ' sign, and this alse are the split char for this file. The description has "" sign, which means to do not 
                        //consideer the commas.
                        string[] splitRowData = content[i].Split(_fileImportOptions.ColumnDelimiterChar);

                        var lineParsingResult = this.LineParserFunction(content[i]);

                        var valueList = new List<string>();

                        foreach (var item in lineParsingResult)
                        {
                              valueList.Add(item.Item3);
                        }

                        if (valueList.Count != columns.Length)
                        {
                              throw new InvalidOperationException("The columns idenfied through type mismatch of identified values");
                        }
                        for (int j = 0; j < properties.Length; i++)
                        {
                              properties[j].SetValue(generic, valueList[j]);
                        }

                        result.Add(generic);
                  }
                  return result;
            }
            catch
            {
                  throw;
            }
      }

      private List<Tuple<int, CsvTokens, string>> LineParserFunction(string line)
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

      public ParserResult Read()
      {
            var result = new ParserResult();
            result.Start();
            result.Finish(new string[] { "teste" });
            var data = result.GetResult<string[]>();
            return result;
      }


}

public enum CsvTokens
{
      EndOfColumn,
      Content,
      InitiateStatement,
      FinishStatement,
      EndOfLine,
      Undefined
}
