namespace EternalArrowBackup.ReportStorage.InMemoryReportStorage
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Threading.Tasks.Dataflow;
    using EternalArrowBackup.ReportStorage.Contracts;

    internal class ReportInfo : IReportInfo
    {
        public ReportInfo(string reportId, DateTime dateTime)
        {
            
        }

        public string ReportId { get; }

        public DateTime DateTime { get; }

        public bool IsCompleted { get; private set; } = false;

        public bool IsEmpty { get; private set; }

        public bool HasErrors { get; private set; }

        private List<string> Errors { get; } = new List<string>();

        private List<IFileInfo> Files { get; } = new List<IFileInfo>();

        public Task GetAllErrors(ActionBlock<string> actionBlock, CancellationToken ct)
        {
            return Task.Run(() =>
            {
                foreach (var error in this.Errors)
                {
                    if (ct.IsCancellationRequested)
                    {
                        break;
                    }

                    actionBlock.Post(error);
                }

                actionBlock.Complete();
            });
        }

        public Task GetAllFiles(ActionBlock<IFileInfo> actionBlock, CancellationToken ct)
        {
            return Task.Run(() =>
            {
                foreach (var fileInfo in this.Files)
                {
                    if (ct.IsCancellationRequested)
                    {
                        break;
                    }

                    actionBlock.Post(fileInfo);
                }

                actionBlock.Complete();
            });
        }

        public void AddFile(IFileInfo fileInfo)
        {
            this.Files.Add(fileInfo);
        }

        public void AddError(string errorMessage)
        {
            this.Errors.Add(errorMessage);
        }

        public void Complete(bool isEmpty, bool hasErrors)
        {
            this.IsCompleted = true;
            this.IsEmpty = isEmpty;
            this.HasErrors = hasErrors;
        }
    }
}
