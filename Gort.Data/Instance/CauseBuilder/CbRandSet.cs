using Gort.Data.DataModel;
using Gort.Data.Instance.StandardTypes;
using Gort.Data.Utils;

namespace Gort.Data.Instance.CauseBuilder
{

    public class CbRandSet : CauseBuilderBase
    {
        public CbRandSet(
                    string workspaceName, 
                    int causeIndex,
                    string descr, 
                    Param paramRngId,
                    Param paramRngCount
                    ) : base(workspaceName, causeIndex)
        {

            PramRngId = AddParam(paramRngId);
            PramRngCount = AddParam(paramRngCount);

            RngId = paramRngId.GuidValue();
            RngCount = PramRngCount.IntValue();

            CauseDescription = $"{descr}({RngId},{RngCount})";
            CauseMakeRandSet = MakeCause(CauseTypes.RngSet);
            CauseParamRngId = MakeCauseParam(CauseParamTypes.RngSet_RndGenId, 
                CauseMakeRandSet, PramRngId);
            CauseParamRngCount = MakeCauseParam(CauseParamTypes.RngSet_RndGenCount, 
                CauseMakeRandSet, PramRngCount);
        }

        public Guid RngId { get; }
        public int RngCount { get; }
        public Param PramRngId { get; private set; }
        public Param PramRngCount { get; private set; }
        public Cause CauseMakeRandSet { get; private set; }
        public CauseParam CauseParamRngId { get; private set; }
        public CauseParam CauseParamRngCount { get; private set; }

    }
}
