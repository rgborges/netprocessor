namespace BgSoftLab.Results;

public struct Result
{
      public readonly bool Success { get; }
      public readonly string[] Errors { get; }

      public Result(bool success, string[] errors)
      {
            Errors = errors;
            Success = success;
      }
}
