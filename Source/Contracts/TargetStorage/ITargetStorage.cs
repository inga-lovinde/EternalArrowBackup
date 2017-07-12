namespace EternalArrowBackup.Contracts.TargetStorage
{
    using System.Threading.Tasks;

    public interface ITargetStorage
    {
        Task<ITargetDirectory> GetDirectory(string normalizedRelativePath);
    }
}
