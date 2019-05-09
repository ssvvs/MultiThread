namespace MultiThreadZip.BlockActors
{
    using System.IO;

    public class BlockReader
    {
        private string _filepath;

        public BlockReader(string filepath)
        {
            _filepath = filepath;
        }

        public bool Read(Datablock block, long startPosition, int count)
        {
            using (var fstream = File.OpenRead(_filepath))
            {
                using (BinaryReader wr = new BinaryReader(fstream))
                {
                    wr.BaseStream.Position = startPosition;
                    int reallyReaded = wr.Read(block.Data, 0, count);

                }
            }
            return true;
        }
    }
}
