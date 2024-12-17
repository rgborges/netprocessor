using System;

namespace NetProcessor.Data.DataFrame;

/// <summary>
/// Interface defines a DataColumn function
/// </summary>
public interface IDataFrameColumn
{
      /// <summary>
      /// Applies a function to a column dataset.
      /// </summary>
      /// <param name="apply"></param>
      /// <returns>A new dataframe column with function applies</returns>
      public DataFrame Apply(Action<Dictionary<string, object>> apply);
}
