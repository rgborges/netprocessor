using System;
using System.Security.Cryptography.X509Certificates;

namespace NetProcessor.Data.Processors;

public record class CSVSplitProcessorOptions
{
    public string Path { get; set; } = String.Empty;
    public string PageBreakderPatter { get; set; } = String.Empty;
}

public class CSVSplitProcessor
{
    public CSVSplitProcessorOptions Options { get; }

    public CSVSplitProcessor(Action<CSVSplitProcessorOptions> configOptions)
    {
        Options = new CSVSplitProcessorOptions();

        configOptions?.Invoke(Options);
    }

    // Implement the processing logic here
    public void Process()
    {
        // Logic to read CSV from Options.Path and process it
        // This is a placeholder for the actual implementation
        if (string.IsNullOrEmpty(Options.Path))
        {
            throw new ArgumentException("Path must be provided", nameof(Options.Path));
        }
        if (string.IsNullOrEmpty(Options.PageBreakderPatter))
        {
            throw new ArgumentException("PageBreakderPatter must be provided", nameof(Options.PageBreakderPatter));
        }

        // Example processing logic
        var files = Directory.GetFiles(Options.Path, "*.csv");

        if (files.Count() == 0)
        {
            return;
        }

        foreach (var file in files)
        {
            // Process each CSV file
            Console.WriteLine($"Processing file: {file}");
            // Add your CSV processing logic here
        }


        Console.WriteLine($"Processing CSV file at: {Options.Path}");
    }
}
