namespace EternalArrowBackup.Contracts.TargetMetadataStorage
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public interface ITargetMetadataStorageForRecovery
    {
        IObservable<ITargetDirectory> GetAllDirectories(CancellationToken ct);

        Task<ITargetFile> GetFile(string normalizedRelativeDirectoryPath, string filename);
    }
}
