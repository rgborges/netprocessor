namespace NetProcessor.Data.Importer;

public class ImporterSourceOptions
{
      public string SourcePath { get; set; }
      public bool ConsiderManyFiles { get; set; } = false;
      public bool Filter { get; set; } = false;
      public string Pattern { get; set; } = string.Empty;

}
