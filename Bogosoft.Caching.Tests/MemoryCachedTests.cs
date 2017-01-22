using Bogosoft.Caching;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bogosoft.Caching.Tests
{
    [TestClass]
    public class MemoryCachedTests
    {
        [TestMethod]
        public void CanClear()
        {
            var cache = this.GetCache();

            cache.Save("Back, you robots! Nobody ruins my family vacation but me, and... maybe the boy!");
            cache.Save("Boy, everyone is stupid except me.");

            Assert.AreEqual(cache.Count, 2);
            Assert.IsTrue(cache.Clear());
            Assert.AreEqual(cache.Count, 0);
        }

        [TestMethod]
        public void CanDelete()
        {
            var cache = this.GetCache();

            cache.Save("Don't hassle the dead, boy. They have eerie powers.");

            Assert.AreEqual(cache.Count, 1);

            Assert.IsTrue(cache.Delete(0) == 1);

            Assert.AreEqual(cache.Count, 0);
        }

        [TestMethod]
        public void CanGet()
        {
            var cache = this.GetCache();

            var quote = "There's so much I don't know about astrophysics! I wish I read that book by that wheelchair guy.";

            cache.Save(quote);

            Assert.AreEqual(quote, cache.Get(0));
        }

        [TestMethod]
        public void CanSave()
        {
            var cache = this.GetCache();

            Assert.AreEqual(cache.Count, 0);

            cache.Save("Now for the easiest job for any coach... the cuts.");

            Assert.AreEqual(cache.Count, 1);
        }

        protected ICache<String, Int32> GetCache()
        {
            var count = 0;

            return new MemoryCache<String, Int32>(s => count++);
        }
    }
}