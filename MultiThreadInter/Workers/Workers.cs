namespace MultiThreadInter.Workers
{
    using System.IO;

    using BlockActors;

    using Providers;

    public interface IWorker
    {
        void Work();
    }

    public class UniversalWorker
    {
        private IDataBlockCountProvider _blockCountProvider;
        private IDataBlockProvider _blockProvider;
        public bool IsWorkStopped { get; set; }
        private IWorker _worker;

        public void DoWork()
        {
            while (IsWorkStopped)
            {
                _worker.Work();
            }
        }
    }

    public class ReadBlockWorker : IWorker
    {
        private IDataBlockProvider _dataBlockProvider;
        private IPositionProvider _positionProvider;
        private IBlockLengthProvider _blockLengthProvider;
        private FileInfo _fileInfo;

        public ReadBlockWorker(IDataBlockProvider blockProvider, FileInfo info, IPositionProvider positionProvider, IBlockLengthProvider blockLengthProvider)
        {
            _dataBlockProvider = blockProvider;
            _fileInfo = info;
            _positionProvider = positionProvider;
            _blockLengthProvider = blockLengthProvider;
        }

        public void Work()
        {
            int blockNumber = _dataBlockProvider.GetNextBlockNumber;
            Datablock block = _dataBlockProvider.CreateNewDataBlock();
            block.Number = blockNumber;
            BlockReader reader = new BlockReader(_fileInfo.FullName);
            long startPosition = _positionProvider.GetReadPosition(blockNumber);
            int count = _blockLengthProvider.GetBlockLength();
            reader.Read(block, startPosition, count);
            _dataBlockProvider.PushToReadedQueue(block);
        }
    }

    public class WriteBlockWorker : IWorker
    {
        private FileInfo _fileInfo;
        private IDataBlockProvider _dataBlockProvider;
        private IPositionProvider _positionProvider;

        public void Work()
        {
            Datablock block = _dataBlockProvider.PullFromZippedQueue();
            BlockWriter writer = new BlockWriter(_fileInfo.FullName);
            long position = _positionProvider.GetWritePosition(block.Number);
            writer.Write(block, position);
        }
    }

    public class ZipBlockWorker : IWorker
    {
        private IDataBlockProvider _dataBlockProvider;

        public void Work()
        {
            BlockArchiver archiver = new BlockArchiver();
            Datablock srcBlock = _dataBlockProvider.PullFromReadedQueue();
            Datablock trgDatablock = _dataBlockProvider.CreateNewDataBlock();
            trgDatablock.Number = srcBlock.Number;
            archiver.Compress(srcBlock, trgDatablock);
            _dataBlockProvider.PushToZippedQueue(trgDatablock);
        }
    }
}
