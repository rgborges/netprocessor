using System.Data;

namespace NetProcessor.Data.Result.Interfaces;

/// <summary>
/// Provides an interface for converting results to DataTable and other 
/// Data Views.
/// </summary>
public interface IDataConvertResult<T>
{
      public string[] ColumnsNames { get; }
      public T[] Rows { get; }

      DataTable ToDataTable(T data);
}
