namespace EternalArrowBackup.Contracts.ReportStorage
{
    public interface IFileInfo
    {
        string DirectoryPath { get; }

        string Filename { get; }

        string Hash { get; }

        long Size { get; }
    }
}
