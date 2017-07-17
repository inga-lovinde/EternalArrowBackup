namespace EternalArrowBackup.Hasher.SHA1
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;
    using Xunit;

    public static class HasherTests
    {
        private const int MegabytesPerSecondLowSpeed = 200;
        private const int MegabytesPerSecondHighSpeed = 1000;

        // Test vectors taken from http://csrc.nist.gov/groups/ST/toolkit/documents/Examples/SHA_All.pdf
        [Theory]
        [Trait("Category", "Simple")]
        [InlineData("DA39A3EE 5E6B4B0D 3255BFEF 95601890 AFD80709", "")]
        [InlineData("A9993E36 4706816A BA3E2571 7850C26C 9CD0D89D", "abc")]
        [InlineData("84983E44 1C3BD26E BAAE4AA1 F95129E5 E54670F1", "abcdbcdecdefdefgefghfghighijhijkijkljklmklmnlmnomnopnopq")]
        public static async Task TestHashes(string expectedHash, string inputMessage)
        {
            var expectedHashFormatted = expectedHash.Replace(" ", string.Empty).ToLowerInvariant();
            var hasher = new SHA1ContentHasher();
            var messageBytes = Encoding.ASCII.GetBytes(inputMessage);
            using (var stream = new MemoryStream(messageBytes))
            {
                var actualHash = await hasher.ComputeHash(stream);
                Assert.Equal(expectedHashFormatted, actualHash);
            }
        }

        // Single-threaded SHA1 performance on i5-6500 is around 500MB/s
        [Theory]
        [Trait("Category", "Integration")]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(1000)]
        [InlineData(1000000)]
        [InlineData(10000000)]
        [InlineData(100000000)]
        [InlineData(1000000000)]
        public static async Task TestPerformance(int length)
        {
            var messageBytes = new byte[length];
            for (var i = 0; i < length; i++)
            {
                messageBytes[i] = (byte)((int)(Math.E * length + Math.PI * i) % 256);
            }

            var expectedTimeLow = length / (MegabytesPerSecondHighSpeed * 1000);
            var expectedTimeHigh = 1 + length / (MegabytesPerSecondLowSpeed * 1000);

            var hasher = new SHA1ContentHasher();
            using (var stream = new MemoryStream(messageBytes))
            {
                await hasher.ComputeHash(stream);
                stream.Seek(0, SeekOrigin.Begin);
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                await hasher.ComputeHash(stream);
                stopwatch.Stop();
                Assert.InRange(stopwatch.ElapsedMilliseconds, expectedTimeLow, expectedTimeHigh);
            }
        }
    }
}
