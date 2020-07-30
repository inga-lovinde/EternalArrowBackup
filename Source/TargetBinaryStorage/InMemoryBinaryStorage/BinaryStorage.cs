namespace EternalArrowBackup.TargetBinaryStorage.InMemoryBinaryStorage
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using EternalArrowBackup.TargetBinaryStorage.Contracts;

    public class BinaryStorage : ITargetBinaryStorage
    {
        private Dictionary<string, IBlobInfo> Blobs { get; } = new Dictionary<string, IBlobInfo>();

        private Dictionary<BlockId, byte[]> Blocks { get; } = new Dictionary<BlockId, byte[]>();

        public Task<IBlobInfo> GetBlobIfExists(string blobId)
        {
            if (!this.Blobs.ContainsKey(blobId))
            {
                return Task.FromResult(default(IBlobInfo));
            }

            return Task.FromResult(this.Blobs[blobId]);
        }

        public Task WriteBlob(string blobId, long originalSize, string[] blockKeys)
        {
            return Task.Run(() =>
            {
                this.Blobs[blobId] = new BlobInfo(DateTime.UtcNow, originalSize, blockKeys);
            });
        }

        public Task WriteBlock(byte[] block, string blobId, int partNumber, string blockKey)
        {
            return Task.Run(() =>
            {
                this.Blocks[new BlockId(blobId, blockKey)] = block;
            });
        }

        public Task<byte[]> RetrieveBlock(string blobId, string blockKey)
        {
            return Task.Run(() => this.Blocks[new BlockId(blobId, blockKey)]);
        }
    }
}
