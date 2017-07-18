namespace EternalArrowBackup.TargetBinaryStorage.InMemoryBinaryStorage
{
    using System;
    using EternalArrowBackup.TargetBinaryStorage.Contracts;

    internal class BlobInfo : IBlobInfo
    {
        public BlobInfo(DateTime uploadDate, long originalSize, string[] blockKeys)
        {
            this.UploadDate = uploadDate;
            this.OriginalSize = originalSize;
            this.BlockKeys = blockKeys;
        }

        public DateTime UploadDate { get; }

        public long OriginalSize { get; }

        public string[] BlockKeys { get; }
    }
}
