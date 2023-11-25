
using System.Reflection.Metadata.Ecma335;
using BgSoftLab.Results;


namespace NetProcessor.Data.Importer;

public abstract class BaseImporter<T>
{
      public string[] ContentWindow { get; private set; }
      public ParserLineContext LineContext { get; private set; }

      private readonly ILineParserConverter<string, T> _lineParser;
      
      public BaseImporter(ILineParserConverter<string, T> lineParser)
      {
            LineContext = new ParserLineContext();
            _lineParser = lineParser;
      }

      public virtual void UpdateContextLine()
      {
            if (ContentWindow is null)
            {
                  throw new NullReferenceException(nameof(LineContext.LineIndex));
            }
            for (int index = 0; index <= ContentWindow.Length; index++)
            {
                  LineContext.LineIndex = index;
                  
                  //todo: do a function for line Parser


            }

      }

      public virtual string LinePreprocessor(string s)
      {
            return s;
      }
      public virtual string TokenPreprocessor(string s)
      {
            return s;
      }

      public virtual T ParseLine(string line)
      {
            //todo: transform a string line in T        
            throw new NotImplementedException();
      }
}
