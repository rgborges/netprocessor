using System.Collections;

namespace BgSoftLab.Results;

public record class ResultData
{
      private object _data;
      private Result _result;
      public Result Result { get => _result; }

      public ResultData(Result result, object data)
      {
            _data = data;
            _result = result;
      }
      public object GetData()
      {
            return _data;
      }
      public T GetData<T>()
      {
            return (T)_data;
      }

      public List<T> GetDataAsList<T>()
      {
            return _data as List<T> ?? new List<T>();
      }
}
