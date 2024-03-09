namespace NetProcessor.Data.Parsers;

public class CsvParser<T> : TextParser
{
      private readonly CsvTokenizerConfiguration _config;
      public CsvParser(TextParserOptions options, CsvTokenizerConfiguration config) : base(options)
      {
            _config = config;
      }
      public override DataProcessingOperation ParseLine(string line)
      {
            try
            {
                  var tokenizer = new CsvTokenizer(_config);
                  var tokensResult = tokenizer.GetTokens(line);
                  var properties = typeof(T).GetProperties();
                  int propCount = properties.Count();
                  
                  var valueResult = tokensResult
                  .Where(x => (CsvFileTokens)x.Token == CsvFileTokens.Content)
                  .Where(x => !String.IsNullOrEmpty(x.Value))
                  .ToList();

                  if (valueResult.Count() != properties.Count())
                  {
                        //Add line error
                  }

                  var tmpObject = Activator.CreateInstance<T>();

                  for (int i = 0; i <= properties.Length; i++)
                  {
                        properties[i].SetValue(tmpObject, valueResult[i]);
                  }

            }
            catch
            {
                  throw;
            }
            return base.ParseLine(line);
      }
}
