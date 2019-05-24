namespace MultiThreadInter.BlockActors
{
    using System.IO;
    using System.IO.Compression;

    public class BlockArchiver
    {
        #region Methods

        public void Compress(Datablock src, Datablock trg)
        {
            using (var targetStream = new MemoryStream())
            {
                using (var compressionStream = new GZipStream(targetStream, CompressionMode.Compress, true))
                {
                    compressionStream.Write(src.Data, 0, src.Count);
                }

                ArchiverHelper.GetHeaderWithLength(targetStream, trg);
            }
        }

        #endregion
    }
}
