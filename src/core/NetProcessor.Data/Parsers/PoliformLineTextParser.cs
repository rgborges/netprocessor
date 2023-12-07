using NetProcessor.Data.Importer;

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
            //try get the key of the type

            var type = typeof(T);

            if (!_map.ContainsKey(type))
            {
                  _map.Add(type, rule);
            }

            return;
      }

    public override object ParseLine(string line)
    {
        var splitChar = base.FileImporterOptions.ColumnDelimiterChar;
       
        string[] contents = line.Split('|');

        string typeReference = contents[0];

        if (_map.ContainsKey(Type.GetType(typeReference)))
        {
            //TODO: Consider other data structure
        }

        throw new NotImplementedException();
    }
    public void Clear()
      {
            _map.Clear();
      }
}
