namespace MultiThreadZip.BlockActors
{
    using System.IO;
    using System.IO.Compression;

    public class BlockDearchiver
    {
        #region Constructors

        #endregion

        #region Methods

        public Datablock Decompress(Datablock src)
        {
            var trg = new Datablock();
            long oLength = 0;
            for (var i = 1; i <= 4; i++)
                oLength = (oLength << 8) | src.Data[src.Data.Length - i];

            using (var targetStream = new MemoryStream(src.Data))
            {
                using (var compressionStream = new GZipStream(targetStream, CompressionMode.Decompress))
                {
                    trg.Data = new byte[oLength];
                    int count = compressionStream.Read(trg.Data, 0, trg.Data.Length);
                }
            }

            return trg;
        }

        #endregion
    }
}
