namespace EternalArrowBackup.TargetBinaryStorage.InMemoryBinaryStorage
{
    internal class BlockInfo
    {
        public BlockInfo(string blobId, int partNumber, string blockKey, byte[] blockData)
        {
            this.BlobId = blobId;
            this.PartNumber = partNumber;
            this.BlockKey = blockKey;
            this.BlockData = blockData;
        }

        public string BlobId { get; }

        public int PartNumber { get; }

        public string BlockKey { get; }

        public byte[] BlockData { get; }
    }
}
