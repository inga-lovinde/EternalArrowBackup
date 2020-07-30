namespace EternalArrowBackup.TargetMetadataStorage.InMemoryMetadataStorage
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Threading.Tasks.Dataflow;
    using EternalArrowBackup.TargetMetadataStorage.Contracts;

    public class MetadataStorage : ITargetMetadataStorageForRecovery
    {
        private Dictionary<string, Dictionary<string, List<ITargetFileVersion>>> Data { get; } = new Dictionary<string, Dictionary<string, List<ITargetFileVersion>>>();

        private Dictionary<string, List<ITargetFileVersion>> BlobsInfo { get; } = new Dictionary<string, List<ITargetFileVersion>>();

        public Task GetAllDirectories(ITargetBlock<ITargetDirectory> actionBlock, CancellationToken ct)
        {
            return Task.Run(() =>
            {
                foreach (var kvp in this.Data)
                {
                    if (ct.IsCancellationRequested)
                    {
                        break;
                    }

                    actionBlock.Post(new TargetDirectory(this, kvp.Key, kvp.Value));
                }

                actionBlock.Complete();
            });
        }

        public Task GetBlobUsages(string hash, ITargetBlock<ITargetFileVersion> actionBlock, CancellationToken ct)
        {
            return Task.Run(() =>
            {
                if (!this.BlobsInfo.ContainsKey(hash))
                {
                    actionBlock.Complete();
                    return;
                }

                foreach (var fileVersion in this.BlobsInfo[hash])
                {
                    if (ct.IsCancellationRequested)
                    {
                        break;
                    }

                    actionBlock.Post(fileVersion);
                }

                actionBlock.Complete();
            });
        }

        public Task<ITargetDirectory> GetDirectory(string normalizedRelativeDirectoryPath)
        {
            return Task.Run(() =>
            {
                if (!this.Data.ContainsKey(normalizedRelativeDirectoryPath))
                {
                    this.Data[normalizedRelativeDirectoryPath] = new Dictionary<string, List<ITargetFileVersion>>();
                }

                return (ITargetDirectory)new TargetDirectory(this, normalizedRelativeDirectoryPath, this.Data[normalizedRelativeDirectoryPath]);
            });
        }

        public Task<ITargetFile> GetFile(string normalizedRelativeDirectoryPath, string filename)
        {
            return Task.Run(() =>
            {
                if (!this.Data.ContainsKey(normalizedRelativeDirectoryPath))
                {
                    this.Data[normalizedRelativeDirectoryPath] = new Dictionary<string, List<ITargetFileVersion>>();
                }

                if (!this.Data[normalizedRelativeDirectoryPath].ContainsKey(filename))
                {
                    this.Data[normalizedRelativeDirectoryPath][filename] = new List<ITargetFileVersion>();
                }

                return (ITargetFile)new TargetFile(this.Data[normalizedRelativeDirectoryPath][filename]);
            });
        }

        public void AddBlob(ITargetFileVersion fileVersion)
        {
            if (!this.BlobsInfo.ContainsKey(fileVersion.Hash))
            {
                this.BlobsInfo[fileVersion.Hash] = new List<ITargetFileVersion>();
            }

            this.BlobsInfo[fileVersion.Hash].Add(fileVersion);
        }
    }
}
