namespace EternalArrowBackup.Contracts.TargetStorage
{
    using System;
    using System.Threading.Tasks;

    public interface ITargetDirectory
    {
        Task UploadFile(string filename, byte[] content, string hash);

        Task<ITargetFile> GetFile(string filename, string hash);

        IObservable<ITargetFile> GetAllFiles();
    }
}
