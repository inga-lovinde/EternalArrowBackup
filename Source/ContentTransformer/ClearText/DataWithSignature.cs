namespace EternalArrowBackup.ContentTransformer.ClearText
{
    using System;

    public class DataWithSignature
    {
        public DataWithSignature(byte[] data, byte[] signature) {
            if (data == null) {
                throw new ArgumentNullException(nameof(data));
            }

            if (signature == null) {
                throw new ArgumentNullException(nameof(signature));
            }

            if (signature.Length > 0xffff) {
                throw new ArgumentOutOfRangeException(nameof(signature), "Signature should be shorter than 64k");
            }

            this.Data = data;
            this.Signature = signature;
        }

        public byte[] Data { get; }

        public byte[] Signature { get; }

        public static DataWithSignature FromByteArray(byte[] byteArray) {
            var signatureLengthBytes = new byte[2];
            BufferHelpers.Extract(byteArray, byteArray.Length - 2, signatureLengthBytes);
            var signatureLength = (signatureLengthBytes[0] * 0x100) + signatureLengthBytes[1];

            var data = new byte[byteArray.Length - signatureLength - 2];
            var signature = new byte[signatureLength];

            BufferHelpers.Extract(byteArray, 0, data);
            BufferHelpers.Extract(byteArray, data.Length, signature);

            return new DataWithSignature(data, signature);
        }

        public byte[] AsByteArray() {
            var signatureLengthBytes = new[] {
                (byte)(this.Signature.Length / 0x100),
                (byte)(this.Signature.Length % 0x100),
            };

            var result = new byte[this.Data.Length + this.Signature.Length + 2];
            BufferHelpers.Add(this.Data, result, 0);
            BufferHelpers.Add(this.Signature, result, this.Data.Length);
            BufferHelpers.Add(signatureLengthBytes, result, this.Data.Length + this.Signature.Length);

            return result;
        }
    }
}
