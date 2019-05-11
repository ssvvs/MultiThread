namespace Tests
{
    using MultiThreadZip;
    using MultiThreadZip.SyncObjects;

    using NUnit.Framework;

    [TestFixture]
    public class DatablockProviderTests
    {
        #region Methods

        [Test]
        public void ProviderReleaseTest()
        {
            var provider = new DatablockProvider();

            Datablock firstBlock = provider.Take();
            provider.Release(firstBlock);
            Datablock secondDatablock = provider.Take();

            Assert.AreSame(firstBlock, secondDatablock);
        }

        [Test]
        public void ProviderTakeTest()
        {
            var provider = new DatablockProvider();

            Datablock firstBlock = provider.Take();
            Datablock secondDatablock = provider.Take();

            Assert.AreNotSame(firstBlock, secondDatablock);
        }

        #endregion
    }
}
