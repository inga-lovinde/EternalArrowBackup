namespace EternalArrowBackup.Contracts.TargetMetadataStorage
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public interface ITargetDirectory
    {
        Task UploadFile(string filename, string originalHash, long originalSize);

        IObservable<ITargetFileVersion> GetAllLatestFileVersions(CancellationToken ct);
    }
}
