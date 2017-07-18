namespace EternalArrowBackup.TargetBinaryStorage.InMemoryBinaryStorage
{
    internal struct BlockId
    {
        public readonly string BlobId;

        public readonly string BlockKey;

        public BlockId(string blobId, string blockKey)
        {
            this.BlobId = blobId;
            this.BlockKey = blockKey;
        }
    }
}
