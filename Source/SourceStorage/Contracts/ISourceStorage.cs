namespace EternalArrowBackup.SourceStorage.Contracts
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public interface ISourceStorage
    {
        Task<ISourceDirectory> GetDirectory(string normalizedRelativePath);

        IObservable<ISourceDirectory> GetAllDirectories(CancellationToken ct);
    }
}
