namespace EternalArrowBackup.ContentTransformer.Contracts
{
    using System.Threading.Tasks;

    public interface IContentTransformer
    {
        Task<byte[]> TransformData(byte[] originalData);

        Task<IDecryptionResult> GetOriginalData(byte[] transformedData);
    }
}
