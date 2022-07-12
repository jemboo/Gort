using Gort.Data.Instance.StandardTypes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Gort.Data.Test
{
    [TestClass]
    public class InstanceFixture
    {
        [TestMethod]
        public void UnqParamTypes()
        {
            var intPiD = ParamTypes.Members.GroupBy(m =>m.ParamTypeId)
                                    .OrderBy(g => - g.Count())
                                    .ToArray();

            Assert.AreEqual(intPiD.Count(), ParamTypes.Members.Count());
        }

        [TestMethod]
        public void UnqCauseTypes()
        {
            var intPiD = CauseTypes.Members.GroupBy(m => m.CauseTypeId)
                                   .OrderBy(g => -g.Count())
                                   .ToArray();

            Assert.AreEqual(intPiD.Count(), CauseTypes.Members.Count());
        }

        [TestMethod]
        public void UnqCauseTypeGroups()
        {
            var intPiD = CauseTypeGroups.Members.GroupBy(m => m.CauseTypeGroupId)
                                   .OrderBy(g => -g.Count())
                                   .ToArray();

            Assert.AreEqual(intPiD.Count(), CauseTypeGroups.Members.Count());
        }

        [TestMethod]
        public void UnqCauseParamTypes()
        {
            var intPiD = CauseParamTypes.Members.GroupBy(m => m.CauseParamTypeId)
                                   .OrderBy(g => -g.Count())
                                   .ToArray();

            Assert.AreEqual(intPiD.Count(), CauseParamTypes.Members.Count());
        }


    }
}