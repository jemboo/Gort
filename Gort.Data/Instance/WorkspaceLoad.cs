using Gort.Data.DataModel;
using Gort.Data.Instance.CauseBuilder;
using Gort.Data.Instance.SeedParams;
using Gort.Data.Instance.StandardTypes;

namespace Gort.Data.Instance
{
    public static class WorkspaceLoad
    {
        public static void LoadCauseBuilder(CauseBuilderBase czBuilder, IGortContext ctxt)
        {
            var ws = ctxt.Workspace.Find(czBuilder.Workspace.WorkspaceId);
            if (ws == null)
            {
                ctxt.Workspace.Add(czBuilder.Workspace);
            }

            foreach (var pram in czBuilder.Params)
            {
                var res = ctxt.Param.Find(pram.ParamId);
                if(res == null)
                {
                    ctxt.Param.Add(pram);
                }
            }

            foreach (var cz in czBuilder.Causes)
            {
                var res = ctxt.Cause.Find(cz.CauseId);
                if (res == null)
                {
                    ctxt.Cause.Add(cz);
                }
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

        public static void LoadSeedParams(SeedParamsBase seedParams, IGortContext ctxt)
        {
            foreach (var pram in seedParams.Members)
            {
                var res = ctxt.Param.Find(pram.ParamId);
                if (res == null)
                {
                    ctxt.Param.Add(pram);
                }
            }
            ctxt.SaveChanges();
        }

        public static void LoadStatics(IGortContext ctxt)
        {
            foreach (var ctg in CauseTypeGroups.Members)
            {
                var res = ctxt.CauseTypeGroup.Find(ctg.CauseTypeGroupId);
                if (res == null)
                {
                    ctxt.CauseTypeGroup.Add(ctg);
                }
            }

            foreach (var pt in ParamTypes.Members)
            {
                var res = ctxt.ParamType.Find(pt.ParamTypeId);
                if (res == null)
                {
                    ctxt.ParamType.Add(pt);
                }
            }

            foreach (var ct in CauseTypes.Members)
            {
                var res = ctxt.CauseType.Find(ct.CauseTypeId);
                if (res == null)
                {
                    ctxt.CauseType.Add(ct);
                }
            }

            foreach (var cpt in CauseParamTypes.Members)
            {
                var res = ctxt.CauseParamType.Find(cpt.CauseParamTypeId);
                if (res == null)
                {
                    ctxt.CauseParamType.Add(cpt);
                }
            }

            ctxt.SaveChanges();
        }

    }
}
