using System.IO;

namespace MultiThreadZip.BlockActors
{
    public class BlockWriter
    {
        private string _filepath;

        public BlockWriter(string filepath)
        {
            _filepath = filepath;
        }

        public void Write(Datablock block, long startPosition)
        {
            using (var fstream = File.OpenWrite(_filepath))
            {
                using (BinaryWriter wr = new BinaryWriter(fstream))
                {
                    wr.BaseStream.Position = startPosition;
                    wr.Write(block.Data, 0, block.Count);
                }
            }
        }
    }


}
