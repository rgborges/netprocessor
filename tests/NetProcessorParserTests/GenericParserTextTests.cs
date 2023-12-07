using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;
using NetProcessor.Data.Parsers;

namespace NetProcessorParserTests;

[TestClass]
public class GenericParserTextTests
{
    [TestMethod]
    public void ParseSimpleCsvText()
    {
        string[] parserLines = new[] {
            "Name, Age",
            "Gabriel, 29"
        };

        var dir = String.Concat<string>(new[] { Path.GetTempFileName().Replace(".tmp", ""), ".csv" });

        if (!File.Exists(dir))
        {
            File.WriteAllLines(dir, parserLines);
        }
        try
        {
            var result = CSV.ReadAll<DTO>(dir);

            var data = result.GetResult().GetDataAsList<DTO>();

            Assert.AreEqual("Gabriel", data[0].Name);

        }
        catch
        {
            throw;
        }
        finally
        {
            File.Delete(dir);
        }
    }

    [TestMethod]
    public void General_TextParserDefinition()
    {
        string[] linesText = new [] { 
            "R89774|1245|Rafael",
            "R9999|124|1|12|T"
        };

        // var genTextParser = new PoliformLineTextParser();

        // var R89774Rule = new LineRule()
        //                         .OfType<R89774>()
        //                         .SplitByChar('|');

        // var R9999Rule = new LineRule()
        //                         .OfType<R9999>()
        //                         .SplitByChar('|');



    }

    public record class R89774
    {
        public string Id { get; set; } = string.Empty;
        public int Number { get; set; }
        public string Name { get; set; } = string.Empty;
    }
    public record class R9999
    {
        public string Id { get; set; } = string.Empty;
        public int Measure { get; set; }
        public int Validation { get; set; }
        public int Quantity { get; set; }
        public char EventType { get; set; }

    }
    public record class DTO
    {
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }
    }
}