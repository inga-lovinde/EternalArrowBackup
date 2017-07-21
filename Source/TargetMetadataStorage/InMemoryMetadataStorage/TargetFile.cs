namespace EternalArrowBackup.TargetMetadataStorage.InMemoryMetadataStorage
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Threading.Tasks.Dataflow;
    using EternalArrowBackup.TargetMetadataStorage.Contracts;

    internal class TargetFile : ITargetFile
    {
        public TargetFile(List<ITargetFileVersion> versions)
        {
            this.Versions = versions;
        }

        private List<ITargetFileVersion> Versions { get; }

        public Task GetAllVersions(ITargetBlock<ITargetFileVersion> actionBlock, CancellationToken ct)
        {
            return Task.Run(() =>
            {
                foreach (var version in this.Versions)
                {
                    if (ct.IsCancellationRequested)
                    {
                        break;
                    }

                    actionBlock.Post(version);
                }

                actionBlock.Complete();
            });
        }
    }
}
