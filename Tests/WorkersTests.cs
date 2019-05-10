namespace Tests
{
    using System;
    using System.IO;

    using MultiThreadZip.Workers;

    using NUnit.Framework;

    [TestFixture]
    public class WorkersTests
    {
        #region Fields

        private string _srcFilePath;
        private string _trgFilePath;

        #endregion

        #region Methods

        [SetUp]
        public void Setup()
        {
            _srcFilePath = Path.GetTempFileName();
            _trgFilePath = Path.GetTempFileName();
        }

        [TearDown]
        public void Teardown()
        {
            if (File.Exists(_srcFilePath))
                File.Delete(_srcFilePath);
            if (File.Exists(_trgFilePath))
                File.Delete(_trgFilePath);
        }

        [TestCase(1)]
        [TestCase(0xFFFF)]
        [TestCase(3000000)]
        [TestCase(0xFFFFFF)]
        public void ZipWorkerTest(long count)
        {
            WriteTestData(count);
            var worker = new ZipWorker(_srcFilePath, _trgFilePath);
        }

        private bool CheckFileEquality(string src, string trg)
        {
            try
            {
                using (var fs1 = new FileStream(src, FileMode.Open, FileAccess.Read))
                {
                    using (var fs2 = new FileStream(trg, FileMode.Open, FileAccess.Read))
                    {
                        if (fs1.Length != fs2.Length)
                            return false;
                        int b1, b2;
                        do
                        {
                            b1 = fs1.ReadByte();
                            b2 = fs2.ReadByte();
                            if (b1 != b2)
                                return false;
                        } while (fs1.Position < fs1.Length);
                    }
                }
            }
            catch
            {
                return false;
            }

            return true;
        }

        private void WriteTestData(long count)
        {
            const long CHUNK_SIZE = 1000000;

            using (var wr = new BinaryWriter(File.OpenWrite(_srcFilePath)))
            {
                var random = new Random();
                var buffer = new byte[CHUNK_SIZE];
                long remainWrite = count;
                while (remainWrite > 0)
                {
                    random.NextBytes(buffer);
                    int stepSize = remainWrite > CHUNK_SIZE ? buffer.Length : (int)remainWrite;
                    wr.Write(buffer, 0, stepSize);
                    remainWrite -= stepSize;
                }
            }
        }

        #endregion
    }
}
