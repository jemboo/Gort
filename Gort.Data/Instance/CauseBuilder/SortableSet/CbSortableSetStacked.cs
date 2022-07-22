using Gort.Data.DataModel;
using Gort.Data.Instance.StandardTypes;
using Gort.Data.Utils;


namespace Gort.Data.Instance.CauseBuilder.SortableSet
{
    public class CbSortableSetStacked : CauseBuilderBase
    {
        public CbSortableSetStacked(
            string workspaceName,
            int causeIndex,
            string descr,
            Param paramOrderStack,
            Param paramSortableFormat) : base(workspaceName, causeIndex)
        {
            Cause = MakeCause(CauseTypes.SortableSetAllForOrder);
            Pram_OrderStack = AddParam(paramOrderStack);
            Pram_SortableFormat = AddParam(paramSortableFormat);

            OrderStack = new[] { 0, 1 };  //Pram_OrderStack.IntValue();
            SortableFormat = Pram_SortableFormat.SortableFormatValue();

            CauseDescription = $"CbSortableSetStacked({descr},{"OrderStack"},{SortableFormat})";
            CauseParam_OrderStack =
                MakeCauseParam(
                    CauseParamTypes.SortableSetRand_Order,
                    Cause,
                    Pram_OrderStack);

            CauseParam_SortableFormat =
                MakeCauseParam(
                    CauseParamTypes.SortableSetRand_SortableFormat,
                    Cause,
                    Pram_SortableFormat);
        }

        public int[] OrderStack { get; }
        public SortableFormat SortableFormat { get; }

        public Param Pram_SortableFormat { get; private set; }
        public Param Pram_OrderStack { get; private set; }

        public CauseParam CauseParam_OrderStack { get; private set; }
        public CauseParam CauseParam_SortableFormat { get; private set; }
    }
}
