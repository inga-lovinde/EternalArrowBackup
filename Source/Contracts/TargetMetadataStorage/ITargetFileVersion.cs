namespace EternalArrowBackup.Contracts.TargetMetadataStorage
{
    using System;

    public interface ITargetFileVersion
    {
        string Filename { get; }

        string Hash { get; }

        DateTime UploadDate { get; }

        long Size { get; }
    }
}
