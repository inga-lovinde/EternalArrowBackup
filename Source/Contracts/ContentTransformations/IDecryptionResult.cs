namespace EternalArrowBackup.Contracts.ContentTransformations
{
    public interface IDecryptionResult
    {
        bool IsSuccessful { get; }

        byte[] Data { get; }
    }
}
