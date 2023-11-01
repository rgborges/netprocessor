using System.Collections;
using BgSoftLab.Results;
using NetProcessor.Data.Importer;

namespace NetProcessor.Data;

public class CsvParser<TRecord>
{
      private readonly FileInfo _fileInfo;
      private bool _useInvarianCulture;
      private FileImporterOptions _fileImportOptions;
      public CsvParser(FileInfo fileInfo, bool useInvarianCulture)
      {
            _fileInfo = fileInfo;
            _useInvarianCulture = useInvarianCulture;
      }
      public CsvParser(FileInfo fileInfo, Action<FileImporterOptions> configAction)
      {
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
                        result.Add(property.Name, property.GetType());
                  }
                  return result;
            }
            catch
            {
                  throw;
            }
      }
      public Tuple<Result, IEnumerable<TRecord>> ReadAll()
      {
            try
            {
                  var result = new CreateResult();
                  if (_fileInfo is null)
                  {
                        return Tuple.Create(result.Failure(new string[] { "the file is null or not defined." }),
                             Enumerable.Empty<TRecord>());
                  }

                  if (_fileInfo.Extension != ".csv")
                  {
                        return Tuple.Create(result.Failure(new[] { "File extention is not supported. Make sure it's a CSV file" }),
                        Enumerable.Empty<TRecord>());
                  }
                  string file = _fileInfo?.FullName;
                  if (!_fileInfo.Exists)
                  {
                        return Tuple.Create(
                              result.Failure(new string[] { "the file is null or not defined." }),
                              Enumerable.Empty<TRecord>());

                  }
                  var content = File.ReadAllLines(file);

                  var fileHeaders = content[0].Split(_fileImportOptions.ColumnDelimiterChar);
                  //TODO: Verify if columns match with the headers
                  var typeColumns = this.GenerateColumnsAndTypes();

                  bool areEqual = StructuralComparisons.StructuralEqualityComparer.Equals(typeColumns, fileHeaders);

                  return Tuple.Create(result.Ok(), Enumerable.Empty<TRecord>());
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
