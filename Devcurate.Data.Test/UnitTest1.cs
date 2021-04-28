using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Devcurate.Data.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async Task TestAdd()
        {
            string guid = Guid.NewGuid().ToString();
            var add = await Main.Add(guid);
            var get = await Main.Get(add);
            Assert.AreEqual(guid, get);
        }

        [TestMethod]
        public async Task TestReuseRefId()
        {
            string guid1 = Guid.NewGuid().ToString();
            var add1 = await Main.Add(guid1);
            await Main.Delete(add1);
            string guid2 = Guid.NewGuid().ToString();
            var add2 = await Main.Add(guid2);
            Assert.AreEqual(add1,add2);
        }
    }
}
