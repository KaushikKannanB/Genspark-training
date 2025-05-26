using System;
using System.IO;

public sealed class FileManager
{
    private static FileManager _instance = null;
    private static readonly object _lock = new object();
    private StreamWriter writer;
    private StreamReader reader;
    private string filePath;

    private FileManager(string path)
    {
        filePath = path;
        FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
        writer = new StreamWriter(fs);
        reader = new StreamReader(fs);
    }

    public static FileManager GetInstance(string path)
    {
        lock (_lock)
        {
            if (_instance == null)
                _instance = new FileManager(path);
            return _instance;
        }
    }

    public StreamWriter GetWriter() => writer;
    public StreamReader GetReader() => reader;

    public void Close()
    {
        writer?.Flush();
        writer?.Close();
        reader?.Close();
    }
}
