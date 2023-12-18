namespace NetProcessor.Data.Parsers;

public class TokenDefinition<TEnumType>
{
      private Dictionary<char, TEnumType> _tokens;
      public Dictionary<char, TEnumType> TokensDefinition { get => _tokens; }

      public void AddDefinition(char symbol, TEnumType t)
      {
            _tokens.Add(symbol, t);
      }
      public void Clear()
      {
            _tokens.Clear();
      }
}
