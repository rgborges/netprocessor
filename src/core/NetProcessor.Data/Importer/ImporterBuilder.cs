namespace NetProcessor.Data.Importer;
/// <summary>
/// Generates an importer from the specified type. 
/// </summary>
public class ImporterBuilder
{
      /// TODO: The importer builder has to be the ability to create different builders with all configuraiton needes
/// run the builder in any format
      private ImporterSourceOptions _sourceOptions;
      private ImporterLocationOptions _localtionOptions;
      public ImporterBuilder()
      {
            _sourceOptions = new ImporterSourceOptions();
      }
      public ImporterBuilder FromSourceFile(Action<ImporterSourceOptions> actionOptions)
      {
            actionOptions(_sourceOptions);
            return this;
      }
      public ISourceLocationConfiguration ToPath(Action<ImporterLocationOptions> actOnOptions)
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
