namespace EternalArrowBackup.Contracts.ContentTransformations
{
    using System.IO;
    using System.Threading.Tasks;

    public interface IContentHasher
    {
        Task<string> ComputeHash(Stream content);
    }
}
