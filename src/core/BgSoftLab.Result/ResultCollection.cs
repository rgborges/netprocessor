namespace BgSoftLab.Results;

public class ResultCollection
{
      private List<BaseResult> _results;
      public ResultCollection(int number = 100)
      {
            _results = new List<BaseResult>(number);
      }

      public void Add(BaseResult result)
      {
            _results.Add(result);
      }

      public void Clear()
      {
            _results.Clear();
      }
}

public class ResultCollection<T>
      where T : BaseResult
{
      private List<T> _results;
      public ResultCollection(int number = 100)
      {
            _results = new List<T>(number);
      }

      public void Add(T result)
      {
            _results.Add(result);
      }
      public void Clear()
      {
            _results.Clear();
      }

      public object[] GetValues()
      {
            var values = new List<object>();
            foreach (var result in _results)
            {

                  values.Add(result.GetResult().GetData());
            }
            return values.ToArray();
      }
}
