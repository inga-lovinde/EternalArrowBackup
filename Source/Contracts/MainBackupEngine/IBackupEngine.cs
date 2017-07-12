namespace EternalArrowBackup.Contracts.BackupEngine
{
    using System.Threading;
    using System.Threading.Tasks;

    public interface IBackupEngine
    {
        Task<IBackupReport> BackupAll(CancellationToken ct);
    }
}
