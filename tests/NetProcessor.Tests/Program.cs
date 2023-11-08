// See https://aka.ms/new-console-template for more information
using NetProcessor.Data;
using NetProcessor.Data.Importer;


string path = @"/home/rborges/tmp/datasets/winemag-data_first150k.csv";
var file = new FileInfo(path);

// var builder = new ImporterBuilder();

// builder.FromSourceFile(file => {
//       file.SourcePath = "~/tmp/test.csv";
// }, fileConfiguration => {
//       fileConfiguration.FileType = CsvFile;
//       fileConfiguration.Delimeter = ';';
// }).Read<ExampleDto>(MethodReadAll).ToJson();




var result = new CsvParser<DTO>(file, options =>
{
      options.ColumnDelimiterChar = ',';
      options.UseSmallCasePropertiesComparison = true;
}).ReadAll();

if (!result.Success)
{
      System.Console.WriteLine("Errors has occured:");
      foreach (string s in result.Errors)
      {
            System.Console.WriteLine(s);
      }
}
System.Console.WriteLine("It was successfull");
// var runner = builder.Build();

// runner.Run();
public record struct DTO
{
      public string Id {get; set;}
      public string Country { get; set; }
      public string Description { get; set; }
      public string Designation { get; set; }
      public string Points { get; set; }
      public string Price { get; set; }
      public string Province { get; set; }
      public string Region_1 { get; set; }
      public string Region_2 { get; set; }
      public string Variety { get; set; }
      public string Winery { get; set; }
      //todo: Id,country,description,designation,points,price,province,region_1,region_2,variety,winery"
}