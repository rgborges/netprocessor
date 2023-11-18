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
}).SetLineParserFunction( (string[] lines) => {
      var result = new ParserResult();

      return result;
}).ReadAllLines();

{
  if (!result.Success)
      System.Console.WriteLine("Errors has occured:");
      foreach (string s in result.Errors)
      {
            System.Console.WriteLine(s);
      }
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
