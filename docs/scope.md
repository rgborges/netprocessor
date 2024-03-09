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
 
Importer works in three steps:
- **Source scope definition**: Defining which are the files and pre processing needed to transform them in a common format, abd cleasing the data.
- **Excecution definition**: Define the processing exectution strategy.
- **Deployment definition**: Define the target infrastructure where the data will be deployed.

 Generates an importer builder object from a specified type. It can be:
 - CSV Importer
 - Json Importer

Configurations:
- Permits configure pre processor function when reading the file lines.
- Permits to work with directory, performing actions againt the files in a directory before import, such as:
  - Merging all files into one (same structure)


### Importers 
**Purpose**: Import a source file of a specified type into a target (file, database, api). Importers need to be reliable, and error resistent. Which means that the process can't stop on an Exception. For that, the scope need to be treated line by line.



#### CsvImporter

The CsvImporter will be able to define the file caratcteristics and how it will process the line to parse from string.

#### CSV Static Class

Permit parse any csv file into a `ParserResult` with data as a List of type T.

#### Parsers
Parsers intend to convert text and parse it into a List of objects of type `T`. Parsers don't need to verify errors along the way, the same way of importers. Actually only importers need to track errors line by line, because it needs to be a realible process.