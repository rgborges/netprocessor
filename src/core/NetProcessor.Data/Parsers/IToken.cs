namespace NetProcessor.Data.Parsers;

public interface IToken
{
      TokenSearchRecord[] GetTokens(string sentence);
}
