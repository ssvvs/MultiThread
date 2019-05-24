﻿namespace MultiThreadInter.BlockActors
{
    using System.IO;

    public class BlockReader
    {
        #region Fields

        private readonly string _filepath;

        #endregion

        #region Constructors

        public BlockReader(string filepath)
        {
            _filepath = filepath;
        }

        #endregion

        #region Methods

        public bool Read(Datablock block, long startPosition, int count)
        {
            using (FileStream fstream = File.OpenRead(_filepath))
            {
                using (var wr = new BinaryReader(fstream))
                {
                    wr.BaseStream.Position = startPosition;
                    wr.Read(block.Data, 0, count);
                }
            }

            block.Count = count;
            return true;
        }

        #endregion
    }
}
