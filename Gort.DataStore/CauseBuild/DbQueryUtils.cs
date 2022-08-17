using Gort.DataStore.DataModel;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Gort.DataStore.CauseBuild
{
    public static class DbQueryUtils
    {
        public static CauseR GetCauseById(int causeId, IGortContext2? gortContext = null)
        {
            try
            {
                var ctxt = gortContext ?? new GortContext2();
                var qua = ctxt.CauseR.Where(c => c.CauseRId == causeId)
                                    .Include(s => s.CauseParamRs).SingleOrDefault();
                if (qua is null)
                {
                    throw new Exception($"cause {causeId} not found");
                }
                foreach (var cp in qua.CauseParamRs)
                {
                    ctxt.Param.Where(p => p.ParamId == cp.ParamId).Load();
                }
                return qua;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static ComponentR? GetRandGenRById(int rndGenId, 
                        IGortContext2? gortContext = null)
        {
            try
            {
                var ctxt = gortContext ?? new GortContext2();
                return ctxt.RandGenR.SingleOrDefault(c => c.ComponentRId == rndGenId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static SortableSetR? GetSortableSetRById(int sortableSetRId,
                        IGortContext2? gortContext = null)
        {
            try
            {
                var ctxt = gortContext ?? new GortContext2();
                return ctxt.SortableSetR.SingleOrDefault(c => 
                        c.SortableSetRId == sortableSetRId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static BitPackR? GetBitPackRById(int bitPackRId,
                        IGortContext2? gortContext = null)
        {
            try
            {
                var ctxt = gortContext ?? new GortContext2();
                return ctxt.BitPackR.SingleOrDefault(c => c.BitPackRId == bitPackRId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static CauseR? GetPendingCauseForWorkspace(string workspaceName,
            IGortContext2? gortContext = null)
        {
            try
            {
                var ctxt = gortContext ?? new GortContext2();
                var ws = ctxt.Workspace.SingleOrDefault(c => c.Name == workspaceName);
                if (ws is null)
                {
                    throw new Exception($"Workspace \"{workspaceName}\" not found");
                }
                var plainCause = ctxt.CauseR.Where(c => c.Workspace == ws &&
                                                 c.CauseStatus != CauseStatus.Complete)
                                           .OrderBy(c => c.Index).FirstOrDefault();

                if (plainCause == null) return null;
                return GetCauseById(plainCause.CauseRId);

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
