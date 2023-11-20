using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Reflection;

namespace NetProcessor.Data.Importer;

public class ColumnTypeConfigurator<T>
{
      private Dictionary<string, FileColumnReference> _columnsType;
      private List<string> _column;
      public ColumnTypeConfigurator()
      {
            _columnsType = new Dictionary<string, FileColumnReference>();
            _column = new List<string>();
      }

      public void Add(Func<T, string> func, string columnName, string typeName, int? index = null)
      {
            T tmp = Activator.CreateInstance<T>();

            int? GetIndex()
            {
                  if (index is null)
                  {
                        return 0;
                  }
                  else
                  {
                        return index;
                  }
            }

            var columnReference = new FileColumnReference
            {
                  ConverToType = Type.GetType(typeName),
                  FileColumnName = columnName,
                  ClassPropertyName = func(tmp),
                  FileColumnIndex = GetIndex() ?? 0
            };
            _column.Add(columnName);
            _columnsType.Add(columnName, columnReference);
      }

}
