namespace MultiThreadZip.SyncObjects
{
    using System.Collections.Generic;

    public class PositionProvider
    {
        private readonly object _locker;
        private const int MAX_ELEMENT_COUNT = 50;

        private readonly SortedDictionary<int, PositionParameter> _positions;

        public PositionProvider()
        {
            _locker = new object();
            _positions = new SortedDictionary<int, PositionParameter>
            {
                { 0, new PositionParameter(0) { PostionToRead = 0, PostionToWrite = 0 } },
            };
            //for (int i = 1; i < MAX_ELEMENT_COUNT; i++)
            //{
            //    _positions.Add(i, new PositionParameter(i));
            //}
        }

        public long? GetReadPosition(int blockNumber)
        {
            PositionParameter curParam = GetParameter(blockNumber);
            if (curParam.PostionToRead == null)
                curParam.ReaderSync.WaitOne();
            return curParam.PostionToRead;
        }

        public long? GetWritePosition(int blockNumber)
        {
            PositionParameter curParam = GetParameter(blockNumber);
            if (curParam.PostionToWrite == null)
                curParam.WriterSync.WaitOne();
            return curParam.PostionToWrite;
        }

        public void DoReport(int blockNumber, bool forRead, long count)
        {
            PositionParameter curItem = GetParameter(blockNumber);
            PositionParameter nextItem = GetParameter(blockNumber + 1);

            if (forRead)
                nextItem.PostionToRead = curItem.PostionToRead + count;
            else
            {
                nextItem.PostionToWrite = curItem.PostionToWrite + count;
                lock (_locker)
                {
                    _positions.Remove(blockNumber);
                }
            }
        }

        public void ReleaseAllReaders()
        {
            lock (_locker)
            {
                foreach (PositionParameter posParam in _positions.Values)
                {
                    posParam.ReaderSync.Set();
                }
            }
        }

        public void ReleaseAllWritters()
        {
            lock (_locker)
            {
                foreach (PositionParameter posParam in _positions.Values)
                {
                    posParam.WriterSync.Set();
                }
            }
        }

        private PositionParameter GetParameter(int blockNumber)
        {
            lock (_locker)
            {
                if (!_positions.ContainsKey(blockNumber))
                {
                    _positions.Add(blockNumber, new PositionParameter(blockNumber));
                }

                return _positions[blockNumber];
            }
        }
    }
}
