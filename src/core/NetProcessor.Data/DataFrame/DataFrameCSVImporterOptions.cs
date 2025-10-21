using System.Text;

namespace NetProcessor.Data.DataFrame;

public record class DataFrameCSVImporterOptions
{
    public string FilePath { get; set; }
    public char Delimiter { get; set; }
    public Encoding Enconding{ get; set; }
};