namespace EternalArrowBackup.SourceStorage.Contracts
{
    using System.Threading;
    using System.Threading.Tasks;
    using System.Threading.Tasks.Dataflow;

    public interface ISourceDirectory
    {
        string NormalizedRelativePath { get; }

        Task<ISourceFile> GetFile(string filename);

        Task GetAllFiles(ActionBlock<ISourceFile> actionBlock, CancellationToken ct);
    }
}
