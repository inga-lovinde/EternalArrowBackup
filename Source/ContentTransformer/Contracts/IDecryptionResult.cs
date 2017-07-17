namespace EternalArrowBackup.ContentTransformer.Contracts
{
    public interface IDecryptionResult
    {
        bool IsSuccessful { get; }

        byte[] Data { get; }
    }
}
