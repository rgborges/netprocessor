namespace NetProcessor.Data.Importer;

public class ImporterExecutionOptions
{
      public ExecutionType ExecutionScopeType { get; set; }
      public ExectionMethod ExectionMethod { get; set; }    
}

public enum ExecutionType
{
      GenericFile
}

public enum ExectionMethod
{
     AllInMemory,
     Streaming
}

