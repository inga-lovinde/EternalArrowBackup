namespace EternalArrowBackup.SourceStorage.InMemorySourceStorage
{
    using System.Collections.Immutable;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Threading.Tasks.Dataflow;
    using EternalArrowBackup.SourceStorage.Contracts;

    internal class SourceDirectory : ISourceDirectory
    {
        public SourceDirectory(string path, ImmutableDictionary<string, byte[]> storageData)
        {
            this.NormalizedRelativePath = path;
            this.StorageData = storageData;
        }

        public string NormalizedRelativePath { get; }

        private ImmutableDictionary<string, byte[]> StorageData { get; }

        public Task GetAllFiles(ITargetBlock<ISourceFile> actionBlock, CancellationToken ct)
        {
            return Task.Run(() =>
            {
                foreach (var kvp in this.StorageData)
                {
                    if (ct.IsCancellationRequested)
                    {
                        break;
                    }

                    actionBlock.Post(new SourceFile(kvp.Key, kvp.Value));
                }

                actionBlock.Complete();
            });
        }

        public Task<ISourceFile> GetFile(string filename)
        {
            return Task.Run(() => (ISourceFile)new SourceFile(filename, this.StorageData[filename]));
        }
    }
}
