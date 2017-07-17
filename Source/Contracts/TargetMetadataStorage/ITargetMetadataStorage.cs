namespace EternalArrowBackup.Contracts.TargetMetadataStorage
{
    using System.Threading.Tasks;

    public interface ITargetMetadataStorage
    {
        Task<ITargetDirectory> GetDirectory(string normalizedRelativeDirectoryPath);
    }
}
