namespace MultiThreadZip.BlockActors
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.IO.Compression;
    using System.Text;

    public class BlockDearchiver
    {
        public BlockDearchiver()
        {

        }

        public Datablock Decompress(Datablock src)
        {
            Datablock trg = new Datablock();
            long oLength = 0;
            for (int i = 1; i <= 4; i++)
            {
                oLength = oLength << 8 | src.Data[src.Data.Length - i];
            }

            using (MemoryStream targetStream = new MemoryStream(src.Data))
            {
                using (GZipStream compressionStream = new GZipStream(targetStream, CompressionMode.Decompress))
                {
                    trg.Data = new byte[oLength];
                    var count = compressionStream.Read(trg.Data, 0, trg.Data.Length);
                }
            }
            return trg;
        }
    }
}
