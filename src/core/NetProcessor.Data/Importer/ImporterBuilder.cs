namespace NetProcessor.Data.Importer;
/// <summary>
/// Generates an importer from the specified type. 
/// </summary>
public class ImporterBuilder
{
      public ImporterBuilder()
      {

      }
      public ISourceLocationConfiguration SetLocation(Action<ImporterLocationOptions> options)
      {
            throw new NotImplementedException();
      }
}
public interface ISourceLocationConfiguration
{
      public void SetSourcePath(string path);
}

public class ImporterLocationOptions
{
}

public class Test
{
      public Test()
      {
            var builder = new ImporterBuilder().SetLocation(options => {

            }).
      }
}