namespace NetProcessor.Data.Importer;

public class FileImporterOptions
{
      public char ColumnDelimiterChar { get; set; } = ',';
      public bool Filter { get; set; }
      public string FilterExtention { get; set; } = ".csv";
      public bool UseSmallCasePropertiesComparison { get; set; } = false;
      public bool FileHasHeaders { get; set; } = true;
      public bool MultipleFiles { get; set; } = false;
      public string Path { get; set; } = String.Empty;
}
