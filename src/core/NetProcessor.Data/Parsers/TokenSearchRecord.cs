namespace NetProcessor.Data.Parsers;

public record class TokenSearchRecord<TEnumToken>
{
      public  int Index { get; set; }
      public TEnumToken Token { get; set; }
      public string Value { get; set; }
}
