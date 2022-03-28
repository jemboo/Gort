using System.Security.Cryptography;
using System.Text;

namespace Gort.Data.Seed
{
    internal static partial class CauseSeed
    {

        /// **********************************************
        /// ************ ParamTypes *****************
        /// **********************************************



        /// *****************************************
        /// ************ workSpace ******************
        /// *****************************************
        public static Workspace? workspace1;


        /// *****************************************
        /// ************** Cause ********************
        /// *****************************************
        public static Cause? causeRndGenA;
        public static Cause? causeRndGenSetA;
        public static Cause? causeSortableSetAllForOrderA;


        /// *****************************************
        /// ************ CauseParam ******************
        /// *****************************************
        public static CauseParam? cpRndGen_Seed;
        public static CauseParam? cpRndGen_Type;

        public static CauseParam? cpRndGenSet_Rng;
        public static CauseParam? cpRndGenSet_Count;

        public static CauseParam? cpSortable_Order;
        public static CauseParam? cpSortable_Format;
        public static CauseParam? cpSortable_Data;

        public static CauseParam? cpSortableImport_Workspace;
        public static CauseParam? cpSortableImport_Table;
        public static CauseParam? cpSortableImport_Record;
        public static CauseParam? cpSortableImport_Path;

        public static CauseParam? cpSortableSet_Order;
        public static CauseParam? cpSortableSet_Count;
        public static CauseParam? cpSortableSet_Format;
        public static CauseParam? cpSortableSet_Data;

        public static CauseParam? cpSortableSetImport_Workspace;
        public static CauseParam? cpSortableSetImport_Table;
        public static CauseParam? cpSortableSetImport_Record;

        public static CauseParam? cpSortableSetAllForOrder_Order;

        public static CauseParam? cpSortableSetOrbit_Perm;
        public static CauseParam? cpSortableSetOrbit_MaxLen;

        public static CauseParam? cpSortableSetStacked_Orders;

        public static CauseParam? cpSorter_Order;
        public static CauseParam? cpSorter_Data;

        public static CauseParam? cpSorterImport_Workspace;
        public static CauseParam? cpSorterImport_Table;
        public static CauseParam? cpSorterImport_Record;
        public static CauseParam? cpSorterImport_Path;

        public static CauseParam? cpSorterSetRef_Source;

        public static CauseParam? cpSorterSetImport_Workspace;
        public static CauseParam? cpSorterSetImport_Table;
        public static CauseParam? cpSorterSetImport_Record;
        public static CauseParam? cpSorterSetImport_Path;

        public static CauseParam? cpSorterSetRndBySwitch_Order;
        public static CauseParam? cpSorterSetRndBySwitch_RndGen;
        public static CauseParam? cpSorterSetRndBySwitch_SwitchCount;
        public static CauseParam? cpSorterSetRndBySwitch_SorterCount;

        public static CauseParam? cpSorterSetRndByStage_Order;
        public static CauseParam? cpSorterSetRndByStage_StageCount;
        public static CauseParam? cpSorterSetRndByStage_RndGen;
        public static CauseParam? cpSorterSetRndByStage_SorterCount;

        public static CauseParam? cpSorterSetRndBySymmetricStage_Order;
        public static CauseParam? cpSorterSetRndBySymmetricStage_StageCount;
        public static CauseParam? cpSorterSetRndBySymmetricStage_RndGen;
        public static CauseParam? cpSorterSetRndBySymmetricStage_SorterCount;

        public static CauseParam? cpSorterSetRndBySymmetricStageBuddies_Order;
        public static CauseParam? cpSorterSetRndBySymmetricStageBuddies_StageCount;
        public static CauseParam? cpSorterSetRndBySymmetricStageBuddies_RndGen;
        public static CauseParam? cpSorterSetRndBySymmetricStageBuddies_SorterCount;
        public static CauseParam? cpSorterSetRndBySymmetricStageBuddies_BuddyCount;

        public static CauseParam? cpSorterPerf_Sorter;
        public static CauseParam? cpSorterPerf_SortableSet;

        public static CauseParam? cpSorterSetPerfBins_SorterSet;
        public static CauseParam? cpSorterSetPerfBins_SortableSet;
        public static CauseParam? cpSorterSetPerfBins_SorterSaveMode;

    }
}
