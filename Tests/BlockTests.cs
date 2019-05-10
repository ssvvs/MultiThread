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
        [TestCase(0xFFFFFF)]
        public void ReadWriteTest(int count)
        {
            var bw = new BlockWriter(_filepath);

            var r = new Random();
            var testData = new byte[count];

            var testblock = new Datablock
            {
                Data = testData,
                Count = testData.Length,
                StartPosition = 0,
                Number = 1
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
        [TestCase(0xFFFFFF)]
        public void ZipTest(int count)
        {
            var r = new Random();
            var testData = new byte[count];
            r.NextBytes(testData);

            var ar = new BlockArchiver();
            var testBlock = new Datablock
            {
                Data = testData,
                Count = 9,
                StartPosition = 0,
                Number = 1
            };
            Datablock compBlock = ar.Compress(testBlock);

            var dear = new BlockDearchiver();
            Datablock decompr = dear.Decompress(compBlock);
            CollectionAssert.AreEqual(testBlock.Data, decompr.Data);
        }

        #endregion
    }
}
