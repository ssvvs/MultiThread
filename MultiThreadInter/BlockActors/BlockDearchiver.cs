namespace MultiThreadInter.BlockActors
{
    using System.IO;
    using System.IO.Compression;

    public class BlockDearchiver
    {
        #region Methods

        public void Decompress(Datablock src, Datablock trg)
        {
            var oLength = 0;
            for (var i = 1; i <= 4; i++)
                oLength = (oLength << 8) | src.Data[src.Count - i];
            trg.Count = oLength;

            using (var targetStream = new MemoryStream(src.Data))
            {
                using (var compressionStream = new GZipStream(targetStream, CompressionMode.Decompress))
                {
                    compressionStream.Read(trg.Data, 0, trg.Count);
                }
            }
        }

        #endregion
    }
}
