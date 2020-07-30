namespace EternalArrowBackup.ContentTransformer.ClearText
{
    using System;

    internal static class BufferHelpers
    {
        public static void Add(byte[] source, byte[] target, int targetOffset) {
            Buffer.BlockCopy(source, 0, target, targetOffset, source.Length);
        }

        public static void Extract(byte[] source, int sourceOffset, byte[] target) {
            Buffer.BlockCopy(source, sourceOffset, target, 0, target.Length);
        }
    }
}
