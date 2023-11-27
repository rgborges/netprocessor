// See https://aka.ms/new-console-template for more information
using System.Runtime.CompilerServices;
using NetProcessor.Data;
using NetProcessor.Data.Importer;

string path = @"/home/rborges/tmp/datasets/winemag-data_first150k.csv";

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



public class DTO
{
      public int Id { get; set; }
      public string Country { get; set; } = string.Empty;
      public string Description { get; set; } = string.Empty;
      public string Designation { get; set; } = string.Empty;
      public int Points { get; set; }
      public double Price { get; set; }
      public string Province { get; set; } = string.Empty;
      public string Region_1 { get; set; } = string.Empty;
      public string Region_2 { get; set; } = string.Empty;
      public string Variety { get; set; } = string.Empty;
      public string Winery { get; set; } = string.Empty;
      //todo: Id,country,description,designation,points,price,province,region_1,region_2,variety,winery"
}
