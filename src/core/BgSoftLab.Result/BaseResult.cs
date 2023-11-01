using System.Diagnostics;

namespace BgSoftLab.Results;

public abstract class BaseResult
{
      private Stopwatch _stopwatch;
      private Result _result;
      private IList<string> _errors;
      private object? _data;
      private ResultStatus _status;

      public Result Result { get => _result; }
      public IList<string> Errors { get => _errors; }
      public object? Data { get => _data; }
      public ResultStatus Status { get => _status; }
      public bool Success
      {
            get
            {
                  return _result.Success;
            }
      }
      public BaseResult()
      {
            _stopwatch = new Stopwatch();
            _errors = new List<string>(10);
            _status = ResultStatus.NotInitiated;
      }
      public virtual void Start()
      {
            _stopwatch.Start();
            _status = ResultStatus.InProgress;
      }
      public virtual void Finish()
      {
            _stopwatch.Stop();
            _status = ResultStatus.Finished;
            _result = new CreateResult().Ok();
      }

      public virtual void Finish(object referenceData)
      {
            _stopwatch.Stop();
            _data = referenceData;
            _status = ResultStatus.Finished;
            _result = new CreateResult().Ok();
      }
      public virtual void AddError(string error)
      {
            _errors.Add(error);
      }
      public virtual void Clear()
      {
            _errors.Clear();
      }
      public virtual void FinishWithError(object referenceData)
      {
            var resultCreation = new CreateResult();
            _stopwatch.Stop();
            _data = referenceData;
            _result = resultCreation.Failure(Errors.ToArray());
      }
      public virtual (Result, T) GetResult<T>()
      {
            if (_data is null)
            {
                  throw new NullReferenceException(nameof(_data));
            }
            return (_result, (T)_data);
      }
}

public enum ResultStatus
{
      NotInitiated,
      InProgress,
      Finished
}
