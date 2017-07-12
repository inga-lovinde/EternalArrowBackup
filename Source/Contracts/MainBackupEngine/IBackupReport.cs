using System;

namespace EternalArrowBackup.Contracts.BackupEngine
{
    public class IBackupReport
    {
        public long FilesUploaded { get; }

        public long BytesUploaded { get; }

        public long FilesUpdated { get; }

        public long FilesProcessed { get; }

        public Exception[] Errors { get; }
    }
}
