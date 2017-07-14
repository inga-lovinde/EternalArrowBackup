namespace EternalArrowBackup.Hasher.SHA1
{
    using System;
    using System.Diagnostics;
    using System.Text;
    using System.Threading.Tasks;
    using Xunit;

    public static class HasherTests
    {
        private const int MegabytesPerSecond = 500;

        // Test vectors taken from http://csrc.nist.gov/groups/ST/toolkit/documents/Examples/SHA_All.pdf
        [Theory]
        [InlineData("A9993E36 4706816A BA3E2571 7850C26C 9CD0D89D", "abc")]
        [InlineData("84983E44 1C3BD26E BAAE4AA1 F95129E5 E54670F1", "abcdbcdecdefdefgefghfghighijhijkijkljklmklmnlmnomnopnopq")]
        public static async Task TestHashes(string expectedHash, string inputMessage)
        {
            var expectedHashFormatted = expectedHash.Replace(" ", string.Empty).ToLowerInvariant();
            var hasher = new SHA1ContentHasher();
            var messageBytes = Encoding.ASCII.GetBytes(inputMessage);
            var actualHash = await hasher.ComputeHash(messageBytes);
            Assert.Equal(expectedHashFormatted, actualHash);
        }

        // Single-threaded SHA1 performance on i5-6500 is around 500MB/s
        [Theory]
        [InlineData(1, 0, 1)]
        [InlineData(1000, 0, 1)]
        [InlineData(1000000, 0.5 * 1000 / MegabytesPerSecond, 2 * 1000 / MegabytesPerSecond)]
        [InlineData(10000000, 0.5 * 10000 / MegabytesPerSecond, 2 * 10000 / MegabytesPerSecond)]
        [InlineData(100000000, 0.5 * 100000 / MegabytesPerSecond, 2 * 100000 / MegabytesPerSecond)]
        [InlineData(1000000000, 0.5 * 1000000 / MegabytesPerSecond, 2 * 1000000 / MegabytesPerSecond)]
        public static async Task TestPerformance(int length, int minMilliseconds, int maxMilliseconds)
        {
            var messageBytes = new byte[length];
            for (var i = 0; i < length; i++)
            {
                messageBytes[i] = (byte)((int)(Math.E * length + Math.PI * i) % 256);
            }

            var hasher = new SHA1ContentHasher();
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            await hasher.ComputeHash(messageBytes);
            stopwatch.Stop();

            Assert.InRange(stopwatch.ElapsedMilliseconds, minMilliseconds, maxMilliseconds);
        }
    }
}
