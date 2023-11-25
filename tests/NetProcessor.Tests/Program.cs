// See https://aka.ms/new-console-template for more information
using System.Runtime.CompilerServices;
using NetProcessor.Data;
using NetProcessor.Data.Importer;

string path = @"/home/rborges/tmp/datasets/winemag-data_first150k.csv";

// var builder = new ImporterBuilder();

// builder.FromSourceFile(file => {
//       file.SourcePath = "~/tmp/test.csv";
// }, fileConfiguration => {
//       fileConfiguration.FileType = CsvFile;
//       fileConfiguration.Delimeter = ';';
// }).Read<ExampleDto>(MethodReadAll).ToJson();

// //CASE 01 - Direct parsing into a list with traditional and consumming read method
// var result = new CsvParser<DTO>(path, options =>
// {
//       options.ColumnDelimiterChar = ',';
//       options.UseSmallCasePropertiesComparison = true;
// }).ReadAllLines();

// //CASE 02 - Custom column configurator from the file with readall lines method
// var result2 = new CsvParser<DTO>(path, options =>
// {
//       options.ColumnDelimiterChar = ',';
// }).OverrideColumnsConfiguration((config) =>
// {
//       config.Add(x => nameof(x.Id), "id", "System.Int32");
//       config.Add(x => nameof(x.Country), "country", "System.String");
// })
// .ReadAllLines();

//CASE 4

var result = CSV.ReadAll<DTO>(path, delimiter: ',', smallCaseCompare: true);

if (!result.Success)
{
      System.Console.WriteLine("Errors has occured:");
      foreach (string s in result.Errors)
      {
            System.Console.WriteLine(s);
      }
      
      var data = result.GetResult()
            .GetDataAsList<DTO>();
}

System.Console.WriteLine("It was successfull");

// var runner = builder.Build();

// runner.Run();




public class DTO
{
      public string Id { get; set; } = string.Empty;
      public string Country { get; set; } = string.Empty;
      public string Description { get; set; } = string.Empty;
      public string Designation { get; set; } = string.Empty;
      public string Points { get; set; } = string.Empty;
      public string Price { get; set; } = string.Empty;
      public string Province { get; set; } = string.Empty;
      public string Region_1 { get; set; } = string.Empty;
      public string Region_2 { get; set; } = string.Empty;
      public string Variety { get; set; } = string.Empty;
      public string Winery { get; set; } = string.Empty;
      //todo: Id,country,description,designation,points,price,province,region_1,region_2,variety,winery"
}
