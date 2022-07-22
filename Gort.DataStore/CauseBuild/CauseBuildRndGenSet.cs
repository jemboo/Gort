using Gort.DataStore.DataModel;

namespace Gort.DataStore.CauseBuild
{
    public class CauseBuildRndGenSet : CauseBuildBase
    {
        public static string Genus = "RndGen";
        public static string Species = "One";
        public static string ParamSeedName = "Seed";
        public static string ParamRngTypeName = "Type";

        public CauseBuildRndGenSet(Param paramSeed, Param paramRngType, 
                                Workspace workspace, int causeIndex) 
            : base(workspace, causeIndex, Genus, Species)
        {
            RngSeed = AddParam(paramSeed);
            RngType = AddParam(paramRngType);
            CauseParamSeed = MakeCauseParam(RngSeed, ParamSeedName);
            CauseParamRngType = MakeCauseParam(RngType, ParamRngTypeName);
            CauseComments = "CauseComments";
        }

        public Param RngSeed { get; private set; }
        public Param RngType { get; private set; }
        public CauseParam CauseParamSeed { get; private set; }
        public CauseParam CauseParamRngType { get; private set; }

    }
}
