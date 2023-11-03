namespace BgSoftLab.Results;

public record class ResultData<T>
{
      public Result Result { get; private set; }
      public T Data {get; private set; }

      public ResultData(Result result, T data)
      {
            Data = data;
            Result = result;
      }


      public T GetData()
      {
            return Data;
      }
}
