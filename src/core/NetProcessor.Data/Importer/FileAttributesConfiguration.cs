namespace NetProcessor.Data.Importer;

public class FileAttributesConfiguration
{
      private Dictionary<string, Type> _attributeConfig;
      public Dictionary<string, Type> GetAttributes { get; set; }
      public FileAttributesConfiguration()
      {
            _attributeConfig = new Dictionary<string, Type>();
      }
      public void AddAttribute(string columnName, Type DateType)
      {
            if (string.IsNullOrEmpty(columnName)) { throw new NullReferenceException("Column name cannot be null "); }
            _attributeConfig.Add(columnName, DateType);
      }
      public void Clear()
      {
            _attributeConfig.Clear();
      }

}
