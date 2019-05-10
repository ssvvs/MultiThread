namespace MultiThreadZip.Workers
{
    using BlockActors;

    public class ZipWorker
    {
        #region Fields

        private readonly string _srcFilePath;
        private readonly string _trgFilePath;

        #endregion

        #region Constructors

        public ZipWorker(string srcFilePath, string trgFilePath)
        {
            _srcFilePath = srcFilePath;
            _trgFilePath = trgFilePath;
        }

        #endregion

        #region Methods

        public void Work(int blockNumber)
        {
            var blockReader = new BlockReader(_srcFilePath);
            var blockArchiver = new BlockArchiver();
            var blockWriter = new BlockWriter(_trgFilePath);

            var readBlock = new Datablock
            {
                Data = new byte[1100000],
                Count = 1000000
            };
            var writeBlock = new Datablock
            {
                Data = new byte[1100000]
            };

            blockReader.Read(readBlock, 0, 1000000);
            blockArchiver.Compress(readBlock, writeBlock);
            blockWriter.Write(writeBlock, 0);
        }

        #endregion
    }
}
