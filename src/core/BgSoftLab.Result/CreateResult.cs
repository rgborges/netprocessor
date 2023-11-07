namespace BgSoftLab.Results;

internal class CreateResult
{
      internal Result Failure(string[] errors)
      {
            return new Result(false, errors);
      }

      internal Result Ok()
      {
            return new Result(true, Array.Empty<string>());
      }

}
