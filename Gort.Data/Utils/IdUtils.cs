using Gort.Data;
using Gort.Data.DataModel;

namespace Gort.Data.Utils
{
    public static class IdUtils
    {
        public static Workspace AddId(this Workspace ctg)
        {
            var id = GuidUtils.guidFromObjs(new object[1] { ctg.Name });
            return new Workspace() { WorkspaceId = id, Name = ctg.Name };
        }
        public static CauseTypeGroup AddId(this CauseTypeGroup ctg)
        {
            var id = GuidUtils.guidFromObjs(new object[2] { ctg.Name, ctg.ParentId});
            return new CauseTypeGroup() { CauseTypeGroupId = id, Name = ctg.Name, ParentId = ctg.ParentId };
        }
        public static CauseType AddId(this CauseType ct)
        {
            var id = GuidUtils.guidFromObjs(new object[2] { ct.Name, ct.CauseTypeGroup?.CauseTypeGroupId });
            return new CauseType() { CauseTypeId = id, Name = ct.Name, CauseTypeGroupId = ct.CauseTypeGroupId };
        }
        public static ParamType AddId(this ParamType ctp)
        {
            var id = GuidUtils.guidFromObjs(new object[1] { ctp.Name });
            return new ParamType() { ParamTypeId = id, Name = ctp.Name, DataType = ctp.DataType };
        }
        public static CauseParamType AddId(this CauseParamType ctp)
        {
            var id = GuidUtils.guidFromObjs(new object[2] { ctp.CauseTypeId, ctp.Name });
            return new CauseParamType() { CauseParamTypeId = id, CauseTypeId = ctp.CauseTypeId, Name = ctp.Name, ParamTypeId = ctp.ParamTypeId };
        }
        public static Param AddId(this Param prm)
        {
            var id = GuidUtils.guidFromObjs(new object[2] { prm.ParamTypeId, prm.Value });
            return new Param() { ParamId = id, ParamTypeId = prm.ParamTypeId, Value = prm.Value };
        }
        public static Cause AddId(this Cause cs)
        {
            var id = GuidUtils.guidFromObjs(new object[2] { cs.Index, cs.WorkspaceId});
            return new Cause() { CauseId = id, CauseDescr = cs.CauseDescr, CauseTypeID = cs.CauseTypeID, CauseParams = cs.CauseParams, Index=cs.Index, WorkspaceId = cs.WorkspaceId };
        }
        public static CauseParam AddId(this CauseParam cp)
        {
            var id = GuidUtils.guidFromObjs(new object[2] { cp.CauseId, cp.CauseParamTypeId });
            return new CauseParam() { CauseParamId = id, CauseId = cp.CauseId, CauseParamTypeId = cp.CauseParamTypeId, ParamId = cp.ParamId };
        }
        public static DataModel.RandGen AddStructId(this DataModel.RandGen rg)
        {
            var ids = GuidUtils.guidFromObjs(new object[2] { rg.RandGenType, rg.Seed });
            var id = GuidUtils.guidFromObjs(new object[2] { rg.CauseId, rg.CausePath });
            return new DataModel.RandGen() { RandGenId = id, StructId = ids, RandGenType = rg.RandGenType, Seed = rg.Seed, CauseId = rg.CauseId, CausePath = rg.CausePath };
        }
        public static DataModel.Sorter AddStructId(this DataModel.Sorter sorter)
        {
            var id = GuidUtils.guidFromObjs(new object[1] { sorter.Data });
            return new DataModel.Sorter() { SorterId = sorter.SorterId, StructId = id, CauseId = sorter.CauseId, Description = sorter.Description, Order = sorter.Order, Data = sorter.Data };
        }
    }
}
