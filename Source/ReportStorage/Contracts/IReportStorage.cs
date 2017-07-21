namespace EternalArrowBackup.ReportStorage.Contracts
{
    using System.Threading;
    using System.Threading.Tasks;
    using System.Threading.Tasks.Dataflow;

    public interface IReportStorage
    {
        Task InitReport(string reportId);

        Task CompleteReport(string reportId, bool isEmpty, bool hasErrors);

        Task AddError(string reportId, string errorMessage);

        Task AddFileInfo(string reportId, string directoryPath, string fileName, string hash, long size);

        Task GetAllReports(ITargetBlock<IReportInfo> actionBlock, bool includeEmpty, CancellationToken ct);
    }
}
