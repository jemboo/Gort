using Gort.Data.DataModel;
using Gort.Data.Instance.StandardTypes;
using Gort.Data.Utils;

namespace Gort.Data.Instance.CauseBuilder.SortableSet
{
    public class CbSortableSetAllForOrder : CauseBuilderBase
    {
        public CbSortableSetAllForOrder(
            string workspaceName,
            int causeIndex,
            string descr,
            Param paramOrder,
            Param paramSortableFormat) : base(workspaceName, causeIndex)
        {
            Cause = MakeCause(CauseTypes.SortableSetAllForOrder);
            Pram_Order = AddParam(paramOrder);
            Pram_SortableFormat = AddParam(paramSortableFormat);

            Order = Pram_Order.IntValue();
            SortableFormat = Pram_SortableFormat.SortableFormatValue();

            CauseDescription = $"SortableSetAllForOrder({descr},{Order},{SortableFormat})";

            CauseParam_Order =
                MakeCauseParam(
                    CauseParamTypes.SortableSetRand_Order,
                    Cause,
                    Pram_Order);

            CauseParam_SortableFormat =
                MakeCauseParam(
                    CauseParamTypes.SortableSetRand_SortableFormat,
                    Cause,
                    Pram_SortableFormat);
        }

        public int Order { get; }
        public SortableFormat SortableFormat { get; }

        public Param Pram_SortableFormat { get; private set; }
        public Param Pram_Order { get; private set; }

        public CauseParam CauseParam_Order { get; private set; }
        public CauseParam CauseParam_SortableFormat { get; private set; }

    }
}
