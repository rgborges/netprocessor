using System.Reflection.Metadata;
using NetProcessor.Data.Importer;
using NetProcessor.Data.Result;

namespace NetProcessor.Data.Parsers;

// public class PoliformLineTextParser : TextParser
// {
//       private Dictionary<string, LineRule<CsvTokens>> _map;
//       public PoliformLineTextParser(TextParserOptions options) : base(options)
//       {
            
//       }

//       public void AddLineRule<T>(LineRule<CsvTokens> rule)
      
//       {
//             var type = typeof(T).ToString();

//             if (!_map.ContainsKey(type))
//             {
//                   _map.Add(type, rule);
//             }
//             return;
//       }

//       public override DataProcessingOperation ParseLine(string line)
//       {

//             return DataCommandHandler.Run((result) =>
//             {
//                   var splitChar = base.FileImporterOptions.ColumnDelimiterChar;

//                   string[] contents = line.Split('|');

//                   string typeReference = contents[0].Replace('|', ' ').TrimEnd();
                  
//                   if (_map.ContainsKey(typeReference))
//                   {
//                         var rule = _map[typeReference];

//                         throw new NotImplementedException();
//                   }
//             });
//       }
//       public void Clear()
//       {
//             _map.Clear();
//       }
// }
