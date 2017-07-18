namespace EternalArrowBackup.SourceStorage.InMemorySourceStorage.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Threading.Tasks.Dataflow;
    using EternalArrowBackup.SourceStorage.Contracts;
    using Xunit;

    public static class StorageTests
    {
        [Fact]
        public static async Task CheckStorage()
        {
            var storageData = CreateStorageData();

            var immutableStorageData = storageData
                .Select(kvp => new KeyValuePair<string, ImmutableDictionary<string, byte[]>>(kvp.Key, kvp.Value.ToImmutableDictionary()))
                .ToImmutableDictionary();
            var storage = new SourceStorage(immutableStorageData);

            var testDirectoryName = storageData.Keys.Skip(storageData.Count / 2).First();
            var testDirectoryInfo = await storage.GetDirectory(testDirectoryName);
            Assert.Equal(testDirectoryName, testDirectoryInfo.NormalizedRelativePath);
            await CheckDirectory(storageData[testDirectoryName].ToDictionary(kvp => kvp.Key, kvp => kvp.Value), testDirectoryInfo);

            var syncObject = new object();
            var actionBlock = new ActionBlock<ISourceDirectory>(async sourceDirectory =>
            {
                Dictionary<string, byte[]> originalDirectoryData;

                lock (syncObject)
                {
                    Assert.True(storageData.ContainsKey(sourceDirectory.NormalizedRelativePath), "Directory does not exist in original data");
                    originalDirectoryData = storageData[sourceDirectory.NormalizedRelativePath];
                    storageData.Remove(sourceDirectory.NormalizedRelativePath);
                }

                await CheckDirectory(originalDirectoryData, sourceDirectory);
            });

            await storage.GetAllDirectories(actionBlock, CancellationToken.None);
            await actionBlock.Completion;

            Assert.False(storageData.Any(), "Not all directories were enumerated");
        }

        private static async Task CheckDirectory(Dictionary<string, byte[]> originalDirectory, ISourceDirectory contractsDirectory)
        {
            if (originalDirectory.Count > 0)
            {
                var testFileName = originalDirectory.Keys.Skip(originalDirectory.Count / 2).First();
                var testFileInfo = await contractsDirectory.GetFile(testFileName);
                Assert.Equal(testFileName, testFileInfo.Filename);
                Assert.Equal(originalDirectory[testFileName].Length, testFileInfo.Size);
                Assert.Equal(originalDirectory[testFileName], await testFileInfo.ReadContents());
            }

            var syncObject = new object();
            var actionBlock = new ActionBlock<ISourceFile>(async sourceFile =>
            {
                byte[] originalFileContents;
                lock (syncObject)
                {
                    Assert.True(originalDirectory.ContainsKey(sourceFile.Filename), "File does not exist in original data");
                    originalFileContents = originalDirectory[sourceFile.Filename];
                    originalDirectory.Remove(sourceFile.Filename);
                }

                Assert.Equal(originalFileContents.Length, sourceFile.Size);
                Assert.Equal(originalFileContents, await sourceFile.ReadContents());
            });

            await contractsDirectory.GetAllFiles(actionBlock, CancellationToken.None);
            await actionBlock.Completion;

            Assert.False(originalDirectory.Any(), "Not all files were enumerated");
        }

        private static Dictionary<string, Dictionary<string, byte[]>> CreateStorageData()
        {
            var random = new Random();
            var result = new Dictionary<string, Dictionary<string, byte[]>>();
            for (var i = 0; i < 2000; i++)
            {
                var directoryName = Guid.NewGuid().ToString();
                var directoryContent = new Dictionary<string, byte[]>();
                var directorySize = random.Next(0, 500);
                for (var j = 0; j < directorySize; j++)
                {
                    var fileName = Guid.NewGuid().ToString();
                    var fileContent = new byte[random.Next(0, 20)];
                    for (var k = 0; k < fileContent.Length; k++)
                    {
                        fileContent[k] = (byte)random.Next(0, 255);
                    }

                    directoryContent[fileName] = fileContent;
                }

                result[directoryName] = directoryContent;
            }

            return result;
        }
    }
}
