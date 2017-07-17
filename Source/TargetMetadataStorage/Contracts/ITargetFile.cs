namespace EternalArrowBackup.TargetMetadataStorage.Contracts
{
    using System;
    using System.Threading;

    public interface ITargetFile
    {
        IObservable<ITargetFileVersion> GetAllVersions(CancellationToken ct);
    }
}
