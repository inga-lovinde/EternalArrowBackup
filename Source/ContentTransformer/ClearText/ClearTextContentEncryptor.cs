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

        public async Task<IDecryptionResult> GetOriginalData(byte[] transformedData)
        {
            var dataWithSignature = DataWithSignature.FromByteArray(transformedData);

            var expectedHash = Encoding.UTF8.GetString(dataWithSignature.Signature);
            using (var stream = new MemoryStream(dataWithSignature.Data))
            {
                var actualHash = await this.Hasher.ComputeHash(stream);

                if (expectedHash != actualHash)
                {
                    return new FailedDecryptionResult();
                }
            }

            return new SuccessfulDecryptionResult(dataWithSignature.Data);
        }

        public async Task<byte[]> TransformData(byte[] originalData)
        {
            string hash;
            using (var stream = new MemoryStream(originalData))
            {
                hash = await this.Hasher.ComputeHash(stream);
            }

            var hashBytes = Encoding.UTF8.GetBytes(hash);
            var dataWithSignature = new DataWithSignature(originalData, hashBytes);

            return dataWithSignature.AsByteArray();
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
