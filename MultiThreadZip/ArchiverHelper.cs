namespace MultiThreadZip
{
    using System;
    using System.IO;

    public class ArchiverHelper
    {
        #region Constants

        public const int EXTRA_FIELD_LENGTH = 9;
        private const int EXTRA_DATA_OFFSET = 12;

        private const byte EXTRA_FIELD_OFFSET = 1 << 2;
        private const int FLG_OFFSET = 3;
        private const byte LEN_H = 0;
        private const byte LEN_L = 3;
        private const byte SI1 = 110;
        private const byte SI2 = 101;
        private const byte XLEN_H = EXTRA_FIELD_LENGTH / 256;
        private const byte XLEN_L = EXTRA_FIELD_LENGTH % 256 - 2;
        private const int XLEN_OFFSET = 10;

        #endregion

        #region Fields

        private static readonly byte[] header = { XLEN_L, XLEN_H, SI1, SI2, LEN_L, LEN_H, 0, 0, 0 };

        #endregion

        #region Methods

        public static int GetBlockLegth(MemoryStream ms)
        {
            var count = 0;
            var offset = 0;
            try
            {
                var array = new byte[XLEN_OFFSET + EXTRA_FIELD_LENGTH];
                ms.Position = 0;
                ms.Read(array, 0, XLEN_OFFSET + EXTRA_FIELD_LENGTH);
                if ((array[FLG_OFFSET] & EXTRA_FIELD_OFFSET) != 0)
                {
                    offset = XLEN_OFFSET;
                    for (var i = 0; i < header.Length - 3; i++)
                        if (array[offset++] != header[i])
                            return 0;
                    count |= array[offset++];
                    count |= array[offset++] << 8;
                    count |= array[offset++] << 16;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }

            return count;
        }

        public static byte[] GetHeaderWithLength(MemoryStream ms)
        {
            ms.Position = 0;
            long sourceArrayLength = ms.Length + EXTRA_FIELD_LENGTH;
            var array = new byte[sourceArrayLength];

            ms.Read(array, 0, 10);
            for (var i = 0; i < header.Length; i++)
                array[10 + i] = header[i];

            array[FLG_OFFSET] |= EXTRA_FIELD_OFFSET;

            array[XLEN_OFFSET + 6] = (byte)sourceArrayLength;
            array[XLEN_OFFSET + 7] = (byte)(sourceArrayLength >> 8);
            array[XLEN_OFFSET + 8] = (byte)(sourceArrayLength >> 16);

            int resulOffset = XLEN_OFFSET + EXTRA_FIELD_LENGTH;
            ms.Read(array, resulOffset, (int)sourceArrayLength - resulOffset);

            return array;
        }

        #endregion
    }
}
