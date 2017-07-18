namespace EternalArrowBackup.SourceStorage.InMemorySourceStorage
{
    using System.Threading.Tasks;
    using EternalArrowBackup.SourceStorage.Contracts;

    internal class SourceFile : ISourceFile
    {
        public SourceFile(string filename, byte[] contents)
        {
            this.Filename = filename;
            this.Size = contents.Length;
            this.Contents = contents;
        }

        public string Filename { get; }

        public long Size { get; }

        private byte[] Contents { get; }

        public Task<byte[]> ReadContents()
        {
            return Task.Run(() => this.Contents);
        }
    }
}
