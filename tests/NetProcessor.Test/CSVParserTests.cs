using NetProcessor.Data.Parsers;

namespace NetProcessor.Test
{
    public class CSVParserTests
    {
        [Fact]
        void test_parse_csv_line()
        {
            string line = "This is my task;\"Quoted value\"\n";

            var tokenizer = new CsvTokenizer(new CsvTokenizerConfiguration()
            {
                CsvSplitChar = ';'
            }
            );

            var tokens = new CsvParser().ParseLine(line, tokenizer);

            Assert.NotNull(tokens);
        }
    }
}
