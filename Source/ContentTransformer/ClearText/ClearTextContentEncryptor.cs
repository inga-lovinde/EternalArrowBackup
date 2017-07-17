namespace EternalArrowBackup.ContentTransformer.ClearText
{
    using System;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;
    using EternalArrowBackup.ContentTransformer.Contracts;
    using EternalArrowBackup.Hasher.Contracts;

    public class ClearTextContentEncryptor : IContentTransformer
    {
        public ClearTextContentEncryptor(IContentHasher hasher)
        {
            this.Hasher = hasher;
        }

        private IContentHasher Hasher { get; }

        public async Task<IDecryptionResult> GetOriginalData(byte[] encryptedData)
        {
            var hashLength = encryptedData[encryptedData.Length - 1];

            var originalData = new byte[encryptedData.Length - hashLength - 1];
            var hashBytes = new byte[hashLength];

            Buffer.BlockCopy(encryptedData, 0, originalData, 0, originalData.Length);
            Buffer.BlockCopy(encryptedData, originalData.Length, hashBytes, 0, hashBytes.Length);

            var expectedHash = Encoding.UTF8.GetString(hashBytes);
            using (var stream = new MemoryStream(originalData))
            {
                var actualHash = await this.Hasher.ComputeHash(stream);

                if (expectedHash != actualHash)
                {
                    return new FailedDecryptionResult();
                }
            }

            return new SuccessfulDecryptionResult(originalData);
        }

        public async Task<byte[]> TransformData(byte[] originalData)
        {
            string hash;
            using (var stream = new MemoryStream(originalData))
            {
                hash = await this.Hasher.ComputeHash(stream);
            }

            var hashBytes = Encoding.UTF8.GetBytes(hash);
            if (hashBytes.Length >= 256)
            {
                throw new Exception("Hash should be shorter than 256 bytes");
            }

            var result = new byte[originalData.Length + hashBytes.Length + 1];
            Buffer.BlockCopy(originalData, 0, result, 0, originalData.Length);
            Buffer.BlockCopy(hashBytes, 0, result, originalData.Length, hashBytes.Length);
            result[result.Length - 1] = (byte)hashBytes.Length;
            return result;
        }

        private class FailedDecryptionResult : IDecryptionResult
        {
            public bool IsSuccessful => false;

            public byte[] Data => throw new NotImplementedException();
        }

        private class SuccessfulDecryptionResult : IDecryptionResult
        {
            public SuccessfulDecryptionResult(byte[] data) => this.Data = data;

            public bool IsSuccessful => true;

            public byte[] Data { get; }
        }
    }
}
