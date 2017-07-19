﻿namespace EternalArrowBackup.TargetMetadataStorage.Contracts
{
    using System.Threading;
    using System.Threading.Tasks;
    using System.Threading.Tasks.Dataflow;

    public interface ITargetFile
    {
        Task GetAllVersions(ActionBlock<ITargetFileVersion> actionBlock, CancellationToken ct);
    }
}
