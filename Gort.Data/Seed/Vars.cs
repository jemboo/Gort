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
        public static CauseType? causeTypeRandGen;
        public static CauseType? causeTypeRandGenSet;

        public static CauseType? causeTypeSortable;
        public static CauseType? causeTypeSortableImport;

        public static CauseType? causeTypeSortableSet;
        public static CauseType? causeTypeSortableSetImport;
        public static CauseType? causeTypeSortableSetAllForOrder;
        public static CauseType? causeTypeSortableSetOrbit;
        public static CauseType? causeTypeSortableSetStacked;

        public static CauseType? causeTypeSorter;
        public static CauseType? causeTypeSorterImport;

        public static CauseType? causeTypeSorterSetRef;
        public static CauseType? causeTypeSorterSetImport;
        public static CauseType? causeTypeSorterSetRndBySwitch;
        public static CauseType? causeTypeSorterSetRndByStage;
        public static CauseType? causeTypeSorterSetRndByRflStage;
        public static CauseType? causeTypeSorterSetRndByRflStageBuddies;

        public static CauseType? causeTypeSorterPerf;
        public static CauseType? causeTypeSorterSetPerfBins;



        /// *****************************************
        /// ************ CauseTypeParams *****************
        /// *****************************************
        public static CauseTypeParam? causeTypeParamRndGen_Seed;
        public static CauseTypeParam? causeTypeParamRndGen_Type;

        public static CauseTypeParam? causeTypeParamRndGenSet_Seed;
        public static CauseTypeParam? causeTypeParamRndGenSet_Type;
        public static CauseTypeParam? causeTypeParamRndGenSet_Count;

        public static CauseTypeParam? causeTypeParamSortable_Order;
        public static CauseTypeParam? causeTypeParamSortable_Format;
        public static CauseTypeParam? causeTypeParamSortable_Data;

        public static CauseTypeParam? causeTypeParamSortableImport_Workspace;
        public static CauseTypeParam? causeTypeParamSortableImport_Table;
        public static CauseTypeParam? causeTypeParamSortableImport_Record;
        public static CauseTypeParam? causeTypeParamSortableImport_Path;

        public static CauseTypeParam? causeTypeParamSortableSet_Order;
        public static CauseTypeParam? causeTypeParamSortableSet_Count;
        public static CauseTypeParam? causeTypeParamSortableSet_Format;
        public static CauseTypeParam? causeTypeParamSortableSet_Data;

        public static CauseTypeParam? causeTypeParamSortableSetImport_Workspace;
        public static CauseTypeParam? causeTypeParamSortableSetImport_Table;
        public static CauseTypeParam? causeTypeParamSortableSetImport_Record;

        public static CauseTypeParam? causeTypeParamSortableSetAllForOrder_Order;

        public static CauseTypeParam? causeTypeParamSortableSetOrbit_Perm;
        public static CauseTypeParam? causeTypeParamSortableSetOrbit_MaxLen;

        public static CauseTypeParam? causeTypeParamSortableSetStacked_Orders;

        public static CauseTypeParam? causeTypeParamSorter_Order;
        public static CauseTypeParam? causeTypeParamSorter_Data;

        public static CauseTypeParam? causeTypeParamSorterImport_Workspace;
        public static CauseTypeParam? causeTypeParamSorterImport_Table;
        public static CauseTypeParam? causeTypeParamSorterImport_Record;
        public static CauseTypeParam? causeTypeParamSorterImport_Path;

        public static CauseTypeParam? causeTypeParamSorterSetRef_Source;

        public static CauseTypeParam? causeTypeParamSorterSetImport_Workspace;
        public static CauseTypeParam? causeTypeParamSorterSetImport_Table;
        public static CauseTypeParam? causeTypeParamSorterSetImport_Record;
        public static CauseTypeParam? causeTypeParamSorterSetImport_Path;

        public static CauseTypeParam? causeTypeParamSorterSetRndBySwitch_Order;
        public static CauseTypeParam? causeTypeParamSorterSetRndBySwitch_RndGen;
        public static CauseTypeParam? causeTypeParamSorterSetRndBySwitch_SwitchCount;
        public static CauseTypeParam? causeTypeParamSorterSetRndBySwitch_SorterCount;

        public static CauseTypeParam? causeTypeParamSorterSetRndByStage_Order;
        public static CauseTypeParam? causeTypeParamSorterSetRndByStage_StageCount;
        public static CauseTypeParam? causeTypeParamSorterSetRndByStage_RndGen;
        public static CauseTypeParam? causeTypeParamSorterSetRndByStage_SorterCount;

        public static CauseTypeParam? causeTypeParamSorterSetRndBySymmetricStage_Order;
        public static CauseTypeParam? causeTypeParamSorterSetRndBySymmetricStage_StageCount;
        public static CauseTypeParam? causeTypeParamSorterSetRndBySymmetricStage_RndGen;
        public static CauseTypeParam? causeTypeParamSorterSetRndBySymmetricStage_SorterCount;

        public static CauseTypeParam? causeTypeParamSorterSetRndBySymmetricStageBuddies_Order;
        public static CauseTypeParam? causeTypeParamSorterSetRndBySymmetricStageBuddies_StageCount;
        public static CauseTypeParam? causeTypeParamSorterSetRndBySymmetricStageBuddies_RndGen;
        public static CauseTypeParam? causeTypeParamSorterSetRndBySymmetricStageBuddies_SorterCount;
        public static CauseTypeParam? causeTypeParamSorterSetRndBySymmetricStageBuddies_BuddyCount;

        public static CauseTypeParam? causeTypeParamSorterPerf_Sorter;
        public static CauseTypeParam? causeTypeParamSorterPerf_SortableSet;

        public static CauseTypeParam? causeTypeParamSorterSetPerfBins_SorterSet;
        public static CauseTypeParam? causeTypeParamSorterSetPerfBins_SortableSet;
        public static CauseTypeParam? causeTypeParamSorterSetPerfBins_SorterSaveMode;


        /// *****************************************
        /// ************ workSpace1 *****************
        /// *****************************************
        public static Workspace? workspace1;

        public static Cause? causeRndGenSetA;
        public static CauseParam? causeParamRndGenSet_Seed;
        public static CauseParam? causeParamRndGenSet_Type;
        public static CauseParam? causeParamRndGenSet_Count;


        public static Cause? causeSortableSetAllForOrderA;
        public static CauseParam? causeParamSortableSetAllForOrderA_Order;

    }
}
