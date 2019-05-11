namespace MultiThreadZip.SyncObjects
{
    using System.Threading;

    public class PositionParameter
    {
        public long? _postionToRead;
        public long? _postionToWrite;

        public int BlockNumber { get; set; }

        public long? PostionToRead
        {
            get
            {
                return _postionToRead;
            }
            set
            {
                _postionToRead = value;
                ReaderSync.Set();
            }
        }

        public long? PostionToWrite
        {
            get
            {
                return _postionToWrite;
            }
            set
            {
                _postionToWrite = value;
                WriterSync.Set();
            }
        }

        public AutoResetEvent ReaderSync { get; }

        public AutoResetEvent WriterSync { get; }  

        public PositionParameter(int number)
        {
            BlockNumber = number;
            ReaderSync = new AutoResetEvent(false);
            WriterSync = new AutoResetEvent(false);

        }
    }
}
