namespace EternalArrowBackup.TargetBinaryStorage.Contracts
{
    using System;

    public interface IBlobInfo
    {
        DateTime UploadDate { get; }

        long OriginalSize { get; }

        string[] BlockKeys { get; }
    }
}
