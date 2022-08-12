using Gort.DataStore.DataModel;

namespace Gort.DataStore.CauseBuild
{
    public static class CauseLoad
    {
        public static void LoadCauseBuilder(CauseBuildBase czBuilder, IGortContext2 ctxt)
        {
            var ws = ctxt.Workspace.Find(czBuilder.Workspace.WorkspaceId);
            if (ws == null)
            {
                ctxt.Workspace.Add(czBuilder.Workspace);
            }

            foreach (var pram in czBuilder.Params)
            {
                var res = ctxt.Param.Find(pram.ParamId);
                if (res == null)
                {
                    ctxt.Param.Add(pram);
                }
            }

            var resC = ctxt.CauseR.Find(czBuilder.CauseR.CauseRId);
            if (resC == null)
            {
                ctxt.CauseR.Add(czBuilder.CauseR);
            }

            foreach (var czP in czBuilder.CauseParamRs)
            {
                var res = ctxt.CauseParamR.Find(czP.CauseParamRId);
                if (res == null)
                {
                    ctxt.CauseParamR.Add(czP);
                }
            }

            ctxt.SaveChanges();
        }
    }
}
