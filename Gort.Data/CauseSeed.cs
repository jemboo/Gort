using System.Security.Cryptography;
using System.Text;

namespace Gort.Data
{
    internal static partial class CauseSeed
    {
        public static void TryThis()
        {
            var ctxt = new GortContext();
            var wrkSpace = new Workspace() { Name = "workspace1" };
            var cause1 = new Cause() { Workspace = wrkSpace };
            ctxt.Workspaces.Add(wrkSpace);
            ctxt.Causes.Add(cause1);
            ctxt.SaveChanges();
            var wsBack = ctxt.Workspaces.Where(x => x.Name == "workspace1");
            //var s = new Sorter() { CauseId}

        }


        public static void Init()
        {
            var ctxt = new GortContext();
            //GetRndGens(ctxt);
            //GetCauseTypeGroup(ctxt);
            //GetCauseTypes(ctxt);
            //GetCauseTypeParams(ctxt);
            //AddRndGens(ctxt);
            AddCauseTypeGroups(ctxt);
            AddCauseTypes(ctxt);
            AddCauseTypeParams(ctxt);

            ctxt.SaveChanges();
        }


        public static void GetCauseTypeGroup(GortContext gortContext)
        {
            ctgRoot = gortContext.CauseTypeGroups.Where(g => g.Name == "root").First();
            ctgRandGen = gortContext.CauseTypeGroups.Where(g => g.Name == "randGens").First();
            ctgSortable = gortContext.CauseTypeGroups.Where(g => g.Name == "Sortables").First();
            ctgSortableSet = gortContext.CauseTypeGroups.Where(g => g.Name == "SortableSets").First();
            ctgSorter = gortContext.CauseTypeGroups.Where(g => g.Name == "Sorters").First();
            ctgSorterSet = gortContext.CauseTypeGroups.Where(g => g.Name == "SorterSets").First();
            ctgSorterSetRand = gortContext.CauseTypeGroups.Where(g => g.Name == "RndSorterSets").First();
            ctgSorterPerf = gortContext.CauseTypeGroups.Where(g => g.Name == "SorterPerfs").First();
            ctgSorterGa = gortContext.CauseTypeGroups.Where(g => g.Name == "SorterGa").First();
        }

        public static void AddCauseTypeGroups(GortContext gortContext)
        {
            ctgRoot = new CauseTypeGroup() { Name = "root", Parent = null }.AddId();
            ctgRandGen = new CauseTypeGroup() { Name = "randGens", Parent = ctgRoot }.AddId();
            ctgSortable = new CauseTypeGroup() { Name = "Sortables", Parent = ctgRoot }.AddId();
            ctgSortableSet = new CauseTypeGroup() { Name = "SortableSets", Parent = ctgRoot }.AddId();
            ctgSorter = new CauseTypeGroup() { Name = "Sorters", Parent = ctgRoot }.AddId();
            ctgSorterSet = new CauseTypeGroup() { Name = "SorterSets", Parent = ctgRoot }.AddId();
            ctgSorterSetRand = new CauseTypeGroup() { Name = "RndSorterSets", Parent = ctgSorterSet }.AddId();
            ctgSorterPerf = new CauseTypeGroup() { Name = "SorterPerfs", Parent = ctgRoot }.AddId();
            ctgSorterGa = new CauseTypeGroup() { Name = "SorterGa", Parent = ctgRoot }.AddId();

            gortContext.CauseTypeGroups.Add(ctgRoot);
            gortContext.CauseTypeGroups.Add(ctgRandGen);
            gortContext.CauseTypeGroups.Add(ctgSortable);
            gortContext.CauseTypeGroups.Add(ctgSortableSet);
            gortContext.CauseTypeGroups.Add(ctgSorter);
            gortContext.CauseTypeGroups.Add(ctgSorterSet);
            gortContext.CauseTypeGroups.Add(ctgSorterSetRand);
            gortContext.CauseTypeGroups.Add(ctgSorterPerf);
            gortContext.CauseTypeGroups.Add(ctgSorterGa);
        }

        public static void GetCauseTypes(GortContext gortContext)
        {
            causeTypeRandGen = gortContext.CauseTypes.Where(g => g.Name == "randGen").First();
            causeTypeRandGenSet = gortContext.CauseTypes.Where(g => g.Name == "rnadGenSet").First();

            causeTypeSortable = gortContext.CauseTypes.Where(g => g.Name == "sortable").First();
            causeTypeSortableImport = gortContext.CauseTypes.Where(g => g.Name == "sortableImport").First();

            causeTypeSortableSet = gortContext.CauseTypes.Where(g => g.Name == "sortableSet").First();
            causeTypeSortableSetImport = gortContext.CauseTypes.Where(g => g.Name == "sortableSetImport").First();
            causeTypeSortableSetAllForOrder = gortContext.CauseTypes.Where(g => g.Name == "sortableSetAllForOrder").First();
            causeTypeSortableSetOrbit = gortContext.CauseTypes.Where(g => g.Name == "sortableSetOrbit").First();
            causeTypeSortableSetStacked = gortContext.CauseTypes.Where(g => g.Name == "sortableSetStacked").First();

            causeTypeSorter = gortContext.CauseTypes.Where(g => g.Name == "sorter").First();
            causeTypeSorterImport = gortContext.CauseTypes.Where(g => g.Name == "sorterImport").First();

            causeTypeSorterSetRef = gortContext.CauseTypes.Where(g => g.Name == "sorterSet").First();
            causeTypeSorterSetImport = gortContext.CauseTypes.Where(g => g.Name == "sorterSetImport").First();

            causeTypeSorterSetRndBySwitch = gortContext.CauseTypes.Where(g => g.Name == "sorterSetRandBySwitch").First();
            causeTypeSorterSetRndByStage = gortContext.CauseTypes.Where(g => g.Name == "sorterSetRandByStage").First();
            causeTypeSorterSetRndBySymetricStage = gortContext.CauseTypes.Where(g => g.Name == "sorterSetRandBySymmetricStage").First();
            causeTypeSorterSetRndBySymetricStageBuddies = gortContext.CauseTypes.Where(g => g.Name == "sorterSetRandBySymmetricStageBuddies").First();

            causeTypeSorterPerf = gortContext.CauseTypes.Where(g => g.Name == "sorterPerf").First();
            causeTypeSorterSetPerfBins = gortContext.CauseTypes.Where(g => g.Name == "sorterSetPerfBins").First();
        }

        public static void AddCauseTypes(GortContext gortContext)
        {
            causeTypeRandGen = new CauseType() { Name = "randGen", CauseTypeGroup = ctgRandGen }.AddId();
            causeTypeRandGenSet = new CauseType() { Name = "rnadGenSet", CauseTypeGroup = ctgRandGen }.AddId();

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
            causeTypeSorterSetRndBySymetricStage = new CauseType() { Name = "sorterSetRandBySymmetricStage", CauseTypeGroup = ctgSorterSetRand }.AddId();
            causeTypeSorterSetRndBySymetricStageBuddies = new CauseType() { Name = "sorterSetRandBySymmetricStageBuddies", CauseTypeGroup = ctgSorterSetRand }.AddId();

            causeTypeSorterPerf = new CauseType() { Name = "sorterPerf", CauseTypeGroup = ctgSorterPerf }.AddId();
            causeTypeSorterSetPerfBins = new CauseType() { Name = "sorterSetPerfBins", CauseTypeGroup = ctgSorterPerf }.AddId();


            gortContext.CauseTypes.Add(causeTypeRandGen);
            gortContext.CauseTypes.Add(causeTypeRandGenSet);

            gortContext.CauseTypes.Add(causeTypeSortable);
            gortContext.CauseTypes.Add(causeTypeSortableImport);

            gortContext.CauseTypes.Add(causeTypeSortableSet);
            gortContext.CauseTypes.Add(causeTypeSortableSetImport);
            gortContext.CauseTypes.Add(causeTypeSortableSetAllForOrder);
            gortContext.CauseTypes.Add(causeTypeSortableSetOrbit);
            gortContext.CauseTypes.Add(causeTypeSortableSetStacked);

            gortContext.CauseTypes.Add(causeTypeSorter);
            gortContext.CauseTypes.Add(causeTypeSorterImport);

            gortContext.CauseTypes.Add(causeTypeSorterSetRef);
            gortContext.CauseTypes.Add(causeTypeSorterSetImport);
            gortContext.CauseTypes.Add(causeTypeSorterSetRndBySwitch);
            gortContext.CauseTypes.Add(causeTypeSorterSetRndByStage);
            gortContext.CauseTypes.Add(causeTypeSorterSetRndBySymetricStage);
            gortContext.CauseTypes.Add(causeTypeSorterSetRndBySymetricStageBuddies);

            gortContext.CauseTypes.Add(causeTypeSorterPerf);
            gortContext.CauseTypes.Add(causeTypeSorterSetPerfBins);
        }


        public static void GetCauseTypeParams(GortContext gortContext)
        {
            causeTypeParamRndGen_Seed = gortContext.CauseTypeParams.Where(g => (g.Name == "Seed") && (g.CauseTypeId == causeTypeRandGen.CauseTypeId)).First();
            causeTypeParamRndGen_Type = gortContext.CauseTypeParams.Where(g => (g.Name == "Type") && (g.CauseTypeId == causeTypeRandGen.CauseTypeId)).First();

            causeTypeParamRndGenSet_Seed = gortContext.CauseTypeParams.Where(g => (g.Name == "Seed") && (g.CauseTypeId == causeTypeRandGenSet.CauseTypeId)).First();
            causeTypeParamRndGenSet_Type = gortContext.CauseTypeParams.Where(g => (g.Name == "Type") && (g.CauseTypeId == causeTypeRandGenSet.CauseTypeId)).First();
            causeTypeParamRndGenSet_Count = gortContext.CauseTypeParams.Where(g => (g.Name == "Count") && (g.CauseTypeId == causeTypeRandGenSet.CauseTypeId)).First();

            causeTypeParamSortable_Order = gortContext.CauseTypeParams.Where(g => (g.Name == "Order") && (g.CauseTypeId == causeTypeSortable.CauseTypeId)).First();
            causeTypeParamSortable_Descr = gortContext.CauseTypeParams.Where(g => (g.Name == "Descr") && (g.CauseTypeId == causeTypeSortable.CauseTypeId)).First();
            causeTypeParamSortable_Format = gortContext.CauseTypeParams.Where(g => (g.Name == "Format") && (g.CauseTypeId == causeTypeSortable.CauseTypeId)).First();
            causeTypeParamSortable_Data = gortContext.CauseTypeParams.Where(g => (g.Name == "Data") && (g.CauseTypeId == causeTypeSortable.CauseTypeId)).First();

            causeTypeParamSortableImport_Workspace = gortContext.CauseTypeParams.Where(g => (g.Name == "Workspace") && (g.CauseTypeId == causeTypeSortableImport.CauseTypeId)).First();
            causeTypeParamSortableImport_Table = gortContext.CauseTypeParams.Where(g => (g.Name == "Table") && (g.CauseTypeId == causeTypeSortableImport.CauseTypeId)).First();
            causeTypeParamSortableImport_Record = gortContext.CauseTypeParams.Where(g => (g.Name == "Record") && (g.CauseTypeId == causeTypeSortableImport.CauseTypeId)).First();
            causeTypeParamSortableImport_Path = gortContext.CauseTypeParams.Where(g => (g.Name == "Path") && (g.CauseTypeId == causeTypeSortableImport.CauseTypeId)).First();

            causeTypeParamSortableSet_Order = gortContext.CauseTypeParams.Where(g => (g.Name == "Order") && (g.CauseTypeId == causeTypeSortableSet.CauseTypeId)).First();
            causeTypeParamSortableSet_Count = gortContext.CauseTypeParams.Where(g => (g.Name == "Count") && (g.CauseTypeId == causeTypeSortableSet.CauseTypeId)).First();
            causeTypeParamSortableSet_Descr = gortContext.CauseTypeParams.Where(g => (g.Name == "Descr") && (g.CauseTypeId == causeTypeSortableSet.CauseTypeId)).First();
            causeTypeParamSortableSet_Format = gortContext.CauseTypeParams.Where(g => (g.Name == "Format") && (g.CauseTypeId == causeTypeSortableSet.CauseTypeId)).First();
            causeTypeParamSortableSet_Data = gortContext.CauseTypeParams.Where(g => (g.Name == "Data") && (g.CauseTypeId == causeTypeSortableSet.CauseTypeId)).First();

            causeTypeParamSortableSetImport_Workspace = gortContext.CauseTypeParams.Where(g => (g.Name == "Workspace") && (g.CauseTypeId == causeTypeSortableSetImport.CauseTypeId)).First();
            causeTypeParamSortableSetImport_Table = gortContext.CauseTypeParams.Where(g => (g.Name == "Table") && (g.CauseTypeId == causeTypeSortableSetImport.CauseTypeId)).First();
            causeTypeParamSortableSetImport_Record = gortContext.CauseTypeParams.Where(g => (g.Name == "Record") && (g.CauseTypeId == causeTypeSortableSetImport.CauseTypeId)).First();

            causeTypeParamSortableSetAllForOrder_Order = gortContext.CauseTypeParams.Where(g => (g.Name == "Order") && (g.CauseTypeId == causeTypeSortableSetAllForOrder.CauseTypeId)).First();

            causeTypeParamSortableSetOrbit_Perm = gortContext.CauseTypeParams.Where(g => (g.Name == "Perm") && (g.CauseTypeId == causeTypeSortableSetOrbit.CauseTypeId)).First();
            causeTypeParamSortableSetOrbit_MaxLen = gortContext.CauseTypeParams.Where(g => (g.Name == "MaxLen") && (g.CauseTypeId == causeTypeSortableSetOrbit.CauseTypeId)).First();

            causeTypeParamSortableSetStacked_Orders = gortContext.CauseTypeParams.Where(g => (g.Name == "Orders") && (g.CauseTypeId == causeTypeSortableSetStacked.CauseTypeId)).First();

            causeTypeParamSorter_Order = gortContext.CauseTypeParams.Where(g => (g.Name == "Order") && (g.CauseTypeId == causeTypeSorter.CauseTypeId)).First();
            causeTypeParamSorter_Descr = gortContext.CauseTypeParams.Where(g => (g.Name == "Descr") && (g.CauseTypeId == causeTypeSorter.CauseTypeId)).First();
            causeTypeParamSorter_Data = gortContext.CauseTypeParams.Where(g => (g.Name == "Data") && (g.CauseTypeId == causeTypeSorter.CauseTypeId)).First();

            causeTypeParamSorterImport_Workspace = gortContext.CauseTypeParams.Where(g => (g.Name == "Workspace") && (g.CauseTypeId == causeTypeSorterImport.CauseTypeId)).First();
            causeTypeParamSorterImport_Table = gortContext.CauseTypeParams.Where(g => (g.Name == "Table") && (g.CauseTypeId == causeTypeSorterImport.CauseTypeId)).First();
            causeTypeParamSorterImport_Record = gortContext.CauseTypeParams.Where(g => (g.Name == "Record") && (g.CauseTypeId == causeTypeSorterImport.CauseTypeId)).First();
            causeTypeParamSorterImport_Path = gortContext.CauseTypeParams.Where(g => (g.Name == "Path") && (g.CauseTypeId == causeTypeSorterImport.CauseTypeId)).First();

            causeTypeParamSorterSetRef_Source = gortContext.CauseTypeParams.Where(g => (g.Name == "Source") && (g.CauseTypeId == causeTypeSorterSetRef.CauseTypeId)).First();

            causeTypeParamSorterSetImport_Workspace = gortContext.CauseTypeParams.Where(g => (g.Name == "Workspace") && (g.CauseTypeId == causeTypeSorterSetImport.CauseTypeId)).First();
            causeTypeParamSorterSetImport_Table = gortContext.CauseTypeParams.Where(g => (g.Name == "Table") && (g.CauseTypeId == causeTypeSorterSetImport.CauseTypeId)).First();
            causeTypeParamSorterSetImport_Record = gortContext.CauseTypeParams.Where(g => (g.Name == "Record") && (g.CauseTypeId == causeTypeSorterSetImport.CauseTypeId)).First();
            causeTypeParamSorterSetImport_Path = gortContext.CauseTypeParams.Where(g => (g.Name == "Path") && (g.CauseTypeId == causeTypeSorterSetImport.CauseTypeId)).First();

            causeTypeParamSorterSetRndBySwitch_Order = gortContext.CauseTypeParams.Where(g => (g.Name == "Order") && (g.CauseTypeId == causeTypeSorterSetRndBySwitch.CauseTypeId)).First();
            causeTypeParamSorterSetRndBySwitch_SwitchCount = gortContext.CauseTypeParams.Where(g => (g.Name == "SwitchCount") && (g.CauseTypeId == causeTypeSorterSetRndBySwitch.CauseTypeId)).First();
            causeTypeParamSorterSetRndBySwitch_RndGen = gortContext.CauseTypeParams.Where(g => (g.Name == "RndGen") && (g.CauseTypeId == causeTypeSorterSetRndBySwitch.CauseTypeId)).First();
            causeTypeParamSorterSetRndBySwitch_SorterCount = gortContext.CauseTypeParams.Where(g => (g.Name == "SorterCount") && (g.CauseTypeId == causeTypeSorterSetRndBySwitch.CauseTypeId)).First();

            causeTypeParamSorterSetRndByStage_Order = gortContext.CauseTypeParams.Where(g => (g.Name == "Order") && (g.CauseTypeId == causeTypeSorterSetRndByStage.CauseTypeId)).First();
            causeTypeParamSorterSetRndByStage_RndGen = gortContext.CauseTypeParams.Where(g => (g.Name == "RndGen") && (g.CauseTypeId == causeTypeSorterSetRndByStage.CauseTypeId)).First();
            causeTypeParamSorterSetRndByStage_StageCount = gortContext.CauseTypeParams.Where(g => (g.Name == "StageCount") && (g.CauseTypeId == causeTypeSorterSetRndByStage.CauseTypeId)).First();
            causeTypeParamSorterSetRndByStage_SorterCount = gortContext.CauseTypeParams.Where(g => (g.Name == "SorterCount") && (g.CauseTypeId == causeTypeSorterSetRndByStage.CauseTypeId)).First();

            causeTypeParamSorterSetRndBySymmetricStage_Order = gortContext.CauseTypeParams.Where(g => (g.Name == "Order") && (g.CauseTypeId == causeTypeSorterSetRndBySymetricStage.CauseTypeId)).First();
            causeTypeParamSorterSetRndBySymmetricStage_StageCount = gortContext.CauseTypeParams.Where(g => (g.Name == "StageCount") && (g.CauseTypeId == causeTypeSorterSetRndBySymetricStage.CauseTypeId)).First();
            causeTypeParamSorterSetRndBySymmetricStage_RndGen = gortContext.CauseTypeParams.Where(g => (g.Name == "RndGen") && (g.CauseTypeId == causeTypeSorterSetRndBySymetricStage.CauseTypeId)).First();
            causeTypeParamSorterSetRndBySymmetricStage_SorterCount = gortContext.CauseTypeParams.Where(g => (g.Name == "SorterCount") && (g.CauseTypeId == causeTypeSorterSetRndBySymetricStage.CauseTypeId)).First();

            causeTypeParamSorterSetRndBySymmetricStageBuddies_Order = gortContext.CauseTypeParams.Where(g => (g.Name == "Order") && (g.CauseTypeId == causeTypeSorterSetRndBySymetricStageBuddies.CauseTypeId)).First();
            causeTypeParamSorterSetRndBySymmetricStageBuddies_StageCount = gortContext.CauseTypeParams.Where(g => (g.Name == "StageCount") && (g.CauseTypeId == causeTypeSorterSetRndBySymetricStageBuddies.CauseTypeId)).First();
            causeTypeParamSorterSetRndBySymmetricStageBuddies_RndGen = gortContext.CauseTypeParams.Where(g => (g.Name == "RndGen") && (g.CauseTypeId == causeTypeSorterSetRndBySymetricStageBuddies.CauseTypeId)).First();
            causeTypeParamSorterSetRndBySymmetricStageBuddies_SorterCount = gortContext.CauseTypeParams.Where(g => (g.Name == "SorterCount") && (g.CauseTypeId == causeTypeSorterSetRndBySymetricStageBuddies.CauseTypeId)).First();
            causeTypeParamSorterSetRndBySymmetricStageBuddies_BuddyCount = gortContext.CauseTypeParams.Where(g => (g.Name == "BuddyCount") && (g.CauseTypeId == causeTypeSorterSetRndBySymetricStageBuddies.CauseTypeId)).First();

            causeTypeParamSorterPerf_Sorter = gortContext.CauseTypeParams.Where(g => (g.Name == "Sorter") && (g.CauseTypeId == causeTypeSorterPerf.CauseTypeId)).First();
            causeTypeParamSorterPerf_SortableSet = gortContext.CauseTypeParams.Where(g => (g.Name == "SortableSet") && (g.CauseTypeId == causeTypeSorterPerf.CauseTypeId)).First();

            causeTypeParamSorterSetPerfBins_SorterSet = gortContext.CauseTypeParams.Where(g => (g.Name == "SorterSet") && (g.CauseTypeId == causeTypeSorterSetPerfBins.CauseTypeId)).First();
            causeTypeParamSorterSetPerfBins_SortableSet = gortContext.CauseTypeParams.Where(g => (g.Name == "SortableSet") && (g.CauseTypeId == causeTypeSorterSetPerfBins.CauseTypeId)).First();
            causeTypeParamSorterSetPerfBins_SorterSaveMode = gortContext.CauseTypeParams.Where(g => (g.Name == "SorterSaveMode") && (g.CauseTypeId == causeTypeSorterSetPerfBins.CauseTypeId)).First();

        }

    }
}
