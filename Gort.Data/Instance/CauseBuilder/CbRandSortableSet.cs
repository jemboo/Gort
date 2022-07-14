using Gort.Data.DataModel;
using Gort.Data.Instance.StandardTypes;
using Gort.Data.Utils;

namespace Gort.Data.Instance.CauseBuilder
{

    public class CbRandSortableSet : CauseBuilderBase
    {
        public CbRandSortableSet(
            string workspaceName, 
            int causeIndex,
            string descr,
            Param paramOrder, 
            Param paramSortableCount,
            Param paramSortableFormat, 
            Param paramRngId) : base(workspaceName, causeIndex)
        {
            PramOrder = AddParam(paramOrder);
            PramRngId = AddParam(paramRngId);
            PramSortableCount = AddParam(paramSortableCount);
            PramSortableFormat = AddParam(paramSortableFormat);

            Order = PramOrder.IntValue();
            SortableCount = PramSortableCount.IntValue();
            RngId = PramRngId.GuidValue();
            SortableFormat = PramSortableFormat.SortableFormatValue();

            CauseDescription = $"SortableSetRand({descr},{Order},{SortableCount})";

            CauseMakeSortableSet = MakeCause(CauseTypes.SortableSetRand);
            CauseParam_MakeSortableSet_RngId = 
                MakeCauseParam(
                    CauseParamTypes.SortableSetRand_RngId, 
                    CauseMakeSortableSet, 
                    PramRngId);
            CauseParam_MakeSortableSet_Order = 
                MakeCauseParam(
                    CauseParamTypes.SortableSetRand_Order, 
                    CauseMakeSortableSet, 
                    PramOrder);
            CauseParam_MakeSortableSet_SortableCount = 
                MakeCauseParam(
                    CauseParamTypes.SortableSetRand_SortableCount, 
                    CauseMakeSortableSet, 
                    PramSortableCount);
            CauseParam_MakeSortableSet_SortableFormat = 
                MakeCauseParam(
                    CauseParamTypes.SortableSetRand_SortableFormat, 
                    CauseMakeSortableSet, 
                    PramSortableFormat);
        }

        public int Order { get; }
        public int SortableCount { get; }
        public Guid RngId { get; }
        public SortableFormat SortableFormat { get; }

        public Cause CauseMakeSortableSet { get; private set; }
        public Param PramRngId { get; private set; }
        public Param PramOrder { get; private set; }
        public Param PramSortableCount { get; private set; }
        public Param PramSortableFormat { get; private set; }

        public CauseParam CauseParam_MakeSortableSet_RngId { get; private set; }
        public CauseParam CauseParam_MakeSortableSet_Order { get; private set; }
        public CauseParam CauseParam_MakeSortableSet_SortableCount { get; private set; }
        public CauseParam CauseParam_MakeSortableSet_SortableFormat { get; private set; }

    }
}
