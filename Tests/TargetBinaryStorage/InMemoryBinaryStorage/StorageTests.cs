namespace EternalArrowBackup.TargetBinaryStorage.InMemoryBinaryStorage.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Threading.Tasks.Dataflow;
    using Xunit;

    public static class StorageTests
    {
        [Fact]
        [Trait("Category", "Simple")]
        public static async Task CheckStorage()
        {
            var storageData = CreateStorageData();
            var storage = new BinaryStorage();

            foreach (var kvp in storageData)
            {
                var blobId = kvp.Key;
                Assert.Null(await storage.GetBlobIfExists(blobId));
                var blocks = kvp.Value.Item2;
                for (var i = 0; i < blocks.Length; i++)
                {
                    await storage.WriteBlock(blocks[i].Item2, blobId, i, blocks[i].Item1);
                }

                await storage.WriteBlob(blobId, kvp.Value.Item1, blocks.Select(tuple => tuple.Item1).ToArray());
            }

            foreach (var kvp in storageData)
            {
                var blobId = kvp.Key;
                var blobInfo = await storage.GetBlobIfExists(blobId);
                Assert.NotNull(blobInfo);
                Assert.Equal(kvp.Value.Item1, blobInfo.OriginalSize);
                Assert.InRange(blobInfo.UploadDate, DateTime.UtcNow.AddMinutes(-2), DateTime.UtcNow);
                Assert.Equal(kvp.Value.Item2.Select(tuple => tuple.Item1).ToArray(), blobInfo.BlockKeys);

                for (var i = 0; i < blobInfo.BlockKeys.Length; i++)
                {
                    var blockKey = blobInfo.BlockKeys[i];
                    Assert.Equal(kvp.Value.Item2[i].Item1, blockKey);

                    var blockContent = await storage.RetrieveBlock(blobId, blockKey);
                    Assert.Equal(kvp.Value.Item2[i].Item2, blockContent);
                }
            }
        }

        private static Dictionary<string, Tuple<long, Tuple<string, byte[]>[]>> CreateStorageData()
        {
            var random = new Random();
            var result = new Dictionary<string, Tuple<long, Tuple<string, byte[]>[]>>();
            for (var i = 0; i < 2000; i++)
            {
                var blobName = Guid.NewGuid().ToString();
                var blocksCount = random.Next(0, 20);
                var blocks = new Tuple<string, byte[]>[blocksCount];
                for (var j = 0; j < blocksCount; j++)
                {
                    var blockKey = Guid.NewGuid().ToString();
                    var blockContent = new byte[random.Next(0, 20)];
                    for (var k = 0; k < blockContent.Length; k++)
                    {
                        blockContent[k] = (byte)random.Next(0, 255);
                    }

                    blocks[j] = Tuple.Create(blockKey, blockContent);
                }

                result[blobName] = Tuple.Create(((long)random.Next()) * random.Next(), blocks);
            }

            return result;
        }
    }
}
