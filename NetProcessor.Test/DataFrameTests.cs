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
        
        Assert.Equal((3,2), df.Shape);
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

        var rows = df.GetRows(0, 1);
        
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
        var dfColumn = new DataFrameColumn("city", new [] { "São Paulo", "Rio de Janeiro", "Bahia" });

        Assert.Equal("city", dfColumn.Name);

        var dfAges = new DataFrameColumn("ages", [ 12, 15, 26, 24 ]);

        Assert.Equal(4, dfAges.Size);

        dfAges.Apply<Int32>((int input) => 
            input = input + 2
        );

        var values = dfAges.GetValues();
    }

    [Fact]
    public void Import_From_CSV()
    {
        var df = DataFrame.FromCSV("C:\\Workspace\\Data\\Admission_Predict_Ver1.1.csv");

        Assert.NotNull(df);
    }


    [Fact]
    public void Dummy()
    {
        var arr = Enumerable.Range(0, 100).ToArray();

        var partArr = arr[1..3];
        var partArr2 = arr[1..];

        Assert.NotNull(partArr);
    }
    
}