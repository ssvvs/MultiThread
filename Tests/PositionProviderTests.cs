namespace Tests
{
    using System.Threading;

    using MultiThreadZip.SyncObjects;

    using NUnit.Framework;
    

    [TestFixture]
    public class PositionProviderTests
    {
        private PositionProvider _provider;

        [SetUp]
        public void Setup()
        {
            _provider = new PositionProvider();
        }

        [Test]
        public void InitialDataTest()
        {
            long curReadOffset = 0;
            long curWtiteOffset = 0;
            int curBlockNumber = 0;
            
            var resultReadPosition = _provider.GetReadPosition(curBlockNumber);
            var resultWritePosition = _provider.GetReadPosition(curBlockNumber);

            Assert.AreEqual(resultReadPosition, curReadOffset);
            Assert.AreEqual(resultWritePosition, curWtiteOffset);

            curReadOffset = 25000;
            curWtiteOffset = 31000;
            _provider.DoReport(curBlockNumber, true, curReadOffset );
            _provider.DoReport(curBlockNumber, false, curWtiteOffset);
            curBlockNumber++;

            resultReadPosition = _provider.GetReadPosition(curBlockNumber);
            resultWritePosition = _provider.GetWritePosition(curBlockNumber);

            Assert.AreEqual(resultReadPosition, curReadOffset);
            Assert.AreEqual(resultWritePosition, curWtiteOffset);
        }

        [TestCase(0, 100, 100)]
        [TestCase(0, 1000, 1000)]
        [TestCase(0, 10000, 10000)]
        [TestCase(0, 100000, 10000)]
        public void SingleThreadTest(int blockNumber, int readCount, int writeCount)
        {
            var initReadPosition = _provider.GetReadPosition(blockNumber);
            var initWritePosition = _provider.GetWritePosition(blockNumber);

            _provider.DoReport(blockNumber, true, readCount);
            _provider.DoReport(blockNumber, false, writeCount);

            var endReadPosition = _provider.GetReadPosition(blockNumber+1);
            var endWritePosition = _provider.GetWritePosition(blockNumber+1);

            Assert.AreEqual(initReadPosition + readCount, endReadPosition);
            Assert.AreEqual(initWritePosition + writeCount, endWritePosition);
        }

        private void DoTestSingle(int blockNumber, int readCount, int writeCount)
        {
            var initReadPosition = _provider.GetReadPosition(blockNumber);
            var initWritePosition = _provider.GetWritePosition(blockNumber);

            _provider.DoReport(blockNumber, true, readCount);

            _provider.DoReport(blockNumber, false, writeCount);

            var endReadPosition = _provider.GetReadPosition(blockNumber + 1);
            var endWritePosition = _provider.GetWritePosition(blockNumber + 1);
        }


        [Test]
        public void MultiThreadTest()
        {
            int threadsCount = 4;

            int[] readedCounts = new int [threadsCount];
            int[] writedCounts = new int[threadsCount];
            Thread[] threads = new Thread[threadsCount];

            for (int i = 0; i < threadsCount; i++)
            {
                int curNum = i;
                int readed = i + 100;
                int writed = i + 1000;
                readedCounts[i] = readed;
                writedCounts[i] = writed;
                threads[curNum] = new Thread(() =>
                {
                    DoTestSingle(curNum, readed, writed);
                });
            }

            foreach (Thread thread in threads)
            {
                thread.Start();
            }

            foreach (Thread thread in threads)
            {
                thread.Join();
            }
        }
    }
}
