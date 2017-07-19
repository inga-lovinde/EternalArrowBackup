namespace EternalArrowBackup.TargetMetadataStorage.InMemoryMetadataStorage
{
    using System;
    using EternalArrowBackup.TargetMetadataStorage.Contracts;

    internal class TargetFileVersion : ITargetFileVersion
    {
        public TargetFileVersion(string filename, string hash, DateTime uploadDate, long size)
        {
            this.Filename = filename;
            this.Hash = hash;
            this.UploadDate = uploadDate;
            this.Size = size;
        }

        public string Filename { get; }

        public string Hash { get; }

        public DateTime UploadDate { get; }

        public long Size { get; }
    }
}
