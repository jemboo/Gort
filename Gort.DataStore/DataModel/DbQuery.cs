using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Gort.DataStore.DataModel
{
    public static class DbQuery
    {
        public static CauseR? GetPendingCauseForWorkspace(string workspaceName, IGortContext2? gortContext = null)
        {
            try
            {
                var ctxt = gortContext ?? new GortContext2();
                var ws = ctxt.Workspace.SingleOrDefault(c => c.Name == workspaceName);
                if (ws is null)
                {
                    throw new Exception($"Workspace \"{workspaceName}\" not found");
                }
                var plainCause = 
                    ctxt.CauseR.Where(
                                  c => c.Workspace == ws && c.CauseStatus != CauseStatus.Complete
                                )
                                .OrderBy(c => c.Index).FirstOrDefault();

                if (plainCause == null) return null;
                return TableById.GetCauseById(plainCause.CauseRId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static class TableById
        {
            public static BitPackR? GetBitPackRById(int bitPackRId, IGortContext2? gortContext = null)
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

            public static ComponentR? GetRandGenRById(int rndGenId, IGortContext2? gortContext = null)
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

            public static SortableSetR? GetSortableSetRById(int sortableSetRId, IGortContext2? gortContext = null)
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

            public static SorterR? GetSorterRById(int sorterRId, IGortContext2? gortContext = null)
            {
                try
                {
                    var ctxt = gortContext ?? new GortContext2();
                    return ctxt.SorterR.SingleOrDefault(c =>
                            c.SorterRId == sorterRId);
                }
                catch (Exception)
                {
                    throw;
                }
            }

            public static SorterSetR? GetSorterSetRById(int sorterSetRId, IGortContext2? gortContext = null)
            {
                try
                {
                    var ctxt = gortContext ?? new GortContext2();
                    return ctxt.SorterSetR.SingleOrDefault(c =>
                            c.SorterSetRId == sorterSetRId);
                }
                catch (Exception)
                {
                    throw;
                }
            }

        }

    }
}
