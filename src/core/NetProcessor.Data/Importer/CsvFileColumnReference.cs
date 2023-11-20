namespace NetProcessor.Data.Importer;

public record class FileColumnReference
{
      public int FileColumnIndex { get; set; } 
      public Type ConverToType { get; set; } 
      public string FileColumnName { get; set; } = string.Empty;
      public string ClassPropertyName { get; set; } = string.Empty;
}
