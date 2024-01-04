using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;
using NetProcessor.Data.Importer;
using NetProcessor.Data.Result;

namespace NetProcessor.Data.Parsers;


/// <summary>
/// Stores the a rule for a text parser. This rule is used in each line context.
/// </summary>
public class LineRule<TEnumType>
{
      private object _objectType = null;
      private ITextTokenRule<TEnumType> _tokens;
      private List<Action<char>> _actions;
      public char SplitChar { get; set; }
      public string LineContent { get; set; } = string.Empty;
      private Func<ParserLineContext, TokenSearchRecord> _processFunc;
      private Action<ParserLineContext, TokenSearchRecord> _processAction;
      public ParserLineContext Context { get; set; }
      public new Type GetType
      {
            get
            {
                  if (_objectType is null)
                  {
                        throw new NullReferenceException($"The method OfType was not used. {nameof(_objectType)}");
                  }
                  return _objectType.GetType();
            }
      }
      public LineRule(ITextTokenRule<TEnumType> token)
      {
            _tokens = token;
            _actions = new List<Action<char>>();
      }
      public LineRule<TEnumType> SpecifyRule(Action<ParserLineContext, TokenSearchRecord> action)
      {
            _processAction = action;
            return this;
      }
      public LineRule<TEnumType> OfType<T>()
      {
            _objectType = Activator.CreateInstance<T>();

            return this;
      }
      public LineRule<TEnumType> SplitByChar(char c)
      {
            this.SplitChar = c;
            return this;
      }
      public ICollection<TokenSearchRecord> FindTokens<T>(string content)
      {
            var result = new List<TokenSearchRecord>();

            var dicToken = _tokens.GetTokensDefinition();
            int lastIndex = 0;
            for (int i = 0; i < LineContent.Length; i++)
            {
                  if (dicToken.ContainsKey(LineContent[i]))
                  {
                        var _ = new TokenSearchRecord()
                        {
                              Index = i,
                              Token = dicToken[LineContent[i]],
                              Value = LineContent.Substring(lastIndex, i)
                        };
                        result.Add(_);
                        lastIndex = i;
                  }
            }
            return result;
      }
      public ICollection<DataProcessingOperation> Run(string lineContent)
      {
            this.LineContent = lineContent;

            var result = new List<DataProcessingOperation>();

            if (Context is null)
            {
                  Context = new ParserLineContext();
            }
            if (_processAction is null)
            {
                  throw new NullReferenceException("None process function was configured in the object. Please Call SepecfyRule at least once");
            }

            Context.TokenDefinition = new CsvTokenizer().GetTokensDefinition();

            Context.LastIndex = 0;

            for (int i = 0; i < LineContent.Length; i++)
            {
                  var charResult = DataCommandHandler.Run((result) =>
                  {
                        Context.CurrentIndex = i;
                        Context.LineContent = LineContent;
                        Context.Character = LineContent[i];
                        var tsearch = new TokenSearchRecord();
                        _processAction(this.Context, tsearch);
                        result.Finish(tsearch);
                  });

                  var data = charResult.GetResult().GetData<TokenSearchRecord>();

                  if (data.Token is not null)
                  {
                        result.Add(charResult);
                  }

                  Context.Next();
            }

            return result;

      }
}
