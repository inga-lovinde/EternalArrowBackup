namespace EternalArrowBackup.ReportStorage.InMemoryReportStorage
{
    using EternalArrowBackup.ReportStorage.Contracts;

    class FileInfo : IFileInfo
    {
        public FileInfo(string directoryPath, string filename, string hash, long size)
        {
            this.DirectoryPath = directoryPath;
            this.Filename = filename;
            this.Hash = hash;
            this.Size = size;
        }

        public string DirectoryPath { get; }

        public string Filename { get; }

        public string Hash { get; }

        public long Size { get; }
    }
}
