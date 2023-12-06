using NetProcessor.Data.Importer;

namespace NetProcessor.Data.Parsers;

/// <summary>
/// Defines a generic file parser.
/// </summary>
public abstract class TextParser<T, TEnumTokenType>
{
      private readonly FileImporterOptions _options;
      private readonly ITextTokenRule<TEnumTokenType> _tokenRules;
      public TextParser(ITextTokenRule<TEnumTokenType> tokenRules,
      FileImporterOptions options)
      {
            _options = options;
            _tokenRules = tokenRules;
      }
      /// <summary>
      /// This function will executed before executing the line parser.
      /// </summary>
      public virtual void SetLinePreProcessorFunction()
      {

      }
      /// <summary>
      /// Defines the function that will parse each string line in the file.
      /// </summary>
      public virtual void SetLineTextParserFunction<TSource>(ILineParserConverter<TSource, T> lineTextParser)
      {

      }
      /// <summary>
      /// Defines the File Tokens
      /// </summary>
      public virtual void SetTokens()
      {

      }
      /// <summary>
      /// This post processos is executed after data Token Identification.
      /// </summary>
      public virtual void SetPostProcessor()
      {

      }
      /// <summary>
      /// Run the parser against the file.
      /// </summary>
      public virtual void Run()
      {
            try
            {

            }
            catch
            {
                  
            }
      }
}
