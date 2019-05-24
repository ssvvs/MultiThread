namespace InterTests
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    using NUnit.Framework;
    using Moq;

    using MultiThreadInter;
    using MultiThreadInter.Providers;
    using MultiThreadInter.Workers;

    public class WorkersTests
    {
        private IDataBlockProvider _dataBlockProvider;
        private IPositionProvider _positionProvider;
        private IBlockLengthProvider _blockLengthProvider;

        private int _curBlockNumber;
        private int _blockLength;
        private long _curPosition;

        private string _trgFilePath;

        private string _srcFilePath;

        private List<Datablock> _pushedBlocks;

        [SetUp]
        public void Setup()
        {
            _pushedBlocks = new List<Datablock>();
            Mock<IDataBlockProvider> dataBlockProviderMock = new Mock<IDataBlockProvider>();
            dataBlockProviderMock.Setup(a => a.GetNextBlockNumber).Returns(() => _curBlockNumber);
            dataBlockProviderMock.Setup(a => a.CreateNewDataBlock()).Returns(() => new Datablock(){ Data = new byte[500]});
            dataBlockProviderMock.Setup(a => a.PushToReadedQueue(It.IsAny<Datablock>())).Callback<Datablock>((item) => _pushedBlocks.Add(item));
            _dataBlockProvider = dataBlockProviderMock.Object;

            Mock<IPositionProvider> positionProviderMock = new Mock<IPositionProvider>();
            positionProviderMock.Setup(a => a.GetReadPosition(It.IsAny<int>())).Returns(() => _curPosition);
            _positionProvider = positionProviderMock.Object;

            Mock<IBlockLengthProvider> blockLengthProviderMock = new Mock<IBlockLengthProvider>();
            blockLengthProviderMock.Setup(a => a.GetBlockLength()).Returns(() => _blockLength);
            _blockLengthProvider = blockLengthProviderMock.Object;

            _srcFilePath = Path.GetTempFileName();
            _trgFilePath = Path.GetTempFileName();
            WriteTestData(100000);

        }

        [Test]
        public void Test1()
        {
            _curPosition = 0;
            _curBlockNumber = 0;
            _blockLength = 100;
            ReadBlockWorker readWorker = new ReadBlockWorker(_dataBlockProvider, new FileInfo(_srcFilePath), _positionProvider, _blockLengthProvider);
            readWorker.Work();

            _curPosition = _blockLength;
            _curBlockNumber = 1;
            _blockLength = 150;
            readWorker.Work();
        }

        [TearDown]
        public void Teardown()
        {
            if (File.Exists(_srcFilePath))
                File.Delete(_srcFilePath);
            if (File.Exists(_trgFilePath))
                File.Delete(_trgFilePath);
        }

        private void WriteTestData(long count)
        {
            const long CHUNK_SIZE = 1000000;

            using (var wr = new BinaryWriter(File.OpenWrite(_srcFilePath)))
            {
                var random = new Random();
                var buffer = new byte[CHUNK_SIZE];
                long remainWrite = count;
                while (remainWrite > 0)
                {
                    random.NextBytes(buffer);
                    int stepSize = remainWrite > CHUNK_SIZE ? buffer.Length : (int)remainWrite;
                    wr.Write(buffer, 0, stepSize);
                    remainWrite -= stepSize;
                }
            }
        }
    }
}