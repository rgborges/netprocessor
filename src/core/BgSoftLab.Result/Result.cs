namespace BgSoftLab.Results;

public struct Result
{
      private readonly bool Success { get; }
      private readonly string[] Errors { get; }

      public Result(bool success, string[] errors)
      {
            Errors = errors;
            Success = success;
      }
}
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

public interface IResult
{
      Result Failure(string[] errors);
      Result Ok();
}
