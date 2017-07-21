namespace EternalArrowBackup.ReportStorage.InMemoryReportStorage
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Threading.Tasks.Dataflow;
    using EternalArrowBackup.ReportStorage.Contracts;

    public class ReportStorage : IReportStorage
    {
        private Dictionary<string, ReportInfo> Reports { get; } = new Dictionary<string, ReportInfo>();

        public Task AddError(string reportId, string errorMessage)
        {
            return Task.Run(() => this.Reports[reportId].AddError(errorMessage));
        }

        public Task AddFileInfo(string reportId, string directoryPath, string fileName, string hash, long size)
        {
            return Task.Run(() => this.Reports[reportId].AddFile(new FileInfo(directoryPath, fileName, hash, size)));
        }

        public Task CompleteReport(string reportId, bool isEmpty, bool hasErrors)
        {
            return Task.Run(() => this.Reports[reportId].Complete(isEmpty, hasErrors));
        }

        public Task GetAllReports(ITargetBlock<IReportInfo> actionBlock, bool includeEmpty, CancellationToken ct)
        {
            return Task.Run(() =>
            {
                foreach (var report in this.Reports.Values)
                {
                    if (ct.IsCancellationRequested)
                    {
                        break;
                    }

                    if (!includeEmpty && report.IsCompleted && !report.IsEmpty)
                    {
                        continue;
                    }

                    actionBlock.Post(report);
                }

                actionBlock.Complete();
            });
        }

        public Task InitReport(string reportId)
        {
            return Task.Run(() => this.Reports[reportId] = new ReportInfo(reportId, DateTime.UtcNow));
        }
    }
}
