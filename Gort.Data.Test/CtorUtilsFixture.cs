using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.FSharp.Core;
using System;

namespace Gort.Data.Test
{
    [TestClass]
    public class CtorUtilsFixture
    {
        [TestMethod]
        public void Params()
        {
            var intPiD = Guid.NewGuid();
            var intViN = 42;
            var intP = CtorUtils.MakeIntParam(intPiD, intViN);
            var intVout = intP.IntValue();
            Assert.AreEqual(intViN, intVout);

            var dblPiD = Guid.NewGuid();
            var dblViN = 42.42;
            var dblP = CtorUtils.MakeDoubleParam(dblPiD, dblViN);
            var dblVout = dblP.DoubleValue();
            Assert.AreEqual(dblViN, dblVout);

            var strPiD = Guid.NewGuid();
            var strViN = "test";
            var strP = CtorUtils.MakeStringParam(strPiD, strViN);
            var strVout = strP.StringValue();
            Assert.AreEqual(strViN, strVout);

            var guPiD = Guid.NewGuid();
            var guViN = Guid.NewGuid();
            var guP = CtorUtils.MakeGuidParam(guPiD, guViN);
            var guVot = guP.GuidValue();
            Assert.AreEqual(guViN, guVot);
        }

        [TestMethod]
        public void ArrayParams()
        {
            var intPiD = Guid.NewGuid();
            var intViN = new[] { 22, 32, 42 };
            var intP = CtorUtils.MakeIntArrayParam(intPiD, intViN);
            var intVout = intP.IntArrayValue();
            CollectionAssert.AreEqual(intViN, intVout);

            var dblPiD = Guid.NewGuid();
            var dblViN = new[] { 22.22, 32.22, 42.22 };
            var dblP = CtorUtils.MakeDoubleArrayParam(dblPiD, dblViN);
            var dblVout = dblP.DoubleArrayValue();
            CollectionAssert.AreEqual(dblViN, dblVout);

            var strPiD = Guid.NewGuid();
            var strViN = "test";
            var strP = CtorUtils.MakeStringParam(strPiD, strViN);
            var strVout = strP.StringValue();
            Assert.AreEqual(strViN, strVout);

            var straPiD = Guid.NewGuid();
            var straViN = new[] { "test1", "test2", "test3" };
            var straP = CtorUtils.MakeStringArrayParam(straPiD, straViN);
            var straVout = straP.StringArrayValue();
            CollectionAssert.AreEqual(straViN, straVout);

            var guPiD = Guid.NewGuid();
            var guViN = new[] { Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() };
            var guP = CtorUtils.MakeGuidArrayParam(guPiD, guViN);
            var guVot = guP.GuidArrayValue();
            CollectionAssert.AreEqual(guViN, guVot);
        }


    }
}