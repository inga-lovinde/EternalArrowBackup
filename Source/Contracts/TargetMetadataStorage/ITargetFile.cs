namespace EternalArrowBackup.Contracts.TargetMetadataStorage
{
    using System;
    using System.Threading;

    public interface ITargetFile
    {
        IObservable<ITargetFileVersion> GetAllVersions(CancellationToken ct);
    }
}
