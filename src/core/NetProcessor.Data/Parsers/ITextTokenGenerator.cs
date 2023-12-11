namespace NetProcessor.Data.Parsers;

public interface ITextTokenRule<TEnumType>
{
       Dictionary<char, TEnumType> GetTokensDefinition();
}
