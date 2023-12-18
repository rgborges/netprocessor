namespace NetProcessor.Data.Parsers;

public record class TokenSearchRecord
{
      public  int Index { get; set; }
      public object Token { get; set; }
      public string Value { get; set; }
}
