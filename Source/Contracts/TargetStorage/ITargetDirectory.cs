namespace EternalArrowBackup.Contracts.TargetStorage
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public interface ITargetDirectory
    {
        /// <summary>
        /// Uploads new content to the target directory, and adds new entity with DateTime.UtcNow to the list of files
        /// </summary>
        /// <param name="filename">File name</param>
        /// <param name="content">Content</param>
        /// <param name="hash">File hash</param>
        /// <returns>Task</returns>
        Task UploadNewContent(string filename, byte[] content, string hash);

        /// <summary>
        /// Adds new entity with DateTime.UtcNow to the list of files.
        /// 
        /// Example: file "abc.txt" with content "qwe" (v1) was replaced with "rty" (v2), and then was replaced with "qwe" (v3) again.
        /// The client should <see cref="UploadNewContent(string, byte[], string)"/> for v1 and v2, and then call this method (passing v1 FileInfo to it) for v3.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        Task UpdateExistingContent(ITargetFile file);

        IObservable<ITargetFile> GetAllFiles(CancellationToken ct);
    }
}
