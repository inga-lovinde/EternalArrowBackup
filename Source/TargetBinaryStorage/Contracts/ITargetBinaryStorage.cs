namespace EternalArrowBackup.TargetBinaryStorage.Contracts
{
    using System.Threading.Tasks;

    public interface ITargetBinaryStorage
    {
        Task WriteBlock(byte[] block, string blobId, int partNumber, string blockKey);

        Task WriteBlob(string blobId, long originalSize, string[] blockKeys);

        Task<IBlobInfo> GetBlobIfExists(string blobId);
    }
}
