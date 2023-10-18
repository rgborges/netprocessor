namespace NetProcessor.Data.Importer;
/// <summary>
/// Generates an importer from the specified type. 
/// </summary>
public class ImporterBuilder
{
      private ImporterSourceOptions _sourceOptions;
      public ImporterBuilder()
      {
            _sourceOptions = new ImporterSourceOptions();
      }
      public ImporterBuilder FromSourceFile(Action<ImporterSourceOptions> actionOptions)
      {
            actionOptions(_sourceOptions);
            return this;
      }
      public ISourceLocationConfiguration SetLocation(Action<ImporterLocationOptions> options)
      {
            throw new NotImplementedException();
      }
      
      
}



public interface ISourceLocationConfiguration
{
}
public interface IDestinationConfiguration
{

}

public class ImporterLocationOptions
{
      
}
