using System.Security.Cryptography;
using System.Text;

namespace Gort.Data.Seed
{
    internal static partial class CauseSeed
    {

        public static void AddCauseTypeGroups(GortContext gortContext)
        {
            ctgRoot = new CauseTypeGroup() { Name = "root", Parent = null }.AddId();
            ctgRandGen = new CauseTypeGroup() { Name = "randGen", Parent = ctgRoot }.AddId();
            ctgSortable = new CauseTypeGroup() { Name = "Sortable", Parent = ctgRoot }.AddId();
            ctgSortableSet = new CauseTypeGroup() { Name = "SortableSet", Parent = ctgRoot }.AddId();
            ctgSorter = new CauseTypeGroup() { Name = "Sorter", Parent = ctgRoot }.AddId();
            ctgSorterSet = new CauseTypeGroup() { Name = "SorterSet", Parent = ctgRoot }.AddId();
            ctgSorterSetRand = new CauseTypeGroup() { Name = "RndSorterSet", Parent = ctgSorterSet }.AddId();
            ctgSorterPerf = new CauseTypeGroup() { Name = "SorterPerf", Parent = ctgRoot }.AddId();
            ctgSorterGa = new CauseTypeGroup() { Name = "SorterGa", Parent = ctgRoot }.AddId();

            gortContext.CauseTypeGroup.Add(ctgRoot);
            gortContext.CauseTypeGroup.Add(ctgRandGen);
            gortContext.CauseTypeGroup.Add(ctgSortable);
            gortContext.CauseTypeGroup.Add(ctgSortableSet);
            gortContext.CauseTypeGroup.Add(ctgSorter);
            gortContext.CauseTypeGroup.Add(ctgSorterSet);
            gortContext.CauseTypeGroup.Add(ctgSorterSetRand);
            gortContext.CauseTypeGroup.Add(ctgSorterPerf);
            gortContext.CauseTypeGroup.Add(ctgSorterGa);
        }




        public static void AddCauseTypes(GortContext gortContext)
        {
            causeTypeRandGen = new CauseType() { Name = "randGen", CauseTypeGroup = ctgRandGen }.AddId();
            causeTypeRandGenSet = new CauseType() { Name = "randGenSet", CauseTypeGroup = ctgRandGen }.AddId();

            causeTypeSortable = new CauseType() { Name = "sortable", CauseTypeGroup = ctgSortable }.AddId();
            causeTypeSortableImport = new CauseType() { Name = "sortableImport", CauseTypeGroup = ctgSortable }.AddId();

            causeTypeSortableSet = new CauseType() { Name = "sortableSet", CauseTypeGroup = ctgSortableSet }.AddId();
            causeTypeSortableSetImport = new CauseType() { Name = "sortableSetImport", CauseTypeGroup = ctgSortableSet }.AddId();
            causeTypeSortableSetAllForOrder = new CauseType() { Name = "sortableSetAllForOrder", CauseTypeGroup = ctgSortableSet }.AddId();
            causeTypeSortableSetOrbit = new CauseType() { Name = "sortableSetOrbit", CauseTypeGroup = ctgSortableSet }.AddId();
            causeTypeSortableSetStacked = new CauseType() { Name = "sortableSetStacked", CauseTypeGroup = ctgSortableSet }.AddId();

            causeTypeSorter = new CauseType() { Name = "sorter", CauseTypeGroup = ctgSorter }.AddId();
            causeTypeSorterImport = new CauseType() { Name = "sorterImport", CauseTypeGroup = ctgSorter }.AddId();

            causeTypeSorterSetRef = new CauseType() { Name = "sorterSet", CauseTypeGroup = ctgSorterSet }.AddId();
            causeTypeSorterSetImport = new CauseType() { Name = "sorterSetImport", CauseTypeGroup = ctgSorterSet }.AddId();

            causeTypeSorterSetRndBySwitch = new CauseType() { Name = "sorterSetRandBySwitch", CauseTypeGroup = ctgSorterSetRand }.AddId();
            causeTypeSorterSetRndByStage = new CauseType() { Name = "sorterSetRandByStage", CauseTypeGroup = ctgSorterSetRand }.AddId();
            causeTypeSorterSetRndByRflStage = new CauseType() { Name = "sorterSetRandByRflStage", CauseTypeGroup = ctgSorterSetRand }.AddId();
            causeTypeSorterSetRndByRflStageBuddies = new CauseType() { Name = "sorterSetRandByRflStageBuddies", CauseTypeGroup = ctgSorterSetRand }.AddId();

            causeTypeSorterPerf = new CauseType() { Name = "sorterPerf", CauseTypeGroup = ctgSorterPerf }.AddId();
            causeTypeSorterSetPerfBins = new CauseType() { Name = "sorterSetPerfBins", CauseTypeGroup = ctgSorterPerf }.AddId();


            gortContext.CauseType.Add(causeTypeRandGen);
            gortContext.CauseType.Add(causeTypeRandGenSet);

            gortContext.CauseType.Add(causeTypeSortable);
            gortContext.CauseType.Add(causeTypeSortableImport);

            gortContext.CauseType.Add(causeTypeSortableSet);
            gortContext.CauseType.Add(causeTypeSortableSetImport);
            gortContext.CauseType.Add(causeTypeSortableSetAllForOrder);
            gortContext.CauseType.Add(causeTypeSortableSetOrbit);
            gortContext.CauseType.Add(causeTypeSortableSetStacked);

            gortContext.CauseType.Add(causeTypeSorter);
            gortContext.CauseType.Add(causeTypeSorterImport);

            gortContext.CauseType.Add(causeTypeSorterSetRef);
            gortContext.CauseType.Add(causeTypeSorterSetImport);
            gortContext.CauseType.Add(causeTypeSorterSetRndBySwitch);
            gortContext.CauseType.Add(causeTypeSorterSetRndByStage);
            gortContext.CauseType.Add(causeTypeSorterSetRndByRflStage);
            gortContext.CauseType.Add(causeTypeSorterSetRndByRflStageBuddies);

            gortContext.CauseType.Add(causeTypeSorterPerf);
            gortContext.CauseType.Add(causeTypeSorterSetPerfBins);
        }



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

            causeTypeParamSorterSetRndBySymmetricStage_Order = new CauseTypeParam() { Name = "Order", CauseType = causeTypeSorterSetRndByRflStage, DataType = DataType.Int }.AddId();
            causeTypeParamSorterSetRndBySymmetricStage_StageCount = new CauseTypeParam() { Name = "StageCount", CauseType = causeTypeSorterSetRndByRflStage, DataType = DataType.Int }.AddId();
            causeTypeParamSorterSetRndBySymmetricStage_RndGen = new CauseTypeParam() { Name = "RndGen", CauseType = causeTypeSorterSetRndByRflStage, DataType = DataType.Guid }.AddId();
            causeTypeParamSorterSetRndBySymmetricStage_SorterCount = new CauseTypeParam() { Name = "SorterCount", CauseType = causeTypeSorterSetRndByRflStage, DataType = DataType.Int }.AddId();

            causeTypeParamSorterSetRndBySymmetricStageBuddies_Order = new CauseTypeParam() { Name = "Order", CauseType = causeTypeSorterSetRndByRflStageBuddies, DataType = DataType.Int }.AddId();
            causeTypeParamSorterSetRndBySymmetricStageBuddies_StageCount = new CauseTypeParam() { Name = "StageCount", CauseType = causeTypeSorterSetRndByRflStageBuddies, DataType = DataType.Int }.AddId();
            causeTypeParamSorterSetRndBySymmetricStageBuddies_RndGen = new CauseTypeParam() { Name = "RndGen", CauseType = causeTypeSorterSetRndByRflStageBuddies, DataType = DataType.Guid }.AddId();
            causeTypeParamSorterSetRndBySymmetricStageBuddies_SorterCount = new CauseTypeParam() { Name = "SorterCount", CauseType = causeTypeSorterSetRndByRflStageBuddies, DataType = DataType.Int }.AddId();
            causeTypeParamSorterSetRndBySymmetricStageBuddies_BuddyCount = new CauseTypeParam() { Name = "BuddyCount", CauseType = causeTypeSorterSetRndByRflStageBuddies, DataType = DataType.Int }.AddId();

            causeTypeParamSorterPerf_Sorter = new CauseTypeParam() { Name = "Sorter", CauseType = causeTypeSorterPerf, DataType = DataType.Guid }.AddId();
            causeTypeParamSorterPerf_SortableSet = new CauseTypeParam() { Name = "SortableSet", CauseType = causeTypeSorterPerf, DataType = DataType.Guid }.AddId();

            causeTypeParamSorterSetPerfBins_SorterSet = new CauseTypeParam() { Name = "SorterSet", CauseType = causeTypeSorterSetPerfBins, DataType = DataType.Guid }.AddId();
            causeTypeParamSorterSetPerfBins_SortableSet = new CauseTypeParam() { Name = "SortableSet", CauseType = causeTypeSorterSetPerfBins, DataType = DataType.Guid }.AddId();
            causeTypeParamSorterSetPerfBins_SorterSaveMode = new CauseTypeParam() { Name = "SorterSaveMode", CauseType = causeTypeSorterSetPerfBins, DataType = DataType.Int }.AddId();


            gortContext.CauseTypeParam.Add(causeTypeParamRndGen_Seed);
            gortContext.CauseTypeParam.Add(causeTypeParamRndGen_Type);

            gortContext.CauseTypeParam.Add(causeTypeParamRndGenSet_Seed);
            gortContext.CauseTypeParam.Add(causeTypeParamRndGenSet_Type);
            gortContext.CauseTypeParam.Add(causeTypeParamRndGenSet_Count);

            gortContext.CauseTypeParam.Add(causeTypeParamSortable_Order);
            gortContext.CauseTypeParam.Add(causeTypeParamSortable_Format);
            gortContext.CauseTypeParam.Add(causeTypeParamSortable_Data);

            gortContext.CauseTypeParam.Add(causeTypeParamSortableImport_Workspace);
            gortContext.CauseTypeParam.Add(causeTypeParamSortableImport_Table);
            gortContext.CauseTypeParam.Add(causeTypeParamSortableImport_Record);
            gortContext.CauseTypeParam.Add(causeTypeParamSortableImport_Path);

            gortContext.CauseTypeParam.Add(causeTypeParamSortableSet_Order);
            gortContext.CauseTypeParam.Add(causeTypeParamSortableSet_Count);
            gortContext.CauseTypeParam.Add(causeTypeParamSortableSet_Format);
            gortContext.CauseTypeParam.Add(causeTypeParamSortableSet_Data);

            gortContext.CauseTypeParam.Add(causeTypeParamSortableSetImport_Workspace);
            gortContext.CauseTypeParam.Add(causeTypeParamSortableSetImport_Table);
            gortContext.CauseTypeParam.Add(causeTypeParamSortableSetImport_Record);

            gortContext.CauseTypeParam.Add(causeTypeParamSortableSetAllForOrder_Order);

            gortContext.CauseTypeParam.Add(causeTypeParamSortableSetOrbit_Perm);
            gortContext.CauseTypeParam.Add(causeTypeParamSortableSetOrbit_MaxLen);

            gortContext.CauseTypeParam.Add(causeTypeParamSortableSetStacked_Orders);

            gortContext.CauseTypeParam.Add(causeTypeParamSorter_Order);
            gortContext.CauseTypeParam.Add(causeTypeParamSorter_Data);

            gortContext.CauseTypeParam.Add(causeTypeParamSorterImport_Workspace);
            gortContext.CauseTypeParam.Add(causeTypeParamSorterImport_Table);
            gortContext.CauseTypeParam.Add(causeTypeParamSorterImport_Record);
            gortContext.CauseTypeParam.Add(causeTypeParamSorterImport_Path);

            gortContext.CauseTypeParam.Add(causeTypeParamSorterSetRef_Source);

            gortContext.CauseTypeParam.Add(causeTypeParamSorterSetImport_Workspace);
            gortContext.CauseTypeParam.Add(causeTypeParamSorterSetImport_Table);
            gortContext.CauseTypeParam.Add(causeTypeParamSorterSetImport_Record);
            gortContext.CauseTypeParam.Add(causeTypeParamSorterSetImport_Path);

            gortContext.CauseTypeParam.Add(causeTypeParamSorterSetRndBySwitch_Order);
            gortContext.CauseTypeParam.Add(causeTypeParamSorterSetRndBySwitch_RndGen);
            gortContext.CauseTypeParam.Add(causeTypeParamSorterSetRndBySwitch_SwitchCount);
            gortContext.CauseTypeParam.Add(causeTypeParamSorterSetRndBySwitch_SorterCount);

            gortContext.CauseTypeParam.Add(causeTypeParamSorterSetRndByStage_Order);
            gortContext.CauseTypeParam.Add(causeTypeParamSorterSetRndByStage_RndGen);
            gortContext.CauseTypeParam.Add(causeTypeParamSorterSetRndByStage_StageCount);
            gortContext.CauseTypeParam.Add(causeTypeParamSorterSetRndByStage_SorterCount);

            gortContext.CauseTypeParam.Add(causeTypeParamSorterSetRndBySymmetricStage_Order);
            gortContext.CauseTypeParam.Add(causeTypeParamSorterSetRndBySymmetricStage_StageCount);
            gortContext.CauseTypeParam.Add(causeTypeParamSorterSetRndBySymmetricStage_RndGen);
            gortContext.CauseTypeParam.Add(causeTypeParamSorterSetRndBySymmetricStage_SorterCount);

            gortContext.CauseTypeParam.Add(causeTypeParamSorterSetRndBySymmetricStageBuddies_Order);
            gortContext.CauseTypeParam.Add(causeTypeParamSorterSetRndBySymmetricStageBuddies_StageCount);
            gortContext.CauseTypeParam.Add(causeTypeParamSorterSetRndBySymmetricStageBuddies_RndGen);
            gortContext.CauseTypeParam.Add(causeTypeParamSorterSetRndBySymmetricStageBuddies_SorterCount);
            gortContext.CauseTypeParam.Add(causeTypeParamSorterSetRndBySymmetricStageBuddies_BuddyCount);

            gortContext.CauseTypeParam.Add(causeTypeParamSorterPerf_Sorter);
            gortContext.CauseTypeParam.Add(causeTypeParamSorterPerf_SortableSet);

            gortContext.CauseTypeParam.Add(causeTypeParamSorterSetPerfBins_SorterSet);
            gortContext.CauseTypeParam.Add(causeTypeParamSorterSetPerfBins_SortableSet);
            gortContext.CauseTypeParam.Add(causeTypeParamSorterSetPerfBins_SorterSaveMode);

        }


        public static void AddCauseRndGenSet(GortContext gortContext)
        {
            causeRndGenSetA = new Cause() { Description = "rndGenSet 123 10", CauseTypeID = causeTypeRandGenSet.CauseTypeId, CauseStatus = CauseStatus.Pending, Index = 1, Workspace = workspace1 };
            causeParamRndGenSet_Seed = new CauseParam() { Cause = causeRndGenSetA, CauseTypeParam = causeTypeParamRndGenSet_Seed, Value = BitConverter.GetBytes(123) };
            causeParamRndGenSet_Type = new CauseParam() { Cause = causeRndGenSetA, CauseTypeParam = causeTypeParamRndGenSet_Type, Value = BitConverter.GetBytes((int)RndGenType.Lcg) };
            causeParamRndGenSet_Count = new CauseParam() { Cause = causeRndGenSetA, CauseTypeParam = causeTypeParamRndGenSet_Count, Value = BitConverter.GetBytes(10) };
            causeRndGenSetA.CauseParams.Add(causeParamRndGenSet_Seed); 
            causeRndGenSetA.CauseParams.Add(causeParamRndGenSet_Type); 
            causeRndGenSetA.CauseParams.Add(causeParamRndGenSet_Count);
            gortContext.Cause.Add(causeRndGenSetA);
        }

        public static void AddCauseSortableSetAllForOrderA(GortContext gortContext)
        {
            causeSortableSetAllForOrderA = new Cause() { Description = "rndGenSet 123 10", CauseTypeID = causeTypeRandGenSet.CauseTypeId, CauseStatus = CauseStatus.Pending, Index = 2, Workspace = workspace1 };
            causeParamSortableSetAllForOrderA_Order = new CauseParam() { Cause = causeSortableSetAllForOrderA, CauseTypeParam = causeTypeParamSortableSetAllForOrder_Order, Value = BitConverter.GetBytes(16) };
            causeSortableSetAllForOrderA.CauseParams.Add(causeParamSortableSetAllForOrderA_Order);
            gortContext.Cause.Add(causeSortableSetAllForOrderA);
        }
    }
}
