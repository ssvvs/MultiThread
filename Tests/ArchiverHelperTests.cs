namespace Tests
{
    using System;
    using System.IO;

    using MultiThreadZip;

    using NUnit.Framework;

    [TestFixture]
    public class ArchiverHelperTests
    {
        #region Methods

        [TestCase(10)]
        [TestCase(0xFF)]
        [TestCase(0xFFFF)]
        [TestCase(1000000)]
        [TestCase(0xFFFFF6)]
        public void TestHeaderWithLegthAndLenthTest(int length)
        {
            var r = new Random();

            var srcArray = new byte[length];
            srcArray[0] = 31;
            srcArray[1] = 139;
            srcArray[2] = 8;
            srcArray[3] = 0;
            srcArray[4] = 0;
            srcArray[5] = 0;
            srcArray[6] = 0;
            srcArray[7] = 0;
            srcArray[8] = 4;
            srcArray[9] = 0;
            for (var i = 10; i < length; i++)
                srcArray[i] = (byte)r.Next();

            var dataBlock = new Datablock
            {
                Count = length,
                Data = new byte[length + ArchiverHelper.EXTRA_FIELD_LENGTH]
            };
            using (var ms = new MemoryStream(srcArray))
            {
                ArchiverHelper.GetHeaderWithLength(ms, dataBlock);
            }

            Assert.AreEqual(srcArray.Length + ArchiverHelper.EXTRA_FIELD_LENGTH, dataBlock.Count);
            Assert.AreNotEqual(srcArray[3], dataBlock.Data[3]);

            var resultCount = 0;
            using (var ms = new MemoryStream(dataBlock.Data, 0, dataBlock.Count))
            {
                resultCount = ArchiverHelper.GetBlockLegth(ms);
            }

            Assert.AreEqual(dataBlock.Count, resultCount);
        }

        #endregion
    }
}
