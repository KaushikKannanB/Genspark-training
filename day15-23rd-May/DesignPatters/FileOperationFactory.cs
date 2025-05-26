public class FileOperationFactory
{
    private readonly FileManager _manager;

    public FileOperationFactory(FileManager manager)
    {
        _manager = manager;
    }

    public IFileOperation CreateReader() => new FileReader(_manager.GetReader());
    public IFileOperation CreateWriter() => new FileWriter(_manager.GetWriter());
}
