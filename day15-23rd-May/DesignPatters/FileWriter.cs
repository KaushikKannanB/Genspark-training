using System;

public class FileWriter : IFileOperation
{
    private readonly StreamWriter _writer;

    public FileWriter(StreamWriter writer)
    {
        _writer = writer;
    }

    public void Execute()
    {
        Console.WriteLine("Writing to File:");
        _writer.WriteLine($"Written at: {DateTime.Now}");
    }
}
