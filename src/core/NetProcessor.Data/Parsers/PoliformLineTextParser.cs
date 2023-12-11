using NetProcessor.Data.Importer;
using NetProcessor.Data.Result;

namespace NetProcessor.Data.Parsers;

public class PoliformLineTextParser : TextParser
{
      private Dictionary<Type, LineRule> _map;
      public PoliformLineTextParser(TextParserOptions options) : base(options)
      {
            _map = new Dictionary<Type, LineRule>();
      }

      public void AddLineRule<T>(LineRule rule)
      {
            var type = typeof(T);

            if (!_map.ContainsKey(type))
            {
                  _map.Add(type, rule);
            }
            return;
      }

      public override DataProcessingOperation ParseLine(string line)
      {

            return DataCommandHandler.Run((result) =>
            {
                  var splitChar = base.FileImporterOptions.ColumnDelimiterChar;

                  string[] contents = line.Split('|');

                  string typeReference = contents[0].Replace('|', ' ').TrimEnd();

                  if (_map.ContainsKey(Type.GetType(typeReference)))
                  {
                        //TODO: Consider other data structure

                        var type = Type.GetType(typeReference);

                        var rule = _map[type];

                        throw new NotImplementedException();
                  }
            });
      }
      public void Clear()
      {
            _map.Clear();
      }
}
