# General scope Definition

.NET Processor is a library to process massive data from a specified source to a given target. The project intends to use in some data operations involved in development a like sync databases, importing data into a specified database, importing data to your code in many ways.

## Import file types scope
- CSV
- Json

## Export target
* Postgree SQL Server
* Text


# Technologies and Dependencies

- .NET
",country,description,designation,points,price,province,region_1,region_2,variety,winery"


## ClassDiagram

### ImporterBuilder
 
 Generates an importer builder object from a specified type. It can be:
 - CSV Importer
 - Json Importer





#### CsvImporter

The CsvImporter will be able to define the file caratcteristics and how it will process the line to parse from string.