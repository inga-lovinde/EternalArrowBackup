namespace EternalArrowBackup.Hasher.Sha256
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;
    using Xunit;

    public static class HasherTests
    {
        // Single-threaded SHA256 performance on modern CPUs is around 200-500MB/s
        private const int MegabytesPerSecondLowSpeed = 20;
        private const int MegabytesPerSecondHighSpeed = 5000;

        // Test vectors taken from http://csrc.nist.gov/groups/ST/toolkit/documents/Examples/SHA_All.pdf
        [Theory]
        [Trait("Category", "Simple")]
        [InlineData("E3B0C442 98FC1C14 9AFBF4C8 996FB924 27AE41E4 649B934C A495991B 7852B855", "")]
        [InlineData("BA7816BF 8F01CFEA 414140DE 5DAE2223 B00361A3 96177A9C B410FF61 F20015AD", "abc")]
        [InlineData("248D6A61 D20638B8 E5C02693 0C3E6039 A33CE459 64FF2167 F6ECEDD4 19DB06C1", "abcdbcdecdefdefgefghfghighijhijkijkljklmklmnlmnomnopnopq")]
        public static async Task TestHashes(string expectedHash, string inputMessage)
        {
            var expectedHashFormatted = expectedHash.Replace(" ", string.Empty).ToLowerInvariant();
            var hasher = new Sha256ContentHasher();
            var messageBytes = Encoding.ASCII.GetBytes(inputMessage);
            using (var stream = new MemoryStream(messageBytes))
            {
                var actualHash = await hasher.ComputeHash(stream);
                Assert.Equal(expectedHashFormatted, actualHash);
            }
        }

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
                messageBytes[i] = (byte)((int)(Math.E * i * i + Math.PI * i) % 256);
            }

            var expectedTimeLow = length / (MegabytesPerSecondHighSpeed * 1000);
            var expectedTimeHigh = 1 + length / (MegabytesPerSecondLowSpeed * 1000);

            var hasher = new Sha256ContentHasher();
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
