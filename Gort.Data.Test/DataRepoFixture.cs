using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.FSharp.Core;
using System.Linq;
using System;

namespace Gort.Data.Test
{
    [TestClass]
    public class DataRepoFixture
    {
        [TestMethod]
        public void GetCauseById()
        {
            var gu = Guid.Parse("08da05ac-e312-4969-8f78-e9e18ae1c20c");
            var cas = MetaDataUtils.GetCauseById(gu);
            Assert.IsNotNull(cas);
        }

        [TestMethod]
        public void GetAllCausesForWorkspace()
        {
            var noWs = MetaDataUtils.GetAllCausesForWorkspace("notAWs").ToArray();
            Assert.AreEqual (noWs.Length, 0);
            var realWs = MetaDataUtils.GetAllCausesForWorkspace("workspace1").ToArray();
            Assert.IsTrue (realWs.Length > 0);
        }

        [TestMethod]
        public void GetCauseTypeGroupAncestors()
        {
            var gu = Guid.Parse("08da05ac-e312-4969-8f78-e9e18ae1c20c");
            var ct = MetaDataUtils.GetCauseById(gu).GetCauseType();
            var realWs = MetaDataUtils.GetCauseTypeGroupAncestors(ct).ToArray();
            Assert.IsTrue(true);
        }


        [TestMethod]
        public void GetBuildInfo()
        {
            var gu = Guid.Parse("08da05ac-e312-4969-8f78-e9e18ae1c20c");
            var cause = MetaDataUtils.GetCauseById(gu);
            var realWs = MetaDataUtils.GetBuildInfo(cause);
            Assert.IsTrue(true);
        }

    }
}