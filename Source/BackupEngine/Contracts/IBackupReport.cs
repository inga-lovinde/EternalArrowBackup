namespace EternalArrowBackup.BackupEngine.Contracts
{
    using System;

    public interface IBackupReport
    {
        long FilesUploaded { get; }

        long BytesUploaded { get; }

        long FilesUpdated { get; }

        long FilesProcessed { get; }

        Exception[] Errors { get; }
    }
}
