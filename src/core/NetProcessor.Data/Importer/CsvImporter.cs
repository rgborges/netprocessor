using System.Collections;
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
                        object record = new object();

                        if (i == 0) { continue; }
//TODO: In this case the description has ' sign, and this alse are the split char for this file. The description has "" sign, which means to do not 
//consideer the commas.
                        string[] splitRowData = content[i].Split(_fileImportOptions.ColumnDelimiterChar);

                        if (columns.Length != splitRowData.Length)
                        {
                              return new List<TRecord>(1);
                        }

                        for (int j = 0; j < columns.Length; j++)
                        {
                              properties[i].SetValue(record, splitRowData[j]);
                        }

                        result.Add((TRecord)record);
                  }
                  return result;
            }
            catch
            {
                  throw;
            }
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
