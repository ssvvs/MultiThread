namespace MultiThreadZip.SyncObjects
{
    public class DatablockProvider
    {
        #region Constants

        private const int BLOCK_SIZE = 2000000;

        #endregion

        #region Fields

        private readonly ObjectPool<Datablock> _pool;

        #endregion

        #region Constructors

        public DatablockProvider()
        {
            _pool = new ObjectPool<Datablock>(CreateNewBlock);
        }

        #endregion

        #region Methods

        public void Release(Datablock item)
        {
            _pool.PutObject(item);
        }

        public Datablock Take()
        {
            return _pool.GetObject();
        }

        private Datablock CreateNewBlock()
        {
            return new Datablock
            {
                Data = new byte[BLOCK_SIZE]
            };
        }

        #endregion
    }
}
