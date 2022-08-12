using Gort.DataStore.DataModel;

namespace Gort.DataStore.CauseBuild
{
    public class CauseBuildRndSortableSet : CauseBuildBase
    {
        public static string Genus = "RndGen";
        public static string Species = "One";
        public static string ParamSeedName = "Seed";
        public static string ParamRngTypeName = "Type";

        public CauseBuildRndSortableSet(Param paramSeed, Param paramRngType, 
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
        public CauseParamR CauseParamSeed { get; private set; }
        public CauseParamR CauseParamRngType { get; private set; }

    }
}
