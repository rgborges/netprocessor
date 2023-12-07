namespace NetProcessor.Data.Parsers;

public record class LineRuleDefinitions
{
      public char SplitByChar { get; set; } = ' ';
      public char EndOdLine { get; set; } = '\n';
}
