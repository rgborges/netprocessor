// See https://aka.ms/new-console-template for more information
using NetProcessor.Data;
using NetProcessor.Data.Importer;


string path = @"/home/rborges/tmp/datasets/winemag-data_first150k.csv";
var fileInfo = new FileInfo(path);

// var builder = new ImporterBuilder();

// builder.FromSourceFile(file => {
//       file.SourcePath = "~/tmp/test.csv";
// }, fileConfiguration => {
//       fileConfiguration.FileType = CsvFile;
//       fileConfiguration.Delimeter = ';';
// }).Read<ExampleDto>(MethodReadAll).ToJson();

var importer = new CsvParser<DTO>(fileInfo, false);
var result = importer.ReadAll();

// var runner = builder.Build();

// runner.Run();
public record struct DTO { 
 public string Country { get; set; }
 public string Description { get; set; }
 public string Designation { get; set; }
 public string Points { get; set; }
 public string Price { get; set; }
 public string Region_1 { get; set; }
 public string Region_2 { get; set; }
 public string Variety { get; set; }
 public string Winary { get; set; }
 //todo: ,country,description,designation,points,price,province,region_1,region_2,variety,winery"
}