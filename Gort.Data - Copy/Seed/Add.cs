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
            ctRandGen = new CauseType() { Name = "randGen", CauseTypeGroup = ctgRandGen }.AddId();
            ctRandGenSet = new CauseType() { Name = "randGenSet", CauseTypeGroup = ctgRandGen }.AddId();

            ctSortable = new CauseType() { Name = "sortable", CauseTypeGroup = ctgSortable }.AddId();
            ctSortableImport = new CauseType() { Name = "sortableImport", CauseTypeGroup = ctgSortable }.AddId();

            ctSortableSet = new CauseType() { Name = "sortableSet", CauseTypeGroup = ctgSortableSet }.AddId();
            ctSortableSetImport = new CauseType() { Name = "sortableSetImport", CauseTypeGroup = ctgSortableSet }.AddId();
            ctSortableSetAllForOrder = new CauseType() { Name = "sortableSetAllForOrder", CauseTypeGroup = ctgSortableSet }.AddId();
            ctSortableSetOrbit = new CauseType() { Name = "sortableSetOrbit", CauseTypeGroup = ctgSortableSet }.AddId();
            ctSortableSetStacked = new CauseType() { Name = "sortableSetStacked", CauseTypeGroup = ctgSortableSet }.AddId();

            ctSorter = new CauseType() { Name = "sorter", CauseTypeGroup = ctgSorter }.AddId();
            ctSorterImport = new CauseType() { Name = "sorterImport", CauseTypeGroup = ctgSorter }.AddId();

            ctSorterSetRef = new CauseType() { Name = "sorterSet", CauseTypeGroup = ctgSorterSet }.AddId();
            ctSorterSetImport = new CauseType() { Name = "sorterSetImport", CauseTypeGroup = ctgSorterSet }.AddId();

            ctSorterSetRndBySwitch = new CauseType() { Name = "sorterSetRandBySwitch", CauseTypeGroup = ctgSorterSetRand }.AddId();
            ctSorterSetRndByStage = new CauseType() { Name = "sorterSetRandByStage", CauseTypeGroup = ctgSorterSetRand }.AddId();
            ctSorterSetRndByRflStage = new CauseType() { Name = "sorterSetRandByRflStage", CauseTypeGroup = ctgSorterSetRand }.AddId();
            ctSorterSetRndByRflStageBuddies = new CauseType() { Name = "sorterSetRandByRflStageBuddies", CauseTypeGroup = ctgSorterSetRand }.AddId();

            ctSorterPerf = new CauseType() { Name = "sorterPerf", CauseTypeGroup = ctgSorterPerf }.AddId();
            ctSorterSetPerfBins = new CauseType() { Name = "sorterSetPerfBins", CauseTypeGroup = ctgSorterPerf }.AddId();


            gortContext.CauseType.Add(ctRandGen);
            gortContext.CauseType.Add(ctRandGenSet);

            gortContext.CauseType.Add(ctSortable);
            gortContext.CauseType.Add(ctSortableImport);

            gortContext.CauseType.Add(ctSortableSet);
            gortContext.CauseType.Add(ctSortableSetImport);
            gortContext.CauseType.Add(ctSortableSetAllForOrder);
            gortContext.CauseType.Add(ctSortableSetOrbit);
            gortContext.CauseType.Add(ctSortableSetStacked);

            gortContext.CauseType.Add(ctSorter);
            gortContext.CauseType.Add(ctSorterImport);

            gortContext.CauseType.Add(ctSorterSetRef);
            gortContext.CauseType.Add(ctSorterSetImport);
            gortContext.CauseType.Add(ctSorterSetRndBySwitch);
            gortContext.CauseType.Add(ctSorterSetRndByStage);
            gortContext.CauseType.Add(ctSorterSetRndByRflStage);
            gortContext.CauseType.Add(ctSorterSetRndByRflStageBuddies);

            gortContext.CauseType.Add(ctSorterPerf);
            gortContext.CauseType.Add(ctSorterSetPerfBins);
        }


        public static void AddCauseParamTypes(GortContext gortContext)
        {
            cptRndGen_Seed = new CauseParamType() { Name = "Seed", CauseType = ctRandGen, DataType = DataType.Int }.AddId();
            cptRndGen_Type = new CauseParamType() { Name = "Type", CauseType = ctRandGen, DataType = DataType.Int }.AddId();

            cptRndGenSet_Seed = new CauseParamType() { Name = "Seed", CauseType = ctRandGenSet, DataType = DataType.Int }.AddId();
            cptRndGenSet_Type = new CauseParamType() { Name = "Type", CauseType = ctRandGenSet, DataType = DataType.Int }.AddId();
            cptRndGenSet_Count = new CauseParamType() { Name = "Count", CauseType = ctRandGenSet, DataType = DataType.Int }.AddId();

            cptSortable_Order = new CauseParamType() { Name = "Order", CauseType = ctSortable, DataType = DataType.Int }.AddId();
            cptSortable_Format = new CauseParamType() { Name = "Format", CauseType = ctSortable, DataType = DataType.Int }.AddId();
            cptSortable_Data = new CauseParamType() { Name = "Data", CauseType = ctSortable, DataType = DataType.ByteArray }.AddId();

            cptSortableImport_Workspace = new CauseParamType() { Name = "Workspace", CauseType = ctSortableImport, DataType = DataType.Guid }.AddId();
            cptSortableImport_Table = new CauseParamType() { Name = "Table", CauseType = ctSortableImport, DataType = DataType.Guid }.AddId();
            cptSortableImport_Record = new CauseParamType() { Name = "Record", CauseType = ctSortableImport, DataType = DataType.Guid }.AddId();
            cptSortableImport_Path = new CauseParamType() { Name = "Path", CauseType = ctSortableImport, DataType = DataType.String }.AddId();

            cptSortableSet_Order = new CauseParamType() { Name = "Order", CauseType = ctSortableSet, DataType = DataType.Int }.AddId();
            cptSortableSet_Count = new CauseParamType() { Name = "Count", CauseType = ctSortableSet, DataType = DataType.Int }.AddId();
            cptSortableSet_Format = new CauseParamType() { Name = "Format", CauseType = ctSortableSet, DataType = DataType.Int }.AddId();
            cptSortableSet_Data = new CauseParamType() { Name = "Data", CauseType = ctSortableSet, DataType = DataType.ByteArray }.AddId();

            cptSortableSetImport_Workspace = new CauseParamType() { Name = "Workspace", CauseType = ctSortableSetImport, DataType = DataType.Guid }.AddId();
            cptSortableSetImport_Table = new CauseParamType() { Name = "Table", CauseType = ctSortableSetImport, DataType = DataType.Guid }.AddId();
            cptSortableSetImport_Record = new CauseParamType() { Name = "Record", CauseType = ctSortableSetImport, DataType = DataType.Guid }.AddId();

            cptSortableSetAllForOrder_Order = new CauseParamType() { Name = "Order", CauseType = ctSortableSetAllForOrder, DataType = DataType.Int }.AddId();

            cptSortableSetOrbit_Perm = new CauseParamType() { Name = "Perm", CauseType = ctSortableSetOrbit, DataType = DataType.IntArray }.AddId();
            cptSortableSetOrbit_MaxLen = new CauseParamType() { Name = "MaxLen", CauseType = ctSortableSetOrbit, DataType = DataType.Int }.AddId();

            cptSortableSetStacked_Orders = new CauseParamType() { Name = "Orders", CauseType = ctSortableSetStacked, DataType = DataType.IntArray }.AddId();

            cptSorter_Order = new CauseParamType() { Name = "Order", CauseType = ctSorter, DataType = DataType.Int }.AddId();
            cptSorter_Data = new CauseParamType() { Name = "Data", CauseType = ctSorter, DataType = DataType.ByteArray }.AddId();

            cptSorterImport_Workspace = new CauseParamType() { Name = "Workspace", CauseType = ctSorterImport, DataType = DataType.Guid }.AddId();
            cptSorterImport_Table = new CauseParamType() { Name = "Table", CauseType = ctSorterImport, DataType = DataType.Guid }.AddId();
            cptSorterImport_Record = new CauseParamType() { Name = "Record", CauseType = ctSorterImport, DataType = DataType.Guid }.AddId();
            cptSorterImport_Path = new CauseParamType() { Name = "Path", CauseType = ctSorterImport, DataType = DataType.String }.AddId();

            cptSorterSetRef_Source = new CauseParamType() { Name = "Source", CauseType = ctSorterSetRef, DataType = DataType.String }.AddId();

            cptSorterSetImport_Workspace = new CauseParamType() { Name = "Workspace", CauseType = ctSorterSetImport, DataType = DataType.Guid }.AddId();
            cptSorterSetImport_Table = new CauseParamType() { Name = "Table", CauseType = ctSorterSetImport, DataType = DataType.Guid }.AddId();
            cptSorterSetImport_Record = new CauseParamType() { Name = "Record", CauseType = ctSorterSetImport, DataType = DataType.Guid }.AddId();
            cptSorterSetImport_Path = new CauseParamType() { Name = "Path", CauseType = ctSorterSetImport, DataType = DataType.String }.AddId();

            cptSorterSetRndBySwitch_Order = new CauseParamType() { Name = "Order", CauseType = ctSorterSetRndBySwitch, DataType = DataType.Int }.AddId();
            cptSorterSetRndBySwitch_SwitchCount = new CauseParamType() { Name = "SwitchCount", CauseType = ctSorterSetRndBySwitch, DataType = DataType.Int }.AddId();
            cptSorterSetRndBySwitch_RndGen = new CauseParamType() { Name = "RndGen", CauseType = ctSorterSetRndBySwitch, DataType = DataType.Guid }.AddId();
            cptSorterSetRndBySwitch_SorterCount = new CauseParamType() { Name = "SorterCount", CauseType = ctSorterSetRndBySwitch, DataType = DataType.Guid }.AddId();

            cptSorterSetRndByStage_Order = new CauseParamType() { Name = "Order", CauseType = ctSorterSetRndByStage, DataType = DataType.Int }.AddId();
            cptSorterSetRndByStage_RndGen = new CauseParamType() { Name = "RndGen", CauseType = ctSorterSetRndByStage, DataType = DataType.Guid }.AddId();
            cptSorterSetRndByStage_StageCount = new CauseParamType() { Name = "StageCount", CauseType = ctSorterSetRndByStage, DataType = DataType.Int }.AddId();
            cptSorterSetRndByStage_SorterCount = new CauseParamType() { Name = "SorterCount", CauseType = ctSorterSetRndByStage, DataType = DataType.Int }.AddId();

            cptSorterSetRndBySymmetricStage_Order = new CauseParamType() { Name = "Order", CauseType = ctSorterSetRndByRflStage, DataType = DataType.Int }.AddId();
            cptSorterSetRndBySymmetricStage_StageCount = new CauseParamType() { Name = "StageCount", CauseType = ctSorterSetRndByRflStage, DataType = DataType.Int }.AddId();
            cptSorterSetRndBySymmetricStage_RndGen = new CauseParamType() { Name = "RndGen", CauseType = ctSorterSetRndByRflStage, DataType = DataType.Guid }.AddId();
            cptSorterSetRndBySymmetricStage_SorterCount = new CauseParamType() { Name = "SorterCount", CauseType = ctSorterSetRndByRflStage, DataType = DataType.Int }.AddId();

            cptSorterSetRndBySymmetricStageBuddies_Order = new CauseParamType() { Name = "Order", CauseType = ctSorterSetRndByRflStageBuddies, DataType = DataType.Int }.AddId();
            cptSorterSetRndBySymmetricStageBuddies_StageCount = new CauseParamType() { Name = "StageCount", CauseType = ctSorterSetRndByRflStageBuddies, DataType = DataType.Int }.AddId();
            cptSorterSetRndBySymmetricStageBuddies_RndGen = new CauseParamType() { Name = "RndGen", CauseType = ctSorterSetRndByRflStageBuddies, DataType = DataType.Guid }.AddId();
            cptSorterSetRndBySymmetricStageBuddies_SorterCount = new CauseParamType() { Name = "SorterCount", CauseType = ctSorterSetRndByRflStageBuddies, DataType = DataType.Int }.AddId();
            cptSorterSetRndBySymmetricStageBuddies_BuddyCount = new CauseParamType() { Name = "BuddyCount", CauseType = ctSorterSetRndByRflStageBuddies, DataType = DataType.Int }.AddId();

            cptSorterPerf_Sorter = new CauseParamType() { Name = "Sorter", CauseType = ctSorterPerf, DataType = DataType.Guid }.AddId();
            cptSorterPerf_SortableSet = new CauseParamType() { Name = "SortableSet", CauseType = ctSorterPerf, DataType = DataType.Guid }.AddId();

            cptSorterSetPerfBins_SorterSet = new CauseParamType() { Name = "SorterSet", CauseType = ctSorterSetPerfBins, DataType = DataType.Guid }.AddId();
            cptSorterSetPerfBins_SortableSet = new CauseParamType() { Name = "SortableSet", CauseType = ctSorterSetPerfBins, DataType = DataType.Guid }.AddId();
            cptSorterSetPerfBins_SorterSaveMode = new CauseParamType() { Name = "SorterSaveMode", CauseType = ctSorterSetPerfBins, DataType = DataType.Int }.AddId();


            gortContext.CauseParamType.Add(cptRndGen_Seed);
            gortContext.CauseParamType.Add(cptRndGen_Type);

            gortContext.CauseParamType.Add(cptRndGenSet_Seed);
            gortContext.CauseParamType.Add(cptRndGenSet_Type);
            gortContext.CauseParamType.Add(cptRndGenSet_Count);

            gortContext.CauseParamType.Add(cptSortable_Order);
            gortContext.CauseParamType.Add(cptSortable_Format);
            gortContext.CauseParamType.Add(cptSortable_Data);

            gortContext.CauseParamType.Add(cptSortableImport_Workspace);
            gortContext.CauseParamType.Add(cptSortableImport_Table);
            gortContext.CauseParamType.Add(cptSortableImport_Record);
            gortContext.CauseParamType.Add(cptSortableImport_Path);

            gortContext.CauseParamType.Add(cptSortableSet_Order);
            gortContext.CauseParamType.Add(cptSortableSet_Count);
            gortContext.CauseParamType.Add(cptSortableSet_Format);
            gortContext.CauseParamType.Add(cptSortableSet_Data);

            gortContext.CauseParamType.Add(cptSortableSetImport_Workspace);
            gortContext.CauseParamType.Add(cptSortableSetImport_Table);
            gortContext.CauseParamType.Add(cptSortableSetImport_Record);

            gortContext.CauseParamType.Add(cptSortableSetAllForOrder_Order);

            gortContext.CauseParamType.Add(cptSortableSetOrbit_Perm);
            gortContext.CauseParamType.Add(cptSortableSetOrbit_MaxLen);

            gortContext.CauseParamType.Add(cptSortableSetStacked_Orders);

            gortContext.CauseParamType.Add(cptSorter_Order);
            gortContext.CauseParamType.Add(cptSorter_Data);

            gortContext.CauseParamType.Add(cptSorterImport_Workspace);
            gortContext.CauseParamType.Add(cptSorterImport_Table);
            gortContext.CauseParamType.Add(cptSorterImport_Record);
            gortContext.CauseParamType.Add(cptSorterImport_Path);

            gortContext.CauseParamType.Add(cptSorterSetRef_Source);

            gortContext.CauseParamType.Add(cptSorterSetImport_Workspace);
            gortContext.CauseParamType.Add(cptSorterSetImport_Table);
            gortContext.CauseParamType.Add(cptSorterSetImport_Record);
            gortContext.CauseParamType.Add(cptSorterSetImport_Path);

            gortContext.CauseParamType.Add(cptSorterSetRndBySwitch_Order);
            gortContext.CauseParamType.Add(cptSorterSetRndBySwitch_RndGen);
            gortContext.CauseParamType.Add(cptSorterSetRndBySwitch_SwitchCount);
            gortContext.CauseParamType.Add(cptSorterSetRndBySwitch_SorterCount);

            gortContext.CauseParamType.Add(cptSorterSetRndByStage_Order);
            gortContext.CauseParamType.Add(cptSorterSetRndByStage_RndGen);
            gortContext.CauseParamType.Add(cptSorterSetRndByStage_StageCount);
            gortContext.CauseParamType.Add(cptSorterSetRndByStage_SorterCount);

            gortContext.CauseParamType.Add(cptSorterSetRndBySymmetricStage_Order);
            gortContext.CauseParamType.Add(cptSorterSetRndBySymmetricStage_StageCount);
            gortContext.CauseParamType.Add(cptSorterSetRndBySymmetricStage_RndGen);
            gortContext.CauseParamType.Add(cptSorterSetRndBySymmetricStage_SorterCount);

            gortContext.CauseParamType.Add(cptSorterSetRndBySymmetricStageBuddies_Order);
            gortContext.CauseParamType.Add(cptSorterSetRndBySymmetricStageBuddies_StageCount);
            gortContext.CauseParamType.Add(cptSorterSetRndBySymmetricStageBuddies_RndGen);
            gortContext.CauseParamType.Add(cptSorterSetRndBySymmetricStageBuddies_SorterCount);
            gortContext.CauseParamType.Add(cptSorterSetRndBySymmetricStageBuddies_BuddyCount);

            gortContext.CauseParamType.Add(cptSorterPerf_Sorter);
            gortContext.CauseParamType.Add(cptSorterPerf_SortableSet);

            gortContext.CauseParamType.Add(cptSorterSetPerfBins_SorterSet);
            gortContext.CauseParamType.Add(cptSorterSetPerfBins_SortableSet);
            gortContext.CauseParamType.Add(cptSorterSetPerfBins_SorterSaveMode);

        }


        public static void AddCauseRndGenSet(GortContext gortContext)
        {
            causeRndGenSetA = new Cause() { Description = "rndGenSet 123 10", CauseTypeID = ctRandGenSet.CauseTypeId, CauseStatus = CauseStatus.Pending, Index = 1, Workspace = workspace1 };
            cpRndGenSet_Seed = new CauseParam() { Cause = causeRndGenSetA, CauseParamType = cptRndGenSet_Seed, Value = BitConverter.GetBytes(123) }.AddId();
            cpRndGenSet_Type = new CauseParam() { Cause = causeRndGenSetA, CauseParamType = cptRndGenSet_Type, Value = BitConverter.GetBytes((int)RndGenType.Lcg) }.AddId();
            cpRndGenSet_Count = new CauseParam() { Cause = causeRndGenSetA, CauseParamType = cptRndGenSet_Count, Value = BitConverter.GetBytes(10) }.AddId();
            causeRndGenSetA.CauseParams.Add(cpRndGenSet_Seed); 
            causeRndGenSetA.CauseParams.Add(cpRndGenSet_Type); 
            causeRndGenSetA.CauseParams.Add(cpRndGenSet_Count);
            gortContext.Cause.Add(causeRndGenSetA);
        }

        public static void AddCauseSortableSetAllForOrderA(GortContext gortContext)
        {
            causeSortableSetAllForOrderA = new Cause() { Description = "allSortables 16", CauseTypeID = ctRandGenSet.CauseTypeId, CauseStatus = CauseStatus.Pending, Index = 2, Workspace = workspace1 };
            cpSortableSetAllForOrder_Order = new CauseParam() { Cause = causeSortableSetAllForOrderA, CauseParamType = cptSortableSetAllForOrder_Order, Value = BitConverter.GetBytes(16) }.AddId();
            causeSortableSetAllForOrderA.CauseParams.Add(cpSortableSetAllForOrder_Order);
            gortContext.Cause.Add(causeSortableSetAllForOrderA);
        }
    }
}
