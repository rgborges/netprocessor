namespace NetProcessor.Data.Parsers;



/// <summary>
/// Generic parser parse CSV line
/// </summary>
public struct CsvParser()
{
    /// <summary>
    /// Parses a line string of a CSV file
    /// </summary>
    /// <param name="line"></param>
    /// <returns></returns>
    public TokenSearchRecord[] ParseLine(string line, IToken tokens)
    {
        if(string.IsNullOrEmpty(line))
        {
            return Array.Empty<TokenSearchRecord>();
        }

        return tokens.GetTokens(line);
    }
}

