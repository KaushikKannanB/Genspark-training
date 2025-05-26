public class ConcreteFileOperationFactory : AbstractFileOperationFactory
{
    private readonly FileOperationFactory _factory;

    public ConcreteFileOperationFactory(FileManager manager)
    {
        _factory = new FileOperationFactory(manager);
    }

    public override IFileOperation GetReadOperation() => _factory.CreateReader();
    public override IFileOperation GetWriteOperation() => _factory.CreateWriter();
}
