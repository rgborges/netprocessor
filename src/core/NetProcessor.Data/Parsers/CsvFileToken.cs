
namespace NetProcessor.Data.Parsers;

public class CsvFileToken : ITextTokenRule<CsvFileTokens>
{

      private TokenDefinition<CsvFileTokens> _definitions;

      public CsvFileToken()
      {
            this._definitions = new TokenDefinition<CsvFileTokens>();
      }
      public Dictionary<char, CsvFileTokens> GetTokensDefinition()
      {
            _definitions.AddDefinition('\n', CsvFileTokens.EndOfLine);
            _definitions.AddDefinition('"', CsvFileTokens.OpenContent);
            _definitions.AddDefinition(',', CsvFileTokens.SeparateChar);
            return _definitions.TokensDefinition;
      }
}

public enum CsvFileTokens
{
      EndOfLine,
      Content,
      SeparateChar,
      OpenContent
}
