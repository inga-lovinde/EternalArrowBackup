namespace EternalArrowBackup.Contracts.ContentTransformations
{
    using System.Threading.Tasks;

    public interface IContentEncryptor
    {
        Task<byte[]> Encrypt(byte[] originalData);

        Task<IDecryptionResult> Decrypt(byte[] encryptedData);
    }
}
