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

            var resC = ctxt.Cause.Find(czBuilder.Cause.CauseId);
            if (resC == null)
            {
                ctxt.Cause.Add(czBuilder.Cause);
            }

            foreach (var czP in czBuilder.CauseParams)
            {
                var res = ctxt.CauseParam.Find(czP.CauseParamId);
                if (res == null)
                {
                    ctxt.CauseParam.Add(czP);
                }
            }

            ctxt.SaveChanges();
        }
    }
}
