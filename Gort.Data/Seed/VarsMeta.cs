using System.Security.Cryptography;
using System.Text;

namespace Gort.Data.Seed
{
    internal static partial class CauseSeed
    {
        /// *****************************************
        /// ************ CauseTypeGroups *****************
        /// *****************************************
        public static CauseTypeGroup? ctgRoot;
        public static CauseTypeGroup? ctgRandGen;
        public static CauseTypeGroup? ctgSortable;
        public static CauseTypeGroup? ctgSortableSet;
        public static CauseTypeGroup? ctgSorter;
        public static CauseTypeGroup? ctgSorterSet;
        public static CauseTypeGroup? ctgSorterSetRand;
        public static CauseTypeGroup? ctgSorterPerf;
        public static CauseTypeGroup? ctgSorterGa;


        /// *****************************************
        /// ************ CauseTypes *****************
        /// *****************************************
        public static CauseType? ctRandGen;
        public static CauseType? ctRandGenSet;

        public static CauseType? ctSortable;
        public static CauseType? ctSortableImport;

        public static CauseType? ctSortableSet;
        public static CauseType? ctSortableSetImport;
        public static CauseType? ctSortableSetAllForOrder;
        public static CauseType? ctSortableSetOrbit;
        public static CauseType? ctSortableSetStacked;

        public static CauseType? ctSorter;
        public static CauseType? ctSorterImport;

        public static CauseType? ctSorterSetRef;
        public static CauseType? ctSorterSetImport;
        public static CauseType? ctSorterSetRndBySwitch;
        public static CauseType? ctSorterSetRndByStage;
        public static CauseType? ctSorterSetRndByRflStage;
        public static CauseType? ctSorterSetRndByRflStageBuddies;

        public static CauseType? ctSorterPerf;
        public static CauseType? ctSorterSetPerfBins;



        /// **********************************************
        /// ************ ParamTypes *****************
        /// **********************************************
        public static ParamType? ptRndGen_Seed;
        public static ParamType? ptRndGen_Type;

        public static ParamType? ptRndGenId;

        public static ParamType? ptRndGenCount;

        public static ParamType? ptOrder;
        public static ParamType? ptSortableFormat;
        public static ParamType? ptSortableData;

        public static ParamType? ptWorkspaceId;
        public static ParamType? ptTableName;
        public static ParamType? ptRecordId;
        public static ParamType? ptRecordPath;

        public static ParamType? ptSortableCount;

        public static ParamType? ptSortableSetOrbit_Perm;
        public static ParamType? ptSortableSetOrbit_MaxCount;

        public static ParamType? ptSortableSetStacked_Orders;

        public static ParamType? ptSorterData;

        public static ParamType? ptSorterSetRef_Source;

        public static ParamType? ptSwitchCount;
        public static ParamType? ptStageCount;
        public static ParamType? ptSorterCount;
        public static ParamType? ptStageBuddyCount;

        public static ParamType? ptSorterId;
        public static ParamType? ptSortableSetId;

        public static ParamType? ptSorterSetId;
        public static ParamType? cptSorterSetPerfBins_SorterSaveMode;

    }
}
