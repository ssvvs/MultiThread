namespace MultiThreadInter.Providers
{
    public interface IBlockLengthProvider
    {
        int GetBlockLength();
    }

    public interface IDataBlockProvider
    {
        int GetNextBlockNumber { get; }
        Datablock CreateNewDataBlock();
        void PushToReadedQueue(Datablock item);
        void PushToZippedQueue(Datablock item);
        Datablock PullFromReadedQueue();
        Datablock PullFromZippedQueue();
    }

    public interface IDataBlockCountProvider
    {
        int NeedReadBlockCount { get; }
        int NeedZipBlockCount { get; }
        int NeedWriteBlockCount { get; }
    }

    public interface IPositionProvider
    {
        long GetReadPosition(int blockNumber);
        long GetWritePosition(int blockNumber);
        void DoReport(int blockNumber, bool forRead, long count);
    }
}
