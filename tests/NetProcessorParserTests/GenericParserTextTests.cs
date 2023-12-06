using System.Runtime.CompilerServices;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;

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
    public record class DTO
    {
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }
    }
}