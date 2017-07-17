namespace EternalArrowBackup.TargetMetadataStorage.Contracts
{
    using System.Threading.Tasks;

    public interface ITargetMetadataStorage
    {
        Task<ITargetDirectory> GetDirectory(string normalizedRelativeDirectoryPath);
    }
}
