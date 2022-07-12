using Gort.Data.DataModel;
using Gort.Data.Instance.StandardTypes;

namespace Gort.Data.Instance.CauseBuilder
{

    public class CbRandSet : CauseBuilderBase
    {
        public CbRandSet(
            string workspaceName, int causeIndex,
            string descr, Guid paramRngId, int rngCount) : base(workspaceName, causeIndex)
        {
            RngId = paramRngId;
            RngCount = rngCount;

            CauseDescription = $"RndGenSet({descr}, {RngCount})";

            CauseMakeRandSet = MakeCause(CauseTypes.Rng);
            PramRngId = MakeParam(ParamTypes.RecordId, RngId);
            PramRngCount = MakeParam(ParamTypes.Order, RngCount);
            CauseParamRngId = MakeCauseParam(CauseParamTypes.RngSet_RndGenId, CauseMakeRandSet, PramRngId);
            CauseParamRngCount = MakeCauseParam(CauseParamTypes.RngSet_RndGenCount, CauseMakeRandSet, PramRngCount);
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
