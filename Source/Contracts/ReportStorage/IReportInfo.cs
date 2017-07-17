namespace EternalArrowBackup.Contracts.ReportStorage
{
    using System;

    public interface IReportInfo
    {
        string ReportId { get; }

        DateTime DateTime { get; }

        bool IsCompleted { get; }

        bool IsEmpty { get; }

        bool HasErrors { get; }

        IObservable<string> GetAllErrors();

        IObservable<IFileInfo> GetAllFiles();
    }
}
