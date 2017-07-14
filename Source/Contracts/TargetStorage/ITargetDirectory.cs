namespace EternalArrowBackup.Contracts.TargetStorage
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public interface ITargetDirectory
    {
        Task UploadFile(string filename, byte[] data, string originalHash, long originalSize);

        IObservable<ITargetFile> GetAllFiles(CancellationToken ct);
    }
}
