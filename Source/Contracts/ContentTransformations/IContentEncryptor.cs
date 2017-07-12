namespace EternalArrowBackup.Contracts.Encryption
{
    using System.Threading.Tasks;

    public interface IContentEncryptor
    {
        Task<byte[]> Encrypt(byte[] originalData);

        Task<byte[]> Decrypt(byte[] encryptedData);
    }
}
