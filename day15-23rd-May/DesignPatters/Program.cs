using System;

class Program
{
    static void Main(string[] args)
    {
        string path = "sample.txt";
        var fileManager = FileManager.GetInstance(path);

        AbstractFileOperationFactory operationFactory = new ConcreteFileOperationFactory(fileManager);

        var writer = operationFactory.GetWriteOperation();
        var reader = operationFactory.GetReadOperation();

        writer.Execute();
        reader.Execute();

        fileManager.Close();
    }
}
