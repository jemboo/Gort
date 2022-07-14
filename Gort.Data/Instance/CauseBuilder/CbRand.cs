using Gort.Data.DataModel;
using Gort.Data.Instance.StandardTypes;
using Gort.Data.Utils;

namespace Gort.Data.Instance.CauseBuilder
{

    public class CbRand : CauseBuilderBase
    {
        public CbRand(
            string workspaceName, int causeIndex,
            string descr, Param paramSeed, Param paramRngType)
                : base(workspaceName, causeIndex)
        {
            RngSeed = AddParam(paramSeed);
            RngTypeLcg = AddParam(paramRngType);
            Seed = paramSeed.IntValue();
            RandGenType = paramRngType.RandGenTypeValue();
            CauseDescription = $"{descr}({RandGenType},{Seed})";
            CauseMakeRand = MakeCause(CauseTypes.Rng);
            CauseParamSeed = MakeCauseParam(CauseParamTypes.Rng_RngSeed, 
                CauseMakeRand, RngSeed);
            CauseParamRngType = MakeCauseParam(CauseParamTypes.Rng_RngType, 
                CauseMakeRand, RngTypeLcg);
        }

        public int Seed { get; }
        public RandGenType RandGenType { get; }
        public Cause CauseMakeRand { get; private set; }
        public Param RngSeed { get; private set; }
        public Param RngTypeLcg { get; private set; }
        public CauseParam CauseParamSeed { get; private set; }
        public CauseParam CauseParamRngType { get; private set; }

    }

    public static class CbRandExt
    {
        public static Param GetParamRngId(this CbRand cbRand, IGortContext ctxt)
        {
            var rngRecord = ctxt.RandGen.SingleOrDefault(rg => rg.CauseId == cbRand.CauseMakeRand.CauseId);
            return ParamTypes.RngId.MakeParam(rngRecord.RandGenId);
        }
    }


}
