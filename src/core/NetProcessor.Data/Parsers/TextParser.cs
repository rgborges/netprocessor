using NetProcessor.Data.Importer;

namespace NetProcessor.Data.Parsers;

/// <summary>
/// Defines a generic file parser.
/// </summary>
public abstract class TextParser<T>
{
      private readonly FileImporterOptions _options;

      public TextParser(FileImporterOptions options)
      {
            _options = options;
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
