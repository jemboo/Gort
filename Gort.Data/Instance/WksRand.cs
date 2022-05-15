using Gort.Data.DataModel;

namespace Gort.Data.Instance
{

    public class WksRand : GortInstBase
    {
        public WksRand() :  base("WorkspaceRand")
        {
            CauseMakeRand = MakeCause(1, "Make one Rng", CauseTypes.Rng);
            RngSeedA = MakeParam(ParamTypes.RngSeed, 123);
            RngTypeLcg = MakeParam(ParamTypes.RngType, RndGenType.Lcg);
            CauseParamSeed = MakeCauseParam(CauseParamTypes.Rng_RngSeed, CauseMakeRand, RngSeedA);
            CauseParamRngType = MakeCauseParam(CauseParamTypes.Rng_RngType, CauseMakeRand, RngTypeLcg);

            CauseMakeRandSet = MakeCause(2, "Make 3 Rng", CauseTypes.RngSet);
            RngId = MakeParam(ParamTypes.RngId, Guid.Parse("4ca15023-e1da-d718-2e02-1384b6e86b0b"));
            RngCount = MakeParam(ParamTypes.RngCount, 3);
            CauseParamRngId = MakeCauseParam(CauseParamTypes.RngSet_RndGenId, CauseMakeRandSet, RngId);
            CauseParamRngCount = MakeCauseParam(CauseParamTypes.RngSet_RndGenCount, CauseMakeRandSet, RngCount);
        }

        public static Cause CauseMakeRand { get; private set; }
        public static CauseParam CauseParamSeed { get; private set; }
        public static Param RngSeedA { get; private set; }
        public static Param RngTypeLcg { get; private set; }
        public static CauseParam CauseParamRngType { get; private set; }


        public static Cause CauseMakeRandSet { get; private set; }
        public static Param RngId { get; private set; }
        public static Param RngCount { get; private set; }
        public static CauseParam CauseParamRngId { get; private set; }
        public static CauseParam CauseParamRngCount { get; private set; }

    }
}
