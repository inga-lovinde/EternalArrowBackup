namespace EternalArrowBackup.Contracts.TargetStorage
{
    using System;
    using System.Threading;

    public interface ITargetStorageForRecovery
    {
        IObservable<ITargetDirectory> GetAllDirectories(CancellationToken ct);
    }
}
