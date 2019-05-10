namespace MultiThreadZip.BlockActors
{
    using System.IO;

    public class BlockWriter
    {
        #region Fields

        private readonly string _filepath;

        #endregion

        #region Constructors

        public BlockWriter(string filepath)
        {
            _filepath = filepath;
        }

        #endregion

        #region Methods

        public void Write(Datablock block, long startPosition)
        {
            using (FileStream fstream = File.OpenWrite(_filepath))
            {
                using (var wr = new BinaryWriter(fstream))
                {
                    wr.BaseStream.Position = startPosition;
                    wr.Write(block.Data, 0, block.Count);
                }
            }
        }

        #endregion
    }
}
