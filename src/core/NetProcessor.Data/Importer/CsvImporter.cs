using System.Collections;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Reflection.Metadata;
using BgSoftLab.Results;
using NetProcessor.Data.Importer;

namespace NetProcessor.Data;

public class CsvImporter<T> : BaseImporter<T>
{
      private readonly FileInfo _fileInfo;
      private bool _useInvarianCulture;
      private readonly FileImporterOptions _fileImportOptions;
      private Func<string[], DataProcessingOperation> _lineParserFunc;
      private Func<ParserLineContext, DataProcessingOperation> _convertParserFunction;
      private Dictionary<string, Type> _columnsType;
      private ColumnTypeConfigurator<T> _columnConfigurator;

      #region  Properties
      public bool Read { get; private set; }
      public int CurrentLine { get; private set; }
      #endregion
      #region  Constructors
      public CsvImporter(string filePath, bool useInvarianCulture, ILineParserConverter<string, T> lineParser) : base(lineParser)
      {
            if (String.IsNullOrEmpty(filePath))
            {
                  throw new FileNotFoundException(nameof(filePath));
            }
            _fileInfo = new FileInfo(filePath);
            _useInvarianCulture = useInvarianCulture;
            _fileImportOptions = new FileImporterOptions();
      }
      public CsvImporter(string fileInfo, Action<FileImporterOptions> configAction, ILineParserConverter<string, T> lineParser) : base(lineParser)
      {
            _fileInfo = new FileInfo(fileInfo);

            if (_fileImportOptions is null)
            {
                  _fileImportOptions = new FileImporterOptions();
            }

            configAction(_fileImportOptions);
      }
      #endregion
      #region Configurators
      /// <summary>
      /// Defines columns name and their respective types of the format. This will override the 
      /// original read properties from the type behaivor.
      /// </summary>
      /// <param name="func"></param>
      /// <returns>This object</returns>
      public CsvImporter<T> OverrideColumnsConfiguration(Action<ColumnTypeConfigurator<T>> action)
      {
            _columnConfigurator = new ColumnTypeConfigurator<T>();
            action(_columnConfigurator);
            return this;
      }
      public CsvImporter<T> SetLineParserFunction(Func<string[], DataProcessingOperation> lineFunc)
      {
            _lineParserFunc = lineFunc;
            return this;
      }

      #endregion
      #region Internal Help Functions
      private Dictionary<string, Type> GenerateColumnsAndTypes()
      {
            try
            {
                  var result = new Dictionary<string, Type>();
                  var type = typeof(T);
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
      private DataProcessingOperation ParseFromString(ref string[] content, ref string[] columns)
      {
            var result = new DataProcessingOperation();
            result.Start();
            var dataResult = new List<T>(content.Length);
            var properties = typeof(T).GetProperties();
            try
            {
                  for (int i = 0; i < content.Length; i++)
                  {
                        this.CurrentLine = i;

                        T generic = (T)Activator.CreateInstance<T>();

                        if (i == 0) { continue; }

                        var lineParsingResult = this.LineParserFunction(content[i]);

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
                              result.AddError($"The columns idenfied through type mismatch of identified values line {CurrentLine}");
                              continue;
                        }
                        for (int j = 0; j < properties.Length; j++)
                        {
                              properties[j].SetValue(generic, valueList[j]);
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
      #endregion
      #region ParsingMethods
      public DataProcessingOperation ReadAllLines()
      {
            try
            {
                  var result = new DataProcessingOperation();

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

                  DataProcessingOperation parserResult;

                  if (_lineParserFunc is not null)
                  {
                        try
                        {
                              parserResult = _lineParserFunc(content);
                        }
                        catch
                        {
                              throw;
                        }
                  }
                  else
                  {
                        parserResult = ParseFromString(ref content, ref fileHeaders);
                  }

                  if (!parserResult.Result.Success)
                  {
                        foreach (string s in parserResult.Errors)
                        {
                              result.AddError(s);
                        }
                  }
                  result.Finish(parserResult);

                  return result;
            }
            catch
            {
                  throw;
            }
      }
      public DataProcessingOperation Run()
      {
            try
            {
                  var result = new DataProcessingOperation();
                  var entities = new List<T>();

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

                  Read = true;
                  LineContext.Content = File.ReadAllLines(file);
                  LineContext.ImporterOptions = _fileImportOptions;

                  for (int i = 0; i < LineContext.Content.Length; i++)
                  {
                        if (_fileImportOptions.FileHasHeaders)
                        {
                              if (i == 0)
                              {
                                    continue;
                              }
                        }

                        LineContext.LineContent = LinePreprocessor(LineContext.Content[i]);
                        LineContext.CurrentIndex = i;
                        
                        DataProcessingOperation lineConverResult = LineParserExecution(LineContext, result);

                        var entity = lineConverResult.GetResult().GetData();

                        if (lineConverResult.Success)
                        {
                              entities.Add((T)entity);
                              continue;
                        }

                        foreach (string s in lineConverResult.Errors)
                        {
                              result.AddError(s);
                        }
                        entities.Add((T)entity);
                  }

                  result.Finish(entities);

                  return result;
            }
            catch
            {
                  throw;
            }
      }
      private DataProcessingOperation LineParserExecution(ParserLineContext context, DataProcessingOperation parserProcessorResult)
      {
            var result = _convertParserFunction(context);
           
            if (!result.Success)
            {
                  foreach (string s in result.Errors)
                  {
                        parserProcessorResult.AddError(String.Format("Error: {0} Line: {1}", s, context.LineIndex));
                  }
            }

            return parserProcessorResult;

      }
      public string LinePreProcessorFunction(string line)
      {
            return line;
      }
      #endregion
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
