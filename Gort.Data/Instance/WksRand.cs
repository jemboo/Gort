using Gort.Data.DataModel;
using Gort.Data.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gort.Data.Instance
{

    public class WksRand : GortInstBase
    {
        public WksRand() :  base("WorkspaceRand")
        {
            RngSeedA = MakeParam(ParamTypes.RngSeed, 123);
            RngTypeLcg = MakeParam(ParamTypes.RngType, RndGenType.Lcg);
            CauseMakeRand = MakeCause(1, "Make one Rng", CauseTypes.Rng);
            CauseParamSeed = MakeCauseParam(CauseParamTypes.Rng_RngSeed, CauseMakeRand, RngSeedA);
            CauseParamRngType = MakeCauseParam(CauseParamTypes.Rng_RngType, CauseMakeRand, RngTypeLcg);

        }
        public static Cause CauseMakeRand { get; private set; }
        public static Param RngSeedA { get; private set; }
        public static Param RngTypeLcg { get; private set; }
        public static CauseParam CauseParamSeed { get; private set; }
        public static CauseParam CauseParamRngType { get; private set; }

    }
}
