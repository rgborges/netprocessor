using BgSoftLab.Results;
using NetProcessor.Data.Importer;

namespace NetProcessor.Data;

public class CsvImporter<TRecord>
{
      private readonly FileInfo _fileInfo;
      private bool _useInvarianCulture;
      private FileImporterOptions fileImportOptions;

      public CsvImporter(FileInfo fileInfo, bool useInvarianCulture)
      {
            _fileInfo = fileInfo;
            _useInvarianCulture = useInvarianCulture;
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
                        return Tuple.Create(
                              result.Failure(new string[] { "the file is null or not defined." }),
                              Enumerable.Empty<TRecord>());
                  }
                  string file = _fileInfo?.Directory?.FullName;

                  if (!File.Exists(file))
                  {
                        return Tuple.Create(
                              result.Failure(new string[] { "the file is null or not defined." }),
                              Enumerable.Empty<TRecord>());
 
                  }
                  var content = File.ReadAllLines(file);
                  throw new NotImplementedException();
            }
            catch
            {
                  throw;
            }
      }
}
