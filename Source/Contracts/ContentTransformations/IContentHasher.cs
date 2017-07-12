namespace EternalArrowBackup.Contracts.ContentTransformations
{
    using System.Threading.Tasks;

    public interface IContentHasher
    {
        Task<string> ComputeHash(byte[] content);
    }
}
