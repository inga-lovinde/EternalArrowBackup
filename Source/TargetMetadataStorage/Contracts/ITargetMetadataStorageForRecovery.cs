namespace EternalArrowBackup.TargetMetadataStorage.Contracts
{
    using System.Threading;
    using System.Threading.Tasks;
    using System.Threading.Tasks.Dataflow;

    public interface ITargetMetadataStorageForRecovery : ITargetMetadataStorage
    {
        Task GetAllDirectories(ActionBlock<ITargetDirectory> actionBlock, CancellationToken ct);

        Task<ITargetFile> GetFile(string normalizedRelativeDirectoryPath, string filename);

        Task GetBlobUsages(string hash, ActionBlock<ITargetFileVersion> actionBlock, CancellationToken ct);
    }
}
