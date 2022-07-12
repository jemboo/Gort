using Gort.Data.DataModel;
using Gort.Data.Instance;
using Gort.Data.Instance.CauseBuilder;
using Gort.Data.Instance.StandardTypes;

namespace Gort.Data.Load
{
    internal static class InstanceLoader
    {

        public static void LoadWorkSpaceRnd(IGortContext ctxt,
            string workspaceName, int causeIndex, 
            Param paramSeed, Param paramRndGenType)
        {
            string descr = $"RndGen_{causeIndex}";
            WorkspaceLoad.LoadCauseBuilder(
                new CbRand(workspaceName, causeIndex, descr,
                paramRndGenType, paramSeed), ctxt);
        }

        //public static void LoadWorkSpaceRndSortableSet(IGortContext ctxt, int causeIndex, 
        //    Param paramRndGenId, Param paramOrder, Param paramSortableCount, Param paramSortableFormat)
        //{
        //    string descr = $"RndSortableSet{causeIndex}";
        //    int order = 16;
        //    int sortableCount = 20;
        //    SortableFormat sortableFormat = SortableFormat.b64;
        //    WorkspaceLoad.LoadCauseBuilder(new CbRandSortableSet(
        //        WorkspaceName, causeIndex, descr, paramOrder,
        //        paramSortableCount, paramSortableFormat, paramRndGenId), ctxt);
        //}
    }
}
