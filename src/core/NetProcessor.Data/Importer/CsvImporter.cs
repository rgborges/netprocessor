namespace NetProcessor.Data;

public class CsvImporter<TRecord>
{
      private readonly FileInfo _fileInfo;
      private bool _useInvarianCulture;

      public CsvImporter(FileInfo fileInfo, bool useInvarianCulture)
      {
            _fileInfo = fileInfo;
            _useInvarianCulture = useInvarianCulture;
      }

      public IEnumerable<TRecord> ReadAll()
      {
            try
            {
                  if (_fileInfo is null)
                  {
                        return Enumerable.Empty<TRecord>();
                  }
                  string file = _fileInfo?.Directory?.FullName;
                  var content = File.ReadAllLines(file);
            }
            catch
            {
                  throw;
            }
            throw new NotImplementedException();
      }
}
