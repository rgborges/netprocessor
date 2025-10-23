using NetProcessor.Data.DataFrame;

namespace NetProcessor.Test;

using NetProcessor.Data;

public class DataFrameTests
{
    [Fact]
    public void DataFrame_Column_Retrieval()
    {
        var df = new DataFrame("names", ["John Doe", "Maria", "Stella"])
                        .AddColumn("ages", [12, 23, 15]);

        var ages = df.Data["ages"];

        Assert.NotNull(ages);
        Assert.Equal(3, ages.Length);
    }
    [Fact]
    public void DataFrame_Shape()
    {
        var df = new DataFrame("names", ["John Doe", "Maria", "Stella"])
            .AddColumn("ages", [12, 23, 15]);

        Assert.Equal((3, 2), df.Shape);
    }
    [Fact]
    public void DataFrame_RowIndex()
    {
        var df = new DataFrame("names", ["John Doe", "Maria", "Stella"])
            .AddColumn("ages", [12, 23, 15]);

        var jhon = df[0];

        Assert.NotNull(jhon);
    }
    [Fact]
    public void DataFrame_GetRows()
    {
        var df = new DataFrame("names", ["John Doe", "Maria", "Stella"])
            .AddColumn("ages", [12, 23, 15]);

        var rows = df.GetRows(0, 1).ToArray();

        Assert.NotNull(rows);
    }
    [Fact]
    public void DataFrame_DisposeResources()
    {
        using (var df = new DataFrame("names", ["John Doe", "Maria", "Stella"])
                            .AddColumn("ages", [12, 23, 15]))
        {

        }
    }
    [Fact]
    public void DataFrame_Column()
    {
        var dfColumn = new DataFrameColumn("city", new[] { "São Paulo", "Rio de Janeiro", "Bahia" });

        Assert.Equal("city", dfColumn.Name);

        var dfAges = new DataFrameColumn("ages", [12, 15, 26, 24]);

        Assert.Equal(4, dfAges.Size);

        dfAges.Apply<Int32>((int input) =>
            input = input + 2
        );

        var values = dfAges.GetValues();

        var mean = dfAges.GetValues().Select(v => Convert.ToInt32(v)).Average();

        Assert.Equal(17, mean);

    }
    [Fact]
    public void organize_data_by_category()
    {
        var series = new DataFrameColumn("city", new[] { "São Paulo", "Rio de Janeiro", "Bahia", "São Paulo", "Bahia" });

        var g = series.GetValues()
            .GroupBy(v => v)
            .Select(g => new { City = g.Key, Count = g.Count() })
            .OrderByDescending(g => g.Count);

        Assert.NotNull(g);
    }
    [Fact]
    public void apply_function_to_column()
    {
        var r = new Random();

        var generatedData = Enumerable.Range(0, 10_000).Select(_ => r.Next(1, 100)).Cast<object>().ToArray();
        
        var series = new DataFrameColumn("ages", generatedData);

        var peopleGreaterThan50 = series.Apply<int>(v => v + 2, x => x > 50);

        var values = peopleGreaterThan50.GetValues().Select(v => Convert.ToInt32(v)).ToArray();

        Assert.NotNull(values);
    }
    [Fact]
    public void Import_From_CSV()
    {
        var path = Environment.CurrentDirectory;

        var df = DataFrame.FromCSV(@"..\..\..\..\Data\Admission_Predict_Ver1.1.csv");

        Assert.Equal(500, df.Rows);
        Assert.NotNull(df);
    }
    [Fact]
    public void Import_From_CSV_UsedCarsDataSet()
    {
        string path = Environment.CurrentDirectory;

        var df = DataFrame.FromCSV("..\\..\\..\\..\\Data\\uae_used_cars_10k.csv");

        Assert.Equal(10_000, df.Rows);
        Assert.NotNull(df);
    }
    [Fact]
    public void DataFrame_2GB_LoadTest()
    {
        //System Out of memory excetion
        string path = "C:\\tmp\\tests\\archive\\ratings.csv";

        foreach (var df in DataFrame.FromCSVInChunks(path, chunkSize: 1_000))
        {
            Assert.NotNull(df);
        }
    }
    [Fact]
    public void DataFrame_Top()
    {
        var df = new DataFrame("names", ["John Doe", "Maria", "Stella"])
            .AddColumn("ages", [12, 23, 15]);

        var top = df.Top(1).ToArray();

        Assert.NotNull(top);
    }
    [Fact]
    public void DataFrame_Filters()
    {
        //TODO: Implment filter function

        //df_filtered = df.filter(df['age'] > 21)
        var df = new DataFrame("names", ["John Doe", "Maria", "Stella"])
                   .AddColumn("ages", [12, 23, 15]);

        var dfFiltered = df.Filter("ages", (object age) => (int)age > 21);

        Assert.Equal(1, dfFiltered.Rows);

    }
    [Fact]
    public void DataFrame_GetColumns()
    {
        var df = new DataFrame("names", ["John Doe", "Maria", "Stella"])
            .AddColumn("ages", [12, 23, 15]);

        var columns = df.GetColumns(new[] { "ages" });

        Assert.NotNull(columns);
    }

    [Fact]
    public void DataFrame_Apply()
    {
        var df = new DataFrame("names", new[] { "John Doe", "Maria", "Stella" });
        
        df.Apply("names", name => name.GetHashCode());

        var i = df.GetRows(0, 1).First();
        
        Assert.NotEqual("John Doe", i);
    }
}