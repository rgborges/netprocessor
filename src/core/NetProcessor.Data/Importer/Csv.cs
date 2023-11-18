using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace NetProcessor.Data;

public static class CSV
{
      public static DataResult Parse(string filepath)
      {
            throw new NotImplementedException();
      }

      public class DataResult
      {
            public DataTable ToDataTable()
            {
                  throw new NotImplementedException();
            }
            public List<T> ToList<T>()
            {
                  throw new NotImplementedException();
            }
            public T[][] ToMatrix<T>()
            {
                  throw new NotImplementedException();
            }

      }

}

