using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
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
public abstract class TextParser
{
      public string CurrentLine { get => Context.LineContent; }
      public int LineIndex { get => Context.CurrentIndex; }
      public string Path { get; set; } = string.Empty;
      public ParserLineContext Context { get; set; }
      public FileImporterOptions FileImporterOptions { get; private set; }
      public TextParser(TextParserOptions options)
      {
            if (options is null)
            {
                  throw new NullReferenceException(nameof(options));
            }
            FileImporterOptions = options.FileOptions;
            Context = new ParserLineContext();
      }
      public virtual void OnLineParsing()
      {

      }
      public virtual string LineStringProcessor(string s)
      {
            return s;
      }
      public virtual DataProcessingOperation ParseLine(string line)
      {
            var result = new DataProcessingOperation();
            result.Start();
            result.Finish(line);
            return result;
      }
      public DataProcessingOperation ParseFile()
      {
            if (String.IsNullOrEmpty(FileImporterOptions.Path))
            {
                  throw new FileNotFoundException();
            }

            if (!Directory.Exists(FileImporterOptions.Path))
            {
                  throw new FileNotFoundException();
            }
            if (FileImporterOptions.MultipleFiles)
            {
                  throw new NotImplementedException("Multiple files is not yet supported on Text Parsers");
            }

            Path = FileImporterOptions.Path;

            if (Context is null)
            {
                  Context = new ParserLineContext();
            }

            var result = ParserTextLines(File.ReadAllLines(Path));

            return result;
      }
      public DataProcessingOperation ParserTextLines(string[] lines)
      {
            var result = new DataProcessingOperation();
            List<DataProcessingOperation> operations = new List<DataProcessingOperation>(lines.Length);
            result.Start();
            for (int i = 0; i < lines.Length; i++)
            {
                  Context.CurrentIndex = i;
                  Context.LineContent = LineStringProcessor(lines[i]);
                  OnLineParsing();
                  operations.Add(ParseLine(Context.LineContent));
            }

            result.Finish();
            return result;
      }

}