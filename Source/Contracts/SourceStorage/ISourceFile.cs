namespace EternalArrowBackup.Contracts.SourceStorage
{
    using System.Threading.Tasks;

    public interface ISourceFile
    {
        string Filename { get; }

        long Size { get; }

        Task<byte[]> ReadContents();
    }
}
