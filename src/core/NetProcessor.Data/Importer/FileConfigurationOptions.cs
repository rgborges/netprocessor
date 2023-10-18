namespace NetProcessor.Data.Importer;

public class FileConfigurationOptions
{
      public ImporterSourceFileType FileType { get; set; }
      public char Delimiter { get; set; } = ',';

}

public enum ImporterSourceFileType {
      Xml,
      Json,
      CSV,
      Excel,
      GoogleSheets
}
