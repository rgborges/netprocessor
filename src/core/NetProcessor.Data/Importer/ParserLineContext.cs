using NetProcessor.Data.Parsers;

namespace NetProcessor.Data.Importer;


/// <summary>
/// Representes the context of the current line of the parser
/// </summary>
public class ParserLineContext
{
      public string[] Content { get; set; }
      public int LineIndex { get; set; }
      public string LineContent { get; set; }
      public int CurrentIndex { get; internal set; }
      public int LastIndex { get; set; }
      public FileImporterOptions ImporterOptions { get; set; }
      public Dictionary<char, CsvFileTokens> TokenDefinition { get; internal set; }
      public char Character { get; internal set; }


      /// <summary>
      /// Get n row number based on your current position. If the param number is positive, 
      /// the reference will look for by n line fowards, if negative it will do it backwards.
      /// </summary>
      /// <param name="number"></param>
      /// <returns></returns>
      public string GetReferenceRow(int number)
      {
            try
            {
                  if (number > 0)
                  {
                        if ((LineIndex + number) > Content.Length)
                        {
                              throw new InvalidOperationException($"The line searched on {nameof(GetReferenceRow)} is invalid for the current context");
                        }
                        return Content[LineIndex + number];
                  }

                  if ((LineIndex - number) < 0)
                  {
                        return Content[0];
                  }

                  return Content[LineIndex - number];
            }
            catch
            {
                  throw;
            }

      }
      public char GetNextChar()
      {
            if (CurrentIndex + 1 < LineContent.Length)
            {
                  return LineContent[CurrentIndex + 1];
            }
            return LineContent[CurrentIndex];
      }
      public void Next()
      {
            CurrentIndex++;
            Character = LineContent[CurrentIndex];
      }

}
