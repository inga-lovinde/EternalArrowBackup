namespace EternalArrowBackup.TargetMetadataStorage.InMemoryMetadataStorage
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Threading.Tasks.Dataflow;
    using EternalArrowBackup.TargetMetadataStorage.Contracts;

    internal class TargetDirectory : ITargetDirectory
    {
        public TargetDirectory(MetadataStorage metadataStorage, string directoryName, Dictionary<string, List<ITargetFileVersion>> files)
        {
            this.MetadataStorage = metadataStorage;
            this.DirectoryName = directoryName;
            this.Files = files;
        }

        public string DirectoryName { get; }

        private MetadataStorage MetadataStorage { get; }

        private Dictionary<string, List<ITargetFileVersion>> Files { get; }

        public Task GetAllLatestFileVersions(ActionBlock<ITargetFileVersion> actionBlock, CancellationToken ct)
        {
            return Task.Run(() =>
            {
                foreach (var file in this.Files.Values)
                {
                    if (ct.IsCancellationRequested)
                    {
                        break;
                    }

                    if (file.Any())
                    {
                        actionBlock.Post(file.Last());
                    }
                }

                actionBlock.Complete();
            });
        }

        public Task UploadFile(string filename, string originalHash, long originalSize)
        {
            return Task.Run(() =>
            {
                if (!this.Files.ContainsKey(filename))
                {
                    this.Files[filename] = new List<ITargetFileVersion>();
                }

                var fileVersion = new TargetFileVersion(filename, originalHash, DateTime.UtcNow, originalSize);
                this.Files[filename].Add(fileVersion);
                MetadataStorage.AddBlob(fileVersion);
            });
        }
    }
}
