using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.FSharp.Core;
using System.Linq;
using System;
using Gort.Data.Utils;

namespace Gort.Data.Test
{
    [TestClass]
    public class DataRepoFixture
    {
        Guid gu = Guid.Parse("fd100b52-f74e-8930-3cef-bc1f657a82a5");

        [TestMethod]
        public void GetCauseById()
        {
            var cas = MetaDataUtils.GetCauseById(gu);
            Assert.IsNotNull(cas);
        }

        [TestMethod]
        public void GetAllCausesForWorkspace()
        {
            var realWs = MetaDataUtils.GetAllCausesForWorkspace("WorkspaceRand");
            Assert.IsTrue (realWs.Length > 0);
        }

        [TestMethod]
        public void GetCauseTypeGroupAncestry()
        {
            var cause = MetaDataUtils.GetCauseById(gu);
            var realWs = MetaDataUtils.GetCauseTypeGroupAncestry(cause).ToArray();
            Assert.IsTrue(realWs.Length > 0);
        }

        [TestMethod]
        public void Tst()
        {

            Assert.IsTrue(true);
        }

    }
}