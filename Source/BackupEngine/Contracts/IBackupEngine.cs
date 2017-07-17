namespace EternalArrowBackup.BackupEngine.Contracts
{
    using System.Threading;
    using System.Threading.Tasks;

    public interface IBackupEngine
    {
        Task<IBackupReport> BackupAll(CancellationToken ct);
    }
}
