using System;

public class FileReader : IFileOperation
{
    private readonly StreamReader _reader;

    public FileReader(StreamReader reader)
    {
        _reader = reader;
        _reader.BaseStream.Seek(0, SeekOrigin.Begin); // Rewind to start
    }

    public void Execute()
    {
        Console.WriteLine("Reading File Content:");
        string line;
        while ((line = _reader.ReadLine()) != null)
        {
            Console.WriteLine(line);
        }
    }
}
