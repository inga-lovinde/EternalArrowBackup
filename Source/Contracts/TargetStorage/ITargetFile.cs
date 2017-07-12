namespace EternalArrowBackup.Contracts.TargetStorage
{
    using System;
    using System.Threading.Tasks;

    public interface ITargetFile
    {
        string Filename { get; }

        string Hash { get; }

        DateTime UploadDate { get; }

        long Size { get; }

        Task<byte[]> RetrieveContents();
    }
}
