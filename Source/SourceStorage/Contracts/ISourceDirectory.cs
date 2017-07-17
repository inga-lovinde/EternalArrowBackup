namespace EternalArrowBackup.SourceStorage.Contracts
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public interface ISourceDirectory
    {
        string NormalizedRelativePath { get; }

        Task<ISourceFile> GetFile(string filename);

        IObservable<ISourceFile> GetAllFiles(CancellationToken ct);
    }
}
