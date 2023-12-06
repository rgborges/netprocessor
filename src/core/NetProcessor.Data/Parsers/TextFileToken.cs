namespace NetProcessor.Data.Parsers;

public record class TextFileToken<TEnumToken>
{
      public TEnumToken Token { get; set; }
      public char Value { get; set;}
}
