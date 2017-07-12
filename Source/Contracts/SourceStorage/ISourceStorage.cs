﻿namespace EternalArrowBackup.Contracts.SourceStorage
{
    using System;
    using System.Threading.Tasks;

    public interface ISourceStorage
    {
        Task<ISourceDirectory> GetDirectory(string normalizedRelativePath);

        IObservable<ISourceDirectory> GetAllDirectories();
    }
}
