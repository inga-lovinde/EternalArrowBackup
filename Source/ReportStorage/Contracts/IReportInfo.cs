namespace EternalArrowBackup.ReportStorage.Contracts
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Threading.Tasks.Dataflow;

    public interface IReportInfo
    {
        string ReportId { get; }

        DateTime DateTime { get; }

        bool IsCompleted { get; }

        bool IsEmpty { get; }

        bool HasErrors { get; }

        Task GetAllErrors(ActionBlock<string> actionBlock, CancellationToken ct);

        Task GetAllFiles(ActionBlock<IFileInfo> actionBlock, CancellationToken ct);
    }
}
