namespace EternalArrowBackup.ContentTransformer.ClearText.Tests
{
    using System;
    using Xunit;

    public static class DataWithSignatureTests
    {
        [Fact]
        [Trait("Category", "Simple")]
        public static void TestDataWithSignatureConstructor()
        {
            Assert.ThrowsAny<ArgumentException>(() => new DataWithSignature(null, new byte[0]));
            Assert.ThrowsAny<ArgumentException>(() => new DataWithSignature(new byte[0], null));
            Assert.ThrowsAny<ArgumentException>(() => new DataWithSignature(new byte[0], new byte[65536]));
            Assert.ThrowsAny<ArgumentException>(() => new DataWithSignature(new byte[0], new byte[100000]));
            new DataWithSignature(new byte[0], new byte[0]);
            new DataWithSignature(new byte[100], new byte[100]);
            new DataWithSignature(new byte[1000], new byte[65535]);
            new DataWithSignature(new byte[100000], new byte[65535]);
        }
    }
}
