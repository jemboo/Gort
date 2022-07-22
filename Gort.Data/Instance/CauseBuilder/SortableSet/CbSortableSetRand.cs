using Gort.Data.DataModel;
using Gort.Data.Instance.StandardTypes;
using Gort.Data.Utils;

namespace Gort.Data.Instance.CauseBuilder.SortableSet
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
            Cause = MakeCause(CauseTypes.SortableSetRand);

            PramOrder = AddParam(paramOrder);
            PramRngId = AddParam(paramRngId);
            PramSortableCount = AddParam(paramSortableCount);
            PramSortableFormat = AddParam(paramSortableFormat);

            Order = PramOrder.IntValue();
            SortableCount = PramSortableCount.IntValue();
            RngId = PramRngId.GuidValue();
            SortableFormat = PramSortableFormat.SortableFormatValue();

            CauseDescription = $"SortableSetRand({descr},{Order},{SortableCount})";

            CauseParam_RngId =
                MakeCauseParam(
                    CauseParamTypes.SortableSetRand_RngId,
                    Cause,
                    PramRngId);
            CauseParam_Order =
                MakeCauseParam(
                    CauseParamTypes.SortableSetRand_Order,
                    Cause,
                    PramOrder);
            CauseParam_SortableCount =
                MakeCauseParam(
                    CauseParamTypes.SortableSetRand_SortableCount,
                    Cause,
                    PramSortableCount);
            CauseParam_SortableFormat =
                MakeCauseParam(
                    CauseParamTypes.SortableSetRand_SortableFormat,
                    Cause,
                    PramSortableFormat);
        }

        public int Order { get; }
        public int SortableCount { get; }
        public Guid RngId { get; }
        public SortableFormat SortableFormat { get; }
        public Param PramRngId { get; private set; }
        public Param PramOrder { get; private set; }
        public Param PramSortableCount { get; private set; }
        public Param PramSortableFormat { get; private set; }
        public CauseParam CauseParam_RngId { get; private set; }
        public CauseParam CauseParam_Order { get; private set; }
        public CauseParam CauseParam_SortableCount { get; private set; }
        public CauseParam CauseParam_SortableFormat { get; private set; }

    }
}
