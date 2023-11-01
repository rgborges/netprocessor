namespace BgSoftLab.Results;

public class CreateResult : IResult
{
      public Result Failure(string[] errors)
      {
            return new Result(false, errors);
      }
      public Result Ok()
      {
            return new Result(true, Array.Empty<string>());
      }
}
