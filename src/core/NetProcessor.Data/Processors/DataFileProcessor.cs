using System;
using System.Runtime.CompilerServices;

namespace NetProcessor.Data.Processors;

public abstract class DataFileProcessor
{
    // private FileInfo _file;
    // private DataFileProcessorOptions _options = new DataFileProcessorOptions();
    // public DataFileProcessor(Action<DataFileProcessorOptions> configure)
    // {
    //     configure(_options);
    // }

    // public void Process()
    // {
    //     // Processing logic here

    // }

    // abstract Dictionary<object> ProcessLine(string line)
    // {

    // }

    // async Task<IEnumerable<DataProcessingOperation>> ProcessBatchAsync()
    // {
    //     foreach (var chunk in File.ReadLines(_options.FilePath).Chunk(_options.ChunkSize))
    //     {
    //         // Process each chunk
    //     }
    // }


    // public void Validate()
    // {
    //     // Validation logic here
    // }
}

public record class DataFileProcessorOptions
{
    public string FilePath { get; set; }
    public bool BatchProcessing { get; set; }
    public int ChunkSize { get; set; }
    public bool UseBinary  { get; set; }
};

public class DataProcessorFactory
{
    // public static DataFileProcessor Create(string type)
    // {
    //     switch (type.ToLower())
    //     {
    //         case "csv":
    //             return new DataFileProcessor("path/to/datafile");
    //         default:
    //             throw new ArgumentException("Unknown processor type");
    //     }
    // }
}
