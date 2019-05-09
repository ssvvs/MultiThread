namespace MultiThreadZip.BlockActors
{
    using System.IO;
    using System.IO.Compression;

    public class BlockArchiver
    {
        public BlockArchiver()
        {

        }        

        public Datablock Compress(Datablock src)
        {
            Datablock trg = new Datablock();

            using (MemoryStream targetStream = new MemoryStream())
            {
                using (GZipStream compressionStream = new GZipStream(targetStream, CompressionMode.Compress,true))
                {
                    compressionStream.Write(src.Data, 0, src.Data.Length);                   
                }
                trg.Data = ArchiverHelper.GetHeaderWithLength(targetStream);
            }
            return trg;
        }       
    }
}
