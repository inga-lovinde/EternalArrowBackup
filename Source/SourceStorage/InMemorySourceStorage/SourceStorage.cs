namespace EternalArrowBackup.SourceStorage.InMemorySourceStorage
{
    using System.Collections.Immutable;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Threading.Tasks.Dataflow;
    using EternalArrowBackup.SourceStorage.Contracts;

    public class SourceStorage : ISourceStorage
    {
        public SourceStorage(ImmutableDictionary<string, ImmutableDictionary<string, byte[]>> storageData)
        {
            this.StorageData = storageData;
        }

        private ImmutableDictionary<string, ImmutableDictionary<string, byte[]>> StorageData { get; }

        public Task GetAllDirectories(ITargetBlock<ISourceDirectory> actionBlock, CancellationToken ct)
        {
            return Task.Run(() =>
            {
                foreach (var kvp in this.StorageData)
                {
                    if (ct.IsCancellationRequested)
                    {
                        break;
                    }

                    actionBlock.Post(new SourceDirectory(kvp.Key, kvp.Value));
                }

                actionBlock.Complete();
            });
        }

        public Task<ISourceDirectory> GetDirectory(string normalizedRelativePath)
        {
            return Task.Run(() => (ISourceDirectory)new SourceDirectory(normalizedRelativePath, this.StorageData[normalizedRelativePath]));
        }
    }
}
