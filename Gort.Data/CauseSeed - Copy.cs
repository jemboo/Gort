using System.Security.Cryptography;
using System.Text;

namespace Gort.Data
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
        public static CauseType? causeTypeSorterSetRndBySymetricStage;
        public static CauseType? causeTypeSorterSetRndBySymetricStageBuddies;

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






        public static void AddCauseTypeParams(GortContext gortContext)
        {
            causeTypeParamRndGen_Seed = new CauseTypeParam() { Name = "Seed", CauseType = causeTypeRandGen, DataType = DataType.Int }.AddId();
            causeTypeParamRndGen_Type = new CauseTypeParam() { Name = "Type", CauseType = causeTypeRandGen, DataType = DataType.Int }.AddId();

            causeTypeParamRndGenSet_Seed = new CauseTypeParam() { Name = "Seed", CauseType = causeTypeRandGenSet, DataType = DataType.Int }.AddId();
            causeTypeParamRndGenSet_Type = new CauseTypeParam() { Name = "Type", CauseType = causeTypeRandGenSet, DataType = DataType.Int }.AddId();
            causeTypeParamRndGenSet_Count = new CauseTypeParam() { Name = "Count", CauseType = causeTypeRandGenSet, DataType = DataType.Int }.AddId();

            causeTypeParamSortable_Order = new CauseTypeParam() { Name = "Order", CauseType = causeTypeSortable, DataType = DataType.Int }.AddId();
            causeTypeParamSortable_Format = new CauseTypeParam() { Name = "Format", CauseType = causeTypeSortable, DataType = DataType.Int }.AddId();
            causeTypeParamSortable_Data = new CauseTypeParam() { Name = "Data", CauseType = causeTypeSortable, DataType = DataType.ByteArray }.AddId();

            causeTypeParamSortableImport_Workspace = new CauseTypeParam() { Name = "Workspace", CauseType = causeTypeSortableImport, DataType = DataType.Guid }.AddId();
            causeTypeParamSortableImport_Table = new CauseTypeParam() { Name = "Table", CauseType = causeTypeSortableImport, DataType = DataType.Guid }.AddId();
            causeTypeParamSortableImport_Record = new CauseTypeParam() { Name = "Record", CauseType = causeTypeSortableImport, DataType = DataType.Guid }.AddId();
            causeTypeParamSortableImport_Path = new CauseTypeParam() { Name = "Path", CauseType = causeTypeSortableImport, DataType = DataType.String }.AddId();

            causeTypeParamSortableSet_Order = new CauseTypeParam() { Name = "Order", CauseType = causeTypeSortableSet, DataType = DataType.Int }.AddId();
            causeTypeParamSortableSet_Count = new CauseTypeParam() { Name = "Count", CauseType = causeTypeSortableSet, DataType = DataType.Int }.AddId();
            causeTypeParamSortableSet_Format = new CauseTypeParam() { Name = "Format", CauseType = causeTypeSortableSet, DataType = DataType.Int }.AddId();
            causeTypeParamSortableSet_Data = new CauseTypeParam() { Name = "Data", CauseType = causeTypeSortableSet, DataType = DataType.ByteArray }.AddId();

            causeTypeParamSortableSetImport_Workspace = new CauseTypeParam() { Name = "Workspace", CauseType = causeTypeSortableSetImport, DataType = DataType.Guid }.AddId();
            causeTypeParamSortableSetImport_Table = new CauseTypeParam() { Name = "Table", CauseType = causeTypeSortableSetImport, DataType = DataType.Guid }.AddId();
            causeTypeParamSortableSetImport_Record = new CauseTypeParam() { Name = "Record", CauseType = causeTypeSortableSetImport, DataType = DataType.Guid }.AddId();

            causeTypeParamSortableSetAllForOrder_Order = new CauseTypeParam() { Name = "Order", CauseType = causeTypeSortableSetAllForOrder, DataType = DataType.Int }.AddId();

            causeTypeParamSortableSetOrbit_Perm = new CauseTypeParam() { Name = "Perm", CauseType = causeTypeSortableSetOrbit, DataType = DataType.IntArray }.AddId();
            causeTypeParamSortableSetOrbit_MaxLen = new CauseTypeParam() { Name = "MaxLen", CauseType = causeTypeSortableSetOrbit, DataType = DataType.Int }.AddId();

            causeTypeParamSortableSetStacked_Orders = new CauseTypeParam() { Name = "Orders", CauseType = causeTypeSortableSetStacked, DataType = DataType.IntArray }.AddId();

            causeTypeParamSorter_Order = new CauseTypeParam() { Name = "Order", CauseType = causeTypeSorter, DataType = DataType.Int }.AddId();
            causeTypeParamSorter_Data = new CauseTypeParam() { Name = "Data", CauseType = causeTypeSorter, DataType = DataType.ByteArray }.AddId();

            causeTypeParamSorterImport_Workspace = new CauseTypeParam() { Name = "Workspace", CauseType = causeTypeSorterImport, DataType = DataType.Guid }.AddId();
            causeTypeParamSorterImport_Table = new CauseTypeParam() { Name = "Table", CauseType = causeTypeSorterImport, DataType = DataType.Guid }.AddId();
            causeTypeParamSorterImport_Record = new CauseTypeParam() { Name = "Record", CauseType = causeTypeSorterImport, DataType = DataType.Guid }.AddId();
            causeTypeParamSorterImport_Path = new CauseTypeParam() { Name = "Path", CauseType = causeTypeSorterImport, DataType = DataType.String }.AddId();

            causeTypeParamSorterSetRef_Source = new CauseTypeParam() { Name = "Source", CauseType = causeTypeSorterSetRef, DataType = DataType.String }.AddId();

            causeTypeParamSorterSetImport_Workspace = new CauseTypeParam() { Name = "Workspace", CauseType = causeTypeSorterSetImport, DataType = DataType.Guid }.AddId();
            causeTypeParamSorterSetImport_Table = new CauseTypeParam() { Name = "Table", CauseType = causeTypeSorterSetImport, DataType = DataType.Guid }.AddId();
            causeTypeParamSorterSetImport_Record = new CauseTypeParam() { Name = "Record", CauseType = causeTypeSorterSetImport, DataType = DataType.Guid }.AddId();
            causeTypeParamSorterSetImport_Path = new CauseTypeParam() { Name = "Path", CauseType = causeTypeSorterSetImport, DataType = DataType.String }.AddId();

            causeTypeParamSorterSetRndBySwitch_Order = new CauseTypeParam() { Name = "Order", CauseType = causeTypeSorterSetRndBySwitch, DataType = DataType.Int }.AddId();
            causeTypeParamSorterSetRndBySwitch_SwitchCount = new CauseTypeParam() { Name = "SwitchCount", CauseType = causeTypeSorterSetRndBySwitch, DataType = DataType.Int }.AddId();
            causeTypeParamSorterSetRndBySwitch_RndGen = new CauseTypeParam() { Name = "RndGen", CauseType = causeTypeSorterSetRndBySwitch, DataType = DataType.Guid }.AddId();
            causeTypeParamSorterSetRndBySwitch_SorterCount = new CauseTypeParam() { Name = "SorterCount", CauseType = causeTypeSorterSetRndBySwitch, DataType = DataType.Guid }.AddId();

            causeTypeParamSorterSetRndByStage_Order = new CauseTypeParam() { Name = "Order", CauseType = causeTypeSorterSetRndByStage, DataType = DataType.Int }.AddId();
            causeTypeParamSorterSetRndByStage_RndGen = new CauseTypeParam() { Name = "RndGen", CauseType = causeTypeSorterSetRndByStage, DataType = DataType.Guid }.AddId();
            causeTypeParamSorterSetRndByStage_StageCount = new CauseTypeParam() { Name = "StageCount", CauseType = causeTypeSorterSetRndByStage, DataType = DataType.Int }.AddId();
            causeTypeParamSorterSetRndByStage_SorterCount = new CauseTypeParam() { Name = "SorterCount", CauseType = causeTypeSorterSetRndByStage, DataType = DataType.Int }.AddId();

            causeTypeParamSorterSetRndBySymmetricStage_Order = new CauseTypeParam() { Name = "Order", CauseType = causeTypeSorterSetRndBySymetricStage, DataType = DataType.Int }.AddId();
            causeTypeParamSorterSetRndBySymmetricStage_StageCount = new CauseTypeParam() { Name = "StageCount", CauseType = causeTypeSorterSetRndBySymetricStage, DataType = DataType.Int }.AddId();
            causeTypeParamSorterSetRndBySymmetricStage_RndGen = new CauseTypeParam() { Name = "RndGen", CauseType = causeTypeSorterSetRndBySymetricStage, DataType = DataType.Guid }.AddId();
            causeTypeParamSorterSetRndBySymmetricStage_SorterCount = new CauseTypeParam() { Name = "SorterCount", CauseType = causeTypeSorterSetRndBySymetricStage, DataType = DataType.Int }.AddId();

            causeTypeParamSorterSetRndBySymmetricStageBuddies_Order = new CauseTypeParam() { Name = "Order", CauseType = causeTypeSorterSetRndBySymetricStageBuddies, DataType = DataType.Int }.AddId();
            causeTypeParamSorterSetRndBySymmetricStageBuddies_StageCount = new CauseTypeParam() { Name = "StageCount", CauseType = causeTypeSorterSetRndBySymetricStageBuddies, DataType = DataType.Int }.AddId();
            causeTypeParamSorterSetRndBySymmetricStageBuddies_RndGen = new CauseTypeParam() { Name = "RndGen", CauseType = causeTypeSorterSetRndBySymetricStageBuddies, DataType = DataType.Guid }.AddId();
            causeTypeParamSorterSetRndBySymmetricStageBuddies_SorterCount = new CauseTypeParam() { Name = "SorterCount", CauseType = causeTypeSorterSetRndBySymetricStageBuddies, DataType = DataType.Int }.AddId();
            causeTypeParamSorterSetRndBySymmetricStageBuddies_BuddyCount = new CauseTypeParam() { Name = "BuddyCount", CauseType = causeTypeSorterSetRndBySymetricStageBuddies, DataType = DataType.Int }.AddId();

            causeTypeParamSorterPerf_Sorter = new CauseTypeParam() { Name = "Sorter", CauseType = causeTypeSorterPerf, DataType = DataType.Guid }.AddId();
            causeTypeParamSorterPerf_SortableSet = new CauseTypeParam() { Name = "SortableSet", CauseType = causeTypeSorterPerf, DataType = DataType.Guid }.AddId();

            causeTypeParamSorterSetPerfBins_SorterSet = new CauseTypeParam() { Name = "SorterSet", CauseType = causeTypeSorterSetPerfBins, DataType = DataType.Guid }.AddId();
            causeTypeParamSorterSetPerfBins_SortableSet = new CauseTypeParam() { Name = "SortableSet", CauseType = causeTypeSorterSetPerfBins, DataType = DataType.Guid }.AddId();
            causeTypeParamSorterSetPerfBins_SorterSaveMode = new CauseTypeParam() { Name = "SorterSaveMode", CauseType = causeTypeSorterSetPerfBins, DataType = DataType.Int }.AddId();


            gortContext.CauseTypeParams.Add(causeTypeParamRndGen_Seed);
            gortContext.CauseTypeParams.Add(causeTypeParamRndGen_Type);

            gortContext.CauseTypeParams.Add(causeTypeParamRndGenSet_Seed);
            gortContext.CauseTypeParams.Add(causeTypeParamRndGenSet_Type);
            gortContext.CauseTypeParams.Add(causeTypeParamRndGenSet_Count);

            gortContext.CauseTypeParams.Add(causeTypeParamSortable_Order);
            gortContext.CauseTypeParams.Add(causeTypeParamSortable_Format);
            gortContext.CauseTypeParams.Add(causeTypeParamSortable_Data);

            gortContext.CauseTypeParams.Add(causeTypeParamSortableImport_Workspace);
            gortContext.CauseTypeParams.Add(causeTypeParamSortableImport_Table);
            gortContext.CauseTypeParams.Add(causeTypeParamSortableImport_Record);
            gortContext.CauseTypeParams.Add(causeTypeParamSortableImport_Path);

            gortContext.CauseTypeParams.Add(causeTypeParamSortableSet_Order);
            gortContext.CauseTypeParams.Add(causeTypeParamSortableSet_Count);
            gortContext.CauseTypeParams.Add(causeTypeParamSortableSet_Format);
            gortContext.CauseTypeParams.Add(causeTypeParamSortableSet_Data);

            gortContext.CauseTypeParams.Add(causeTypeParamSortableSetImport_Workspace);
            gortContext.CauseTypeParams.Add(causeTypeParamSortableSetImport_Table);
            gortContext.CauseTypeParams.Add(causeTypeParamSortableSetImport_Record);

            gortContext.CauseTypeParams.Add(causeTypeParamSortableSetAllForOrder_Order);

            gortContext.CauseTypeParams.Add(causeTypeParamSortableSetOrbit_Perm);
            gortContext.CauseTypeParams.Add(causeTypeParamSortableSetOrbit_MaxLen);

            gortContext.CauseTypeParams.Add(causeTypeParamSortableSetStacked_Orders);

            gortContext.CauseTypeParams.Add(causeTypeParamSorter_Order);
            gortContext.CauseTypeParams.Add(causeTypeParamSorter_Data);

            gortContext.CauseTypeParams.Add(causeTypeParamSorterImport_Workspace);
            gortContext.CauseTypeParams.Add(causeTypeParamSorterImport_Table);
            gortContext.CauseTypeParams.Add(causeTypeParamSorterImport_Record);
            gortContext.CauseTypeParams.Add(causeTypeParamSorterImport_Path);

            gortContext.CauseTypeParams.Add(causeTypeParamSorterSetRef_Source);

            gortContext.CauseTypeParams.Add(causeTypeParamSorterSetImport_Workspace);
            gortContext.CauseTypeParams.Add(causeTypeParamSorterSetImport_Table);
            gortContext.CauseTypeParams.Add(causeTypeParamSorterSetImport_Record);
            gortContext.CauseTypeParams.Add(causeTypeParamSorterSetImport_Path);

            gortContext.CauseTypeParams.Add(causeTypeParamSorterSetRndBySwitch_Order);
            gortContext.CauseTypeParams.Add(causeTypeParamSorterSetRndBySwitch_RndGen);
            gortContext.CauseTypeParams.Add(causeTypeParamSorterSetRndBySwitch_SwitchCount);
            gortContext.CauseTypeParams.Add(causeTypeParamSorterSetRndBySwitch_SorterCount);

            gortContext.CauseTypeParams.Add(causeTypeParamSorterSetRndByStage_Order);
            gortContext.CauseTypeParams.Add(causeTypeParamSorterSetRndByStage_RndGen);
            gortContext.CauseTypeParams.Add(causeTypeParamSorterSetRndByStage_StageCount);
            gortContext.CauseTypeParams.Add(causeTypeParamSorterSetRndByStage_SorterCount);

            gortContext.CauseTypeParams.Add(causeTypeParamSorterSetRndBySymmetricStage_Order);
            gortContext.CauseTypeParams.Add(causeTypeParamSorterSetRndBySymmetricStage_StageCount);
            gortContext.CauseTypeParams.Add(causeTypeParamSorterSetRndBySymmetricStage_RndGen);
            gortContext.CauseTypeParams.Add(causeTypeParamSorterSetRndBySymmetricStage_SorterCount);

            gortContext.CauseTypeParams.Add(causeTypeParamSorterSetRndBySymmetricStageBuddies_Order);
            gortContext.CauseTypeParams.Add(causeTypeParamSorterSetRndBySymmetricStageBuddies_StageCount);
            gortContext.CauseTypeParams.Add(causeTypeParamSorterSetRndBySymmetricStageBuddies_RndGen);
            gortContext.CauseTypeParams.Add(causeTypeParamSorterSetRndBySymmetricStageBuddies_SorterCount);
            gortContext.CauseTypeParams.Add(causeTypeParamSorterSetRndBySymmetricStageBuddies_BuddyCount);

            gortContext.CauseTypeParams.Add(causeTypeParamSorterPerf_Sorter);
            gortContext.CauseTypeParams.Add(causeTypeParamSorterPerf_SortableSet);

            gortContext.CauseTypeParams.Add(causeTypeParamSorterSetPerfBins_SorterSet);
            gortContext.CauseTypeParams.Add(causeTypeParamSorterSetPerfBins_SortableSet);
            gortContext.CauseTypeParams.Add(causeTypeParamSorterSetPerfBins_SorterSaveMode);

        }





    }
}
