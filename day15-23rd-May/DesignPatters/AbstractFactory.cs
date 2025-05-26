public abstract class AbstractFileOperationFactory
{
    public abstract IFileOperation GetReadOperation();
    public abstract IFileOperation GetWriteOperation();
}
