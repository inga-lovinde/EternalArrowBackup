namespace EternalArrowBackup.TargetMetadataStorage.Contracts
{
    using System.Threading;
    using System.Threading.Tasks;
    using System.Threading.Tasks.Dataflow;

    public interface ITargetDirectory
    {
        string DirectoryName { get; }

        Task UploadFile(string filename, string originalHash, long originalSize);

        Task GetAllLatestFileVersions(ITargetBlock<ITargetFileVersion> actionBlock, CancellationToken ct);
    }
}
