using System.Security.Cryptography.X509Certificates;

namespace NetProcessor.Data.Parsers;


/// <summary>
/// Stores the a rule for a text parser. This rule is used in each line context.
/// </summary>
public class LineRule
{
      public LineRule()
      {
            
      }
      public LineRule OfType<T>()
      {
            return this;
      }
      public LineRule SplitByChar(char c)
      {
            return this;
      }

}
