namespace EternalArrowBackup.Contracts.TargetStorage
{
    using System;

    public interface ITargetStorageForRecovery
    {
        IObservable<ITargetDirectory> GetAllDirectories();
    }
}
