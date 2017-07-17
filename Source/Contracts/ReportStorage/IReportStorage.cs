namespace EternalArrowBackup.Contracts.ReportStorage
{
    using System;
    using System.Threading.Tasks;

    public interface IReportStorage
    {
        Task InitReport(string reportId);

        Task CompleteReport(string reportId, bool isEmpty, bool hasErrors);

        Task AddError(string reportId, string errorMessage);

        Task AddFileInfo(string reportId, string directoryPath, string fileName, string hash, long size);

        IObservable<IReportInfo> GetAllReports(bool includeEmpty);
    }
}
