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
        /// ************ CauseParamTypes *****************
        /// **********************************************
        public static CauseParamType? cptRndGen_Seed;
        public static CauseParamType? cptRndGen_Type;

        public static CauseParamType? cptRndGenSet_Seed;
        public static CauseParamType? cptRndGenSet_Type;
        public static CauseParamType? cptRndGenSet_Count;

        public static CauseParamType? cptSortable_Order;
        public static CauseParamType? cptSortable_Format;
        public static CauseParamType? cptSortable_Data;

        public static CauseParamType? cptSortableImport_Workspace;
        public static CauseParamType? cptSortableImport_Table;
        public static CauseParamType? cptSortableImport_Record;
        public static CauseParamType? cptSortableImport_Path;

        public static CauseParamType? cptSortableSet_Order;
        public static CauseParamType? cptSortableSet_Count;
        public static CauseParamType? cptSortableSet_Format;
        public static CauseParamType? cptSortableSet_Data;

        public static CauseParamType? cptSortableSetImport_Workspace;
        public static CauseParamType? cptSortableSetImport_Table;
        public static CauseParamType? cptSortableSetImport_Record;

        public static CauseParamType? cptSortableSetAllForOrder_Order;

        public static CauseParamType? cptSortableSetOrbit_Perm;
        public static CauseParamType? cptSortableSetOrbit_MaxLen;

        public static CauseParamType? cptSortableSetStacked_Orders;

        public static CauseParamType? cptSorter_Order;
        public static CauseParamType? cptSorter_Data;

        public static CauseParamType? cptSorterImport_Workspace;
        public static CauseParamType? cptSorterImport_Table;
        public static CauseParamType? cptSorterImport_Record;
        public static CauseParamType? cptSorterImport_Path;

        public static CauseParamType? cptSorterSetRef_Source;

        public static CauseParamType? cptSorterSetImport_Workspace;
        public static CauseParamType? cptSorterSetImport_Table;
        public static CauseParamType? cptSorterSetImport_Record;
        public static CauseParamType? cptSorterSetImport_Path;

        public static CauseParamType? cptSorterSetRndBySwitch_Order;
        public static CauseParamType? cptSorterSetRndBySwitch_RndGen;
        public static CauseParamType? cptSorterSetRndBySwitch_SwitchCount;
        public static CauseParamType? cptSorterSetRndBySwitch_SorterCount;

        public static CauseParamType? cptSorterSetRndByStage_Order;
        public static CauseParamType? cptSorterSetRndByStage_StageCount;
        public static CauseParamType? cptSorterSetRndByStage_RndGen;
        public static CauseParamType? cptSorterSetRndByStage_SorterCount;

        public static CauseParamType? cptSorterSetRndBySymmetricStage_Order;
        public static CauseParamType? cptSorterSetRndBySymmetricStage_StageCount;
        public static CauseParamType? cptSorterSetRndBySymmetricStage_RndGen;
        public static CauseParamType? cptSorterSetRndBySymmetricStage_SorterCount;

        public static CauseParamType? cptSorterSetRndBySymmetricStageBuddies_Order;
        public static CauseParamType? cptSorterSetRndBySymmetricStageBuddies_StageCount;
        public static CauseParamType? cptSorterSetRndBySymmetricStageBuddies_RndGen;
        public static CauseParamType? cptSorterSetRndBySymmetricStageBuddies_SorterCount;
        public static CauseParamType? cptSorterSetRndBySymmetricStageBuddies_BuddyCount;

        public static CauseParamType? cptSorterPerf_Sorter;
        public static CauseParamType? cptSorterPerf_SortableSet;

        public static CauseParamType? cptSorterSetPerfBins_SorterSet;
        public static CauseParamType? cptSorterSetPerfBins_SortableSet;
        public static CauseParamType? cptSorterSetPerfBins_SorterSaveMode;

    }
}
