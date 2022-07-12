using Gort.Data.DataModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gort.Data.Utils
{
    public static class CauseQuery
    {
        public static Cause[] GetAllCausesForWorkspace(string workspaceName,
            IGortContext? gortContext = null)
        {
            try
            {
                var ctxt = gortContext ?? new GortContext();
                var ws = ctxt.Workspace.SingleOrDefault(c => c.Name == workspaceName);
                if (ws is null)
                {
                    throw new Exception($"Workspace \"{workspaceName}\" not found");
                }
                return ctxt.Cause.Where(c => c.Workspace == ws).ToArray();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static Cause? GetPendingCauseForWorkspace(string workspaceName,
            IGortContext? gortContext = null)
        {
            try
            {
                var ctxt = gortContext ?? new GortContext();
                var ws = ctxt.Workspace.SingleOrDefault(c => c.Name == workspaceName);
                if (ws is null)
                {
                    throw new Exception($"Workspace \"{workspaceName}\" not found");
                }
                var plainCause = ctxt.Cause.Where(c => (c.Workspace == ws) && 
                                                 (c.CauseStatus != CauseStatus.Complete))
                                           .OrderBy(c => c.Index).FirstOrDefault();

                if (plainCause == null) return null;
                return GetCauseById(plainCause.CauseId);

            }
            catch (Exception)
            {
                throw;
            }
        }

        public static Cause GetCauseById(Guid causeId, IGortContext? gortContext = null)
        {
            try
            {
                var ctxt = gortContext ?? new GortContext();
                var qua = ctxt.Cause.Where(c => c.CauseId == causeId)
                                    .Include(s => s.CauseParams).SingleOrDefault();
                if (qua is null)
                {
                    throw new Exception($"cause {causeId} not found");
                }
                ctxt.CauseType.Where(P => P.CauseTypeId == qua.CauseTypeID).Load();

                foreach (var cp in qua.CauseParams)
                {
                    ctxt.Param.Where(p => p.ParamId == cp.ParamId).Load();
                    ctxt.ParamType.Where(P => P.ParamTypeId == cp.Param.ParamTypeId).Load();
                    ctxt.CauseParamType.Where(p => p.CauseParamTypeId == cp.CauseParamTypeId).Load();
                }
                return qua;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public static CauseType GetCauseType(this Cause cause, IGortContext? gortContext = null)
        {
            try
            {
                var ctxt = gortContext ?? new GortContext();
                var ct = ctxt.CauseType.SingleOrDefault(ct => ct.CauseTypeId == cause.CauseTypeID);
                if (ct is null)
                {
                    throw new Exception($"causetype {cause.CauseTypeID} not found");
                }
                return ct;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static CauseTypeGroup GetCauseTypeGroup(this CauseType ct, IGortContext? gortContext = null)
        {
            try
            {
                var ctxt = gortContext ?? new GortContext();
                var ctg = ctxt.CauseTypeGroup.SingleOrDefault(ct => ct.CauseTypeGroupId == ct.CauseTypeGroupId);
                if (ctg is null)
                {
                    throw new Exception($"CauseTypeGroup {ct.CauseTypeGroupId} not found");
                }
                return ctg;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static CauseTypeGroup GetCauseTypeGroupParent(this CauseTypeGroup ctGroup, IGortContext? gortContext = null)
        {
            try
            {
                var ctxt = gortContext ?? new GortContext();
                var ctgp = ctxt.CauseTypeGroup.SingleOrDefault(ct => ct.CauseTypeGroupId == ctGroup.ParentId);
                if (ctgp is null)
                {
                    throw new Exception($"ctGroup {ctGroup.ParentId} not found");
                }
                return ctgp;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static ParamType GetParamType(this ParamTypeName name, IGortContext? gortContext = null)
        {
            try
            {
                var ctxt = gortContext ?? new GortContext();
                return ctxt.ParamType.SingleOrDefault(pt => pt.Name == name.ToString());
            }
            catch (Exception ex)
            {
                throw new Exception($"ParamType with name {name} not found", ex);
            }
        }

        public static IEnumerable<CauseTypeGroup> GetCauseTypeGroupAncestors(this CauseType ct,
            IGortContext? gortContext = null)
        {
            var ctxt = gortContext ?? new GortContext();
            Guid? ctgId = ct.CauseTypeGroupId;
            while (ctgId is not null)
            {
                var ctGroup = ctxt.CauseTypeGroup.SingleOrDefault(ctg => ctg.CauseTypeGroupId == ctgId);
                yield return ctGroup;
                ctgId = ctGroup?.ParentId;
            }
        }

        public static CauseTypeGroup[] GetCauseTypeGroupAncestry(this Cause cause,
            IGortContext? gortContext = null)
        {
            try
            {
                var ctxt = gortContext ?? new GortContext();
                var ct = cause.GetCauseType(gortContext);
                var ancestors = ct.GetCauseTypeGroupAncestors().ToArray().Reverse().ToArray();
                return ancestors;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in GetCauseTypeGroupAncestry for cause {cause.CauseId}", ex);
            }
        }
    }
}
