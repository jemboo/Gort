using Gort.DataStore.CauseBuild;
using Gort.DataStore.DataModel;
using Microsoft.EntityFrameworkCore;

namespace Gort.DataStore.Test
{
    [TestClass]
    public class RecordStorageFixture
    {
        const string WorkspaceName = "Ralph2";
        Workspace makeWorkspace()
        {
            var ws = new Workspace();
            ws.Name = WorkspaceName;
            ws = ws.AddId();
            return ws;
        }

        bool workSpaceSelector(Workspace workspace)
        {
            return workspace.Name == WorkspaceName;
        }

        [TestMethod]
        public void TestMethod1()
        {
            var ctx = new GortContext2();
            var ws = ctx.Workspace.GetOrMake(workSpaceSelector, makeWorkspace);
            ctx.SaveChanges();
        }

        [TestMethod]
        public void TestMethod2()
        {
            var ctx = new GortContext2();
            var ws = ctx.Workspace.GetOrMake(workSpaceSelector, makeWorkspace);
            var cz = new CauseR();
            cz.Genus = "Genus";
            cz.Species = "Species";
            cz.Comments = "Comments";
            ctx.CauseR.Add(cz);
            ctx.SaveChanges();
        }

        [TestMethod]
        public void TestMethod3()
        {
            var ctx = new GortContext2();
            var ws = ctx.Workspace.GetOrMake(workSpaceSelector, makeWorkspace); 
            ctx.SaveChanges();
            var qq = ctx.Workspace.Remove(ws);
            ctx.SaveChanges();
        }

        [TestMethod]
        public void TestMethod4()
        {
            var ctx = new GortContext2();
            var pp = new Param();
            pp.Name = "ParamName";
            pp.ParamDataType = ParamDataType.String;
            ctx.Param.Add(pp);
            ctx.SaveChanges();
        }

        [TestMethod]
        public void TestMethod5()
        {
            var ctx = new GortContext2();
            var ws = ctx.Workspace.GetOrMake(workSpaceSelector, makeWorkspace);
            var cc = new CauseR();
            cc.CauseStatus = CauseStatus.Error;
            cc.Genus = "Genus8";
            cc.Species = "Species8";
            cc.Index = 888;
            cc.Workspace = ws;

            var cp = new CauseParamR();
            var pp = new Param();
            pp.ParamDataType = ParamDataType.String;
            pp.Name = "ParamName";

            cp.CauseR = cc;
            cp.Name = "CauseParamName";
            cp.Param = pp;
           

            //ctx.Workspace.Add(ws);
            ctx.CauseR.Add(cc);
            ctx.CauseParamR.Add(cp);
            ctx.Param.Add(pp);


            ctx.SaveChanges();
        }


        [TestMethod]
        public void TestMethod6()
        {
            var ctx = new GortContext2();
            var ws = ctx.Workspace.GetOrMake(workSpaceSelector, makeWorkspace);

            var paramSeed = Utils.MakeParam("Seed", ParamDataType.Int32, (int)rngType.Lcg);
            var paramRngType = Utils.MakeParam("RngType", ParamDataType.Int32, 123);

            var cbRndGen = new CauseBuildRndGen(paramSeed, paramRngType, ws, 1);
            CauseLoad.LoadCauseBuilder(cbRndGen, ctx);

            ctx.SaveChanges();
        }



    }
}