using System.Linq.Expressions;

namespace NetProcessor.Data.Parsers;

public static class TokenSearchExtentions 
{
      public static TokenSearchRecord[] Get(this TokenSearchRecord[] obj, Predicate<TokenSearchRecord> expresion)
      {
            var result = new List<TokenSearchRecord>();
            foreach (var record in obj)
            {
                  if (expresion(record))
                  {
                        result.Add(record);
                  }
            }
            return result.ToArray();
      }
}
