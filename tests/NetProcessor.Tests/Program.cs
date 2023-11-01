// See https://aka.ms/new-console-template for more information
using NetProcessor.Data;
using NetProcessor.Data.Importer;


string path = @"home/rborges/tmp/datasets/winemag-data_first150k.csv";
var fileInfo = new FileInfo(path);

// var builder = new ImporterBuilder();

// builder.FromSourceFile(file => {
//       file.SourcePath = "~/tmp/test.csv";
// }, fileConfiguration => {
//       fileConfiguration.FileType = CsvFile;
//       fileConfiguration.Delimeter = ';';
// }).Read<ExampleDto>(MethodReadAll).ToJson();

var importer = new CsvImporter<DTO>(fileInfo, false);
importer.ReadAll();

// var runner = builder.Build();

// runner.Run();
public record struct DTO { 
 string id;
 string content;

}