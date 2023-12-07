using NetProcessor.Data.Importer;

namespace NetProcessor.Data.Parsers;

public class TextParserOptions
{
      public ParserStrategy Strategy { get; set; }
      public FileImporterOptions FileOptions { get; set; }
}

public enum ParserStrategy
{
      Line,
      Column,
      Tree
}
