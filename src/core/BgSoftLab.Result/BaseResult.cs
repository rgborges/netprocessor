using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

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
      public long ElapsedTime { get => _stopwatch.ElapsedMilliseconds; }
      public BaseResult()
      {
            _stopwatch = new Stopwatch();
            _stopwatch.Start();
            _errors = new List<string>(10);
            _status = ResultStatus.InProgress;
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

            if (_errors.Any())
            {
                  _result = new CreateResult().Failure(_errors.ToArray());
            }
            else
            {
                  _result = new CreateResult().Ok();
            }
      }
      public virtual void AddError(string error)
      {
            _errors.Add(error);
      }
      public virtual void Clear()
      {
            _errors.Clear();
      }
      public virtual void FinishWithError(object referenceData, string error)
      {
            var resultCreation = new CreateResult();
            _stopwatch.Stop();
            _data = referenceData;
            _errors.Add(error);
            _result = new CreateResult().Failure(_errors.ToArray());
      }
      public virtual ResultData<T> GetResult<T>()
      {
            if (_data is null)
            {
                  throw new NullReferenceException(nameof(_data));
            }
            return new ResultData<T>(_result, (T)_data);
      }
}

public enum ResultStatus
{
      NotInitiated,
      InProgress,
      Finished
}
