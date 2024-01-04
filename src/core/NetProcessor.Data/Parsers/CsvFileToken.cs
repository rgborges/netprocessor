namespace NetProcessor.Data.Parsers;

public class CsvTokenizer : ITextTokenRule<CsvFileTokens>, IToken
{

      private TokenDefinition<CsvFileTokens> _definitions;

      public CsvTokenizer(CsvTokenizerConfiguration config = null)
      {
            this._definitions = new TokenDefinition<CsvFileTokens>();
            _definitions.AddDefinition('\n', CsvFileTokens.EndOfLine);
            _definitions.AddDefinition('"', CsvFileTokens.OpenContent);
            if (config is not null)
            {
                  config = new CsvTokenizerConfiguration();
            }
            _definitions.AddDefinition(config.CsvSpliChar, CsvFileTokens.SeparateChar);

      }
      public Dictionary<char, CsvFileTokens> GetTokensDefinition()
      {
            return _definitions.TokensDefinition;
      }

      public TokenSearchRecord[] GetTokens(string sentence)
      {
            var tokenSearchList = new List<TokenSearchRecord>();
            int cursor = 0;
            for (int i = 0; i < sentence.Length; i++)
            {
                  if (_definitions.TokensDefinition.ContainsKey(sentence[i]))
                  {

                        var key = _definitions.TokensDefinition[sentence[i]];

                        switch (key)
                        {
                              case CsvFileTokens.SeparateChar:
                                    tokenSearchList.Add(new TokenSearchRecord()
                                    {
                                          Token = CsvFileTokens.Content,
                                          Value = sentence.Substring(cursor, i - cursor),
                                          Index = i - 1
                                    });
                                    cursor = i;

                                    tokenSearchList.Add(new TokenSearchRecord()
                                    {
                                          Token = CsvFileTokens.SeparateChar,
                                          Value = sentence.Substring(cursor, 1),
                                          Index = i
                                    });
                                    cursor++;
                                    break;
                              case CsvFileTokens.EndOfLine:
                                    {
                                          tokenSearchList.Add(new TokenSearchRecord()
                                          {
                                                Token = CsvFileTokens.Content,
                                                Value = sentence.Substring(cursor, i - cursor),
                                                Index = i - 1
                                          });
                                          cursor = i;

                                          tokenSearchList.Add(new TokenSearchRecord()
                                          {
                                                Token = CsvFileTokens.EndOfLine,
                                                Value = sentence.Substring(cursor, i - cursor),
                                                Index = i
                                          });
                                          cursor++;
                                          break;
                                    }
                        }
                  }
            }

            return tokenSearchList.ToArray();
      }
}
public class CsvTokenizerConfiguration
{
      public char CsvSpliChar { get; set; } = ',';
      public bool HasHeders { get; set; } = true;
}

public enum CsvFileTokens
{
      EndOfLine,
      Content,
      SeparateChar,
      OpenContent
}
