namespace Gort.Data
{
    public static class EntityUtils
    {
        public static CauseTypeGroup AddId(this CauseTypeGroup ctg)
        {
            var id = GuidUtils.guidFromObjs(new object[2] { ctg.Name, ctg.ParentId});
            return new CauseTypeGroup() { CauseTypeGroupId = id, Name = ctg.Name, Parent = ctg.Parent };
        }

        public static CauseType AddId(this CauseType ct)
        {
            var id = GuidUtils.guidFromObjs(new object[2] { ct.Name, ct.CauseTypeGroup.CauseTypeGroupId });
            return new CauseType() { CauseTypeId = id, Name = ct.Name, CauseTypeGroup = ct.CauseTypeGroup };
        }

        public static CauseTypeParam AddId(this CauseTypeParam ctp)
        {
            var id = GuidUtils.guidFromObjs(new object[2] { ctp.Name, ctp.CauseType.CauseTypeId });
            return new CauseTypeParam() { CauseTypeParamId = id, Name = ctp.Name, CauseType = ctp.CauseType, CauseTypeId = ctp.CauseTypeId, DataType = ctp.DataType };
        }

        public static Cause AddId(this Cause cs)
        {
            var id = GuidUtils.guidFromObjs(new object[4] { cs.Index, cs.WorkspaceId, cs.CauseTypeID, cs.CauseParams });
            return new Cause() { CauseId = id, Description = cs.Description, CauseTypeID = cs.CauseTypeID, CauseParams = cs.CauseParams, Index=cs.Index, WorkspaceId = cs.WorkspaceId };
        }

        public static CauseParam AddId(this CauseParam cp)
        {
            var id = GuidUtils.guidFromObjs(new object[2] { cp.CauseTypeParamId, cp.CauseId });
            return new CauseParam() { CauseParamId = id, CauseId = cp.CauseId, CauseTypeParamId = cp.CauseTypeParamId, Value = cp.Value };
        }

        public static RandGen AddId(this RandGen rg)
        {
            var id = GuidUtils.guidFromObjs(new object[2] { rg.RndGenType, rg.Seed });
            return new RandGen() { RandGenId = rg.RandGenId, StructId = id, RndGenType = rg.RndGenType, Seed = rg.Seed, Description = rg.Description, CauseId = rg.CauseId };
        }

        public static Sorter AddId(this Sorter sorter)
        {
            var id = GuidUtils.guidFromObjs(new object[1] { sorter.Data });
            return new Sorter() { SorterId = sorter.SorterId, StructId = id, CauseId = sorter.CauseId, Description = sorter.Description, Order = sorter.Order, Data = sorter.Data };
        }


        public static void makeSortableSetGen()
        {
           // return null;
        }
    }
}
