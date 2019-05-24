namespace MultiThreadInter.Heap
{
    using System.IO;
    using System.Threading;
 

    public interface IMultithreadZiper
    {
        void Zip(string srcFilepath, string trgFilePath);
    }

    public class MultiThreadZipper : IMultithreadZiper
    {
        private Thread[] _threads;

        public void Zip(string srcFilepath, string trgFilePath)
        {
            FileInfo info = new FileInfo(srcFilepath);
            
        }

        private void DoWork()
        {

        }
    }

}
