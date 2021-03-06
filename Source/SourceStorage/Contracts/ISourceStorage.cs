﻿namespace EternalArrowBackup.SourceStorage.Contracts
{
    using System.Threading;
    using System.Threading.Tasks;
    using System.Threading.Tasks.Dataflow;

    public interface ISourceStorage
    {
        Task<ISourceDirectory> GetDirectory(string normalizedRelativePath);

        Task GetAllDirectories(ITargetBlock<ISourceDirectory> actionBlock, CancellationToken ct);
    }
}
