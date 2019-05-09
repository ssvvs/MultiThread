using MultiThreadZip;
using MultiThreadZip.BlockActors;
using NUnit.Framework;
using System;
using System.IO;

namespace Tests
{
    public class DataBlockTests
    {
        private string _filepath;
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
        public void ReadWriteTest(int count)
        {
            BlockWriter bw = new BlockWriter(_filepath);

            Random r = new Random();
            byte[] testData = new byte[count];

            Datablock testblock = new Datablock()
            {
                Data = testData,
                Count = testData.Length,
                StartPosition = 0,
                Number = 1,
            };

            bw.Write(testblock, 0);

            Datablock testblock2 = new Datablock()
            {
                Data = new byte[count],                
            };
            BlockReader br = new BlockReader(_filepath);
            br.Read(testblock2, 0, testblock.Count);


            CollectionAssert.AreEqual(testblock.Data, testblock2.Data);
            Assert.AreEqual(testblock.Count, testblock.Count);
        }

        [TestCase(1)]
        [TestCase(0xFF)]
        [TestCase(0xFFFF)]
        [TestCase(0xFFFFFF)]             
        public void ZipTest(int count)
        {
            Random r = new Random();
            byte[] testData = new byte[count];
            r.NextBytes(testData);

            BlockArchiver ar = new BlockArchiver();
            Datablock testBlock = new Datablock()
            {
                Data = testData,
                Count = 9,
                StartPosition = 0,
                Number = 1,
            };
            var compBlock = ar.Compress(testBlock);

            BlockDearchiver dear = new BlockDearchiver();
            var decompr = dear.Decompress(compBlock);
            CollectionAssert.AreEqual(testBlock.Data, decompr.Data);

        }       
    }
}