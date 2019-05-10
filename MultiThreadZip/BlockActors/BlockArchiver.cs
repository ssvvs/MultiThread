namespace MultiThreadZip.BlockActors
{
    using System.IO;
    using System.IO.Compression;

    public class BlockArchiver
    {
        #region Constructors

        #endregion

        #region Methods

        public Datablock Compress(Datablock src)
        {
            var trg = new Datablock();

            using (var targetStream = new MemoryStream())
            {
                using (var compressionStream = new GZipStream(targetStream, CompressionMode.Compress, true))
                {
                    compressionStream.Write(src.Data, 0, src.Data.Length);
                }

                trg.Data = ArchiverHelper.GetHeaderWithLength(targetStream);
            }

            return trg;
        }

        #endregion
    }
}
