using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gort.Data
{
    public static class MetaDataUtils
    {

        public static IEnumerable<Cause> GetAllCausesForWorkspace(string workspaceName, GortContext? gortContext = null)
        {
            var ctxt = gortContext ?? new GortContext();
            var ws = ctxt.Workspace.Where(c => c.Name == workspaceName).FirstOrDefault();
            if (ws is null)
            {
                return Enumerable.Empty<Cause>();
            }
            return ctxt.Cause.Where(c => c.Workspace == ws);
        }

        public static Cause GetCauseById(Guid causeId, GortContext? gortContext = null)
        {
            try
            {
                var ctxt = gortContext ?? new GortContext();
                return ctxt.Cause.Where(c => c.CauseId == causeId).First();
            }
            catch (Exception ex)
            {
                throw new Exception($"cause {causeId} not found", ex);
            }
        }

        public static CauseType GetCauseType(this Cause cause, GortContext? gortContext = null)
        {
            try
            {
                var ctxt = gortContext ?? new GortContext();
                return ctxt.CauseType.Where(ct => ct.CauseTypeId == cause.CauseTypeID).First();
            }
            catch (Exception ex)
            {
                throw new Exception($"causetype {cause.CauseTypeID} not found", ex);
            }
        }

        public static CauseTypeGroup GetCauseTypeGroup(this CauseType ct, GortContext? gortContext = null)
        {
            try
            {
                var ctxt = gortContext ?? new GortContext();
                return ctxt.CauseTypeGroup.Where(ct => ct.CauseTypeGroupId == ct.CauseTypeGroupId).First();
            }
            catch (Exception ex)
            {
                throw new Exception($"ctGroup {ct.CauseTypeGroupId} not found", ex);
            }
        }

        public static CauseTypeGroup? GetCauseTypeGroupParent(this CauseTypeGroup ctGroup, GortContext? gortContext = null)
        {
            try
            {
                var ctxt = gortContext ?? new GortContext();
                return ctxt.CauseTypeGroup.Where(ct => ct.CauseTypeGroupId == ctGroup.ParentId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception($"ctGroup {ctGroup.ParentId} not found", ex);
            }
        }

        public static IEnumerable<CauseTypeGroup> GetCauseTypeGroupAncestors(this CauseType ct, GortContext? gortContext = null)
        {
            var ctxt = gortContext ?? new GortContext();
            Guid? ctgId = ct.CauseTypeGroupId;
            while (ctgId is not null)
            {
                var ctGroup = ctxt.CauseTypeGroup.Where(ctg => ctg.CauseTypeGroupId == ctgId).First();
                yield return ctGroup;
                ctgId = ctGroup?.ParentId;
            }
        }

        public static Tuple<CauseType, CauseTypeGroup[]> GetBuildInfo(this Cause cause, GortContext? gortContext = null)
        {
            try
            {
                var ctxt = gortContext ?? new GortContext();
                var ct = cause.GetCauseType(gortContext);
                var ancestors = ct.GetCauseTypeGroupAncestors().ToArray();
                return new Tuple<CauseType, CauseTypeGroup[]>(
                        cause.GetCauseType(gortContext),
                        ct.GetCauseTypeGroupAncestors().ToArray()
                    );
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in GetBuildInfo for cause {cause.CauseId} not found", ex);
            }
        }

        //public static CauseParam GetOrMakeCauseParam(ParamType cpt, byte[] value, GortContext? gortContext = null)
        //{
        //    var ctxt = gortContext ?? new GortContext();
        //    var id = GuidUtils.guidFromObjs(new object[2] { cpt.ParamTypeId, value });
        //    var causeParam = gortContext.CauseParam.Where(cp => cp.CauseParamId == id).FirstOrDefault();
        //    if (causeParam != null) return causeParam;
        //    else
        //    {

        //    }
        //}

    }
}
