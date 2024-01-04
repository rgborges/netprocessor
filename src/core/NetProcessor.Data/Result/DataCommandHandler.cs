namespace NetProcessor.Data.Result;

public static class DataCommandHandler
{
      public static DataProcessingOperation Run(Action<DataProcessingOperation> action)
      {
            var result = new DataProcessingOperation();
            result.Start();
            try
            {
                  action(result);
            }
            catch (Exception exp)
            {
                  result.AddError(exp.ToString());
            }
            finally
            {
                  result.Stop();
            }
            return result;
      }
}
