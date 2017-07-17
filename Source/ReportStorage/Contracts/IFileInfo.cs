namespace EternalArrowBackup.ReportStorage.Contracts
{
    public interface IFileInfo
    {
        string DirectoryPath { get; }

        string Filename { get; }

        string Hash { get; }

        long Size { get; }
    }
}
