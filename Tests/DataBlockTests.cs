namespace Tests
{
    using System;
    using System.IO;

    using MultiThreadZip;
    using MultiThreadZip.BlockActors;

    using NUnit.Framework;

    public class DataBlockTests
    {
        #region Fields

        private string _filepath;

        #endregion

        #region Methods

        [TestCase(1)]
        [TestCase(0xFF)]
        [TestCase(0xFFFF)]
        [TestCase(1000000)]
        [TestCase(0xFFFFFF)]
        public void ReadWriteTest(int count)
        {
            var bw = new BlockWriter(_filepath);

            var r = new Random();
            var testData = new byte[count];

            var testblock = new Datablock
            {
                Data = testData,
                Count = testData.Length
            };

            bw.Write(testblock, 0);

            var testblock2 = new Datablock
            {
                Data = new byte[count]
            };
            var br = new BlockReader(_filepath);
            br.Read(testblock2, 0, testblock.Count);

            CollectionAssert.AreEqual(testblock.Data, testblock2.Data);
            Assert.AreEqual(testblock.Count, testblock.Count);
        }

        [SetUp]
        public void Setup()
        {
            _filepath = Path.GetTempFileName();
        }

        [TearDown]
        public void Teardown()
        {
            if (File.Exists(_filepath))
                File.Delete(_filepath);
        }

        [TestCase(1)]
        [TestCase(0xFF)]
        [TestCase(0xFFFF)]
        [TestCase(1000000)]
        [TestCase(0xFFFFFF)]
        public void ZipTest(int count)
        {
            var r = new Random();
            var testData = new byte[count + 30 * count];
            r.NextBytes(testData);

            var ar = new BlockArchiver();
            var testBlock = new Datablock
            {
                Data = testData,
                Count = count
            };
            var compBlock = new Datablock
            {
                Data = new byte[count + 30 * count],
                Count = 0
            };

            ar.Compress(testBlock, compBlock);

            var dear = new BlockDearchiver();
            var decompr = new Datablock
            {
                Data = new byte[count + 30 * count],
                Count = 0
            };
            dear.Decompress(compBlock, decompr);

            byte[] actualData = decompr.Data;
            Array.Resize(ref actualData, decompr.Count);

            byte[] awaitData = testData;
            Array.Resize(ref awaitData, count);

            CollectionAssert.AreEqual(awaitData, actualData);
        }

        #endregion
    }
}
