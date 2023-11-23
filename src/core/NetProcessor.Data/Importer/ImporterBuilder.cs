namespace NetProcessor.Data.Importer;
/// <summary>
/// Generates an importer from the specified type. 
/// </summary>
public class ImporterBuilder<T> 
{
      /// TODO: The importer builder has to be the ability to create different builders with all configuraiton needes
/// run the builder in any format
      private ImporterSourceOptions _sourceOptions;
      private ImporterLocationOptions _localtionOptions;
      private ColumnTypeConfigurator<T> _columnConfigurator;
      public ImporterBuilder()
      {
            _sourceOptions = new ImporterSourceOptions();
      }
      public ImporterBuilder<T> FromSourceFile(Action<ImporterSourceOptions> actionOptions)
      {
            actionOptions(_sourceOptions);
            return this;
      }
      public ImporterBuilder<T> OverrideColumnsConfiguratior(Action<ColumnTypeConfigurator<T>> action)
      {
            action(_columnConfigurator);
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
