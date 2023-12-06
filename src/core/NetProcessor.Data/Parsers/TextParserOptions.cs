 namespace NetProcessor.Data.Parsers;

public class TextParserOptions
{
      public ParserStrategy Strategy { get; set; }
}

public enum ParserStrategy
{
      Line,
      Column,
      Tree
}
