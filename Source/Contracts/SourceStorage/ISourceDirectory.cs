namespace EternalArrowBackup.Contracts.SourceStorage
{
    using System;
    using System.Threading.Tasks;

    public interface ISourceDirectory
    {
        string NormalizedRelativePath { get; }

        Task<ISourceFile> GetFile(string filename);

        IObservable<ISourceFile> GetAllFiles();
    }
}
