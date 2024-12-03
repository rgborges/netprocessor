using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using NetProcessor.Data.Importer;
using NetProcessor.Data.Result;

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
      /// This function will execute before executing the line parser.
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
      /// This post process is executed after data Token Identification.
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
/// <summary>
///Defines a base text file importer which context line by line 
/// </summary>
public abstract class TextImporter
{
      public string CurrentLine { get => Context.LineContent; }
      public int LineIndex { get => Context.CurrentIndex; }
      public string Path { get; set; } = string.Empty;
      public Importer.ParserLineContext Context { get; set; }
      public FileImporterOptions FileImporterOptions { get; private set; }
      public TextImporter(TextParserOptions options)
      {
            if (options is null)
            {
                  throw new NullReferenceException(nameof(options));
            }
            FileImporterOptions = options.FileOptions;
        Context = new Importer.ParserLineContext();
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
            Context = new Importer.ParserLineContext();
            }

            var result = ParserTextLines(File.ReadAllLines(Path));

            return result;
      }
      public DataProcessingOperation ParserTextLines(string[] lines)
      {
            return DataCommandHandler.Run((result) =>
            {
                  List<DataProcessingOperation> operations = new List<DataProcessingOperation>(lines.Length);
                  for (int i = 0; i < lines.Length; i++)
                  {
                        Context.CurrentIndex = i;
                        Context.LineContent = LineStringProcessor(lines[i]);
                        OnLineParsing();
                        var lineProcessResult = ParseLine(Context.LineContent);
                        if (!lineProcessResult.Success)
                        {
                              foreach (string e in lineProcessResult.Errors)
                              {
                                    result.AddError(
                                          String.Format("On text line {0} Error: {1}\n", Context.CurrentIndex, e)
                                    );
                              }
                        }
                        operations.Add(lineProcessResult);
                  }
            });
      }
}