// See https://aka.ms/new-console-template for more information
using System.Runtime.CompilerServices;
using NetProcessor.Data;
using NetProcessor.Data.Importer;
using NetProcessor.Data.Parsers;

// string path = @"/home/rborges/tmp/datasets/winemag-data_first150k.csv";

// var result = CSV.ReadAll<DTO>(path, delimiter: ',', smallCaseCompare: true);

// if (!result.Success)
// {

//       System.Console.WriteLine("Errors has occured:");
//       foreach (string s in result.Errors)
//       {
//             System.Console.WriteLine(s);
//       }

//       var data = result.GetResult()
//             .GetDataAsList<DTO>();

// }

// System.Console.WriteLine("It was successfull");


//Test Parser

string[] lines = new[] {
      "R99|Rafael|26",
      "R88|245|Whiscousin"
      };

var parserOption = new TextParserOptions()
{
      Strategy = ParserStrategy.Line,
      FileOptions = new FileImporterOptions()
      {
            FileHasHeaders = false,
            ColumnDelimiterChar = '|'
      }
};

var parser = new PoliformLineTextParser(parserOption);


var r99Rule = new LineRule<CsvFileTokens>(new CsvFileToken())
                        .OfType<R99>()
                        .SpecifyRule((context, tsearch) =>
                        {

                              var rules = context.TokenDefinition;

                              if (rules.ContainsKey(context.GetNextChar()))
                              {
                                    char value = context.LineContent[context.CurrentIndex + 1];
                                    if (rules[value] == CsvFileTokens.SeparateChar)
                                    {
                                          tsearch.Value = context.LineContent.Substring(context.LastIndex, context.CurrentIndex);
                                          tsearch.Token = CsvFileTokens.Content;
                                          tsearch.Index = context.CurrentIndex;
                                          context.LastIndex = context.CurrentIndex;
                                          return;
                                    }
                              }

                              if (context.TokenDefinition.ContainsKey(context.Character))
                              {
                                    var symbol = context.TokenDefinition[context.Character];

                                    switch (symbol)
                                    {
                                          case CsvFileTokens.SeparateChar:
                                                tsearch.Value = null;
                                                tsearch.Token = CsvFileTokens.SeparateChar;
                                                tsearch.Index = context.CurrentIndex;
                                                break;
                                    }
                              }

                        });

try
{
      var result = parser.ParserTextLines(lines);

      if (!result.Success)
      {
            System.Console.WriteLine(String.Join(' ', result.Errors.ToArray()));
      }
}
catch (Exception exp)
{
      System.Console.WriteLine(exp);
}




public record class R99
{
      public string Id { get; set; } = string.Empty;
      public string Name { get; set; } = string.Empty;
      public int Age { get; set; }
}
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
