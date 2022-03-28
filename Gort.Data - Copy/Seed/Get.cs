using System.Security.Cryptography;
using System.Text;

namespace Gort.Data.Seed
{
    internal static partial class CauseSeed
    {


        public static void GetCauseTypeGroups(GortContext gortContext)
        {
            ctgRoot = gortContext.CauseTypeGroup.Where(g => g.Name == "root").First();
            ctgRandGen = gortContext.CauseTypeGroup.Where(g => g.Name == "randGen").First();
            ctgSortable = gortContext.CauseTypeGroup.Where(g => g.Name == "Sortable").First();
            ctgSortableSet = gortContext.CauseTypeGroup.Where(g => g.Name == "SortableSet").First();
            ctgSorter = gortContext.CauseTypeGroup.Where(g => g.Name == "Sorter").First();
            ctgSorterSet = gortContext.CauseTypeGroup.Where(g => g.Name == "SorterSet").First();
            ctgSorterSetRand = gortContext.CauseTypeGroup.Where(g => g.Name == "RndSorterSet").First();
            ctgSorterPerf = gortContext.CauseTypeGroup.Where(g => g.Name == "SorterPerf").First();
            ctgSorterGa = gortContext.CauseTypeGroup.Where(g => g.Name == "SorterGa").First();

            //gortContext.CauseTypeGroup.Attach(ctgRoot);
            //gortContext.CauseTypeGroup.Attach(ctgRandGen);
            //gortContext.CauseTypeGroup.Attach(ctgSortable);
            //gortContext.CauseTypeGroup.Attach(ctgSortableSet);
            //gortContext.CauseTypeGroup.Attach(ctgSorter);
            //gortContext.CauseTypeGroup.Attach(ctgSorterSet);
            //gortContext.CauseTypeGroup.Attach(ctgSorterSetRand);
            //gortContext.CauseTypeGroup.Attach(ctgSorterPerf);
            //gortContext.CauseTypeGroup.Attach(ctgSorterGa);
        }

        public static void GetCauseTypes(GortContext gortContext)
        {
            ctRandGen = gortContext.CauseType.Where(g => g.Name == "randGen").First();
            ctRandGenSet = gortContext.CauseType.Where(g => g.Name == "randGenSet").First();

            ctSortable = gortContext.CauseType.Where(g => g.Name == "sortable").First();
            ctSortableImport = gortContext.CauseType.Where(g => g.Name == "sortableImport").First();

            ctSortableSet = gortContext.CauseType.Where(g => g.Name == "sortableSet").First();
            ctSortableSetImport = gortContext.CauseType.Where(g => g.Name == "sortableSetImport").First();
            ctSortableSetAllForOrder = gortContext.CauseType.Where(g => g.Name == "sortableSetAllForOrder").First();
            ctSortableSetOrbit = gortContext.CauseType.Where(g => g.Name == "sortableSetOrbit").First();
            ctSortableSetStacked = gortContext.CauseType.Where(g => g.Name == "sortableSetStacked").First();

            ctSorter = gortContext.CauseType.Where(g => g.Name == "sorter").First();
            ctSorterImport = gortContext.CauseType.Where(g => g.Name == "sorterImport").First();

            ctSorterSetRef = gortContext.CauseType.Where(g => g.Name == "sorterSet").First();
            ctSorterSetImport = gortContext.CauseType.Where(g => g.Name == "sorterSetImport").First();

            ctSorterSetRndBySwitch = gortContext.CauseType.Where(g => g.Name == "sorterSetRandBySwitch").First();
            ctSorterSetRndByStage = gortContext.CauseType.Where(g => g.Name == "sorterSetRandByStage").First();
            ctSorterSetRndByRflStage = gortContext.CauseType.Where(g => g.Name == "sorterSetRandByRflStage").First();
            ctSorterSetRndByRflStageBuddies = gortContext.CauseType.Where(g => g.Name == "sorterSetRandByRflStageBuddies").First();

            ctSorterPerf = gortContext.CauseType.Where(g => g.Name == "sorterPerf").First();
            ctSorterSetPerfBins = gortContext.CauseType.Where(g => g.Name == "sorterSetPerfBins").First();

            gortContext.CauseType.Attach(ctRandGen);
            gortContext.CauseType.Attach(ctRandGenSet);

            gortContext.CauseType.Attach(ctSortable);
            gortContext.CauseType.Attach(ctSortableImport);

            gortContext.CauseType.Attach(ctSortableSet);
            gortContext.CauseType.Attach(ctSortableSetImport);
            gortContext.CauseType.Attach(ctSortableSetAllForOrder);
            gortContext.CauseType.Attach(ctSortableSetOrbit);
            gortContext.CauseType.Attach(ctSortableSetStacked);

            gortContext.CauseType.Attach(ctSorter);
            gortContext.CauseType.Attach(ctSorterImport);

            gortContext.CauseType.Attach(ctSorterSetRef);
            gortContext.CauseType.Attach(ctSorterSetImport);

            gortContext.CauseType.Attach(ctSorterSetRndBySwitch);
            gortContext.CauseType.Attach(ctSorterSetRndByStage);
            gortContext.CauseType.Attach(ctSorterSetRndByRflStage);
            gortContext.CauseType.Attach(ctSorterSetRndByRflStageBuddies);

            gortContext.CauseType.Attach(ctSorterPerf);
            gortContext.CauseType.Attach(ctSorterSetPerfBins);
        }

        public static void GetCauseParamTypes(GortContext gortContext)
        {
            cptRndGen_Seed = gortContext.CauseParamType.Where(g => (g.Name == "Seed") && (g.CauseTypeId == ctRandGen.CauseTypeId)).First();
            cptRndGen_Type = gortContext.CauseParamType.Where(g => (g.Name == "Type") && (g.CauseTypeId == ctRandGen.CauseTypeId)).First();

            cptRndGenSet_Seed = gortContext.CauseParamType.Where(g => (g.Name == "Seed") && (g.CauseTypeId == ctRandGenSet.CauseTypeId)).First();
            cptRndGenSet_Type = gortContext.CauseParamType.Where(g => (g.Name == "Type") && (g.CauseTypeId == ctRandGenSet.CauseTypeId)).First();
            cptRndGenSet_Count = gortContext.CauseParamType.Where(g => (g.Name == "Count") && (g.CauseTypeId == ctRandGenSet.CauseTypeId)).First();

            cptSortable_Order = gortContext.CauseParamType.Where(g => (g.Name == "Order") && (g.CauseTypeId == ctSortable.CauseTypeId)).First();
            cptSortable_Format = gortContext.CauseParamType.Where(g => (g.Name == "Format") && (g.CauseTypeId == ctSortable.CauseTypeId)).First();
            cptSortable_Data = gortContext.CauseParamType.Where(g => (g.Name == "Data") && (g.CauseTypeId == ctSortable.CauseTypeId)).First();

            cptSortableImport_Workspace = gortContext.CauseParamType.Where(g => (g.Name == "Workspace") && (g.CauseTypeId == ctSortableImport.CauseTypeId)).First();
            cptSortableImport_Table = gortContext.CauseParamType.Where(g => (g.Name == "Table") && (g.CauseTypeId == ctSortableImport.CauseTypeId)).First();
            cptSortableImport_Record = gortContext.CauseParamType.Where(g => (g.Name == "Record") && (g.CauseTypeId == ctSortableImport.CauseTypeId)).First();
            cptSortableImport_Path = gortContext.CauseParamType.Where(g => (g.Name == "Path") && (g.CauseTypeId == ctSortableImport.CauseTypeId)).First();

            cptSortableSet_Order = gortContext.CauseParamType.Where(g => (g.Name == "Order") && (g.CauseTypeId == ctSortableSet.CauseTypeId)).First();
            cptSortableSet_Count = gortContext.CauseParamType.Where(g => (g.Name == "Count") && (g.CauseTypeId == ctSortableSet.CauseTypeId)).First();
            cptSortableSet_Format = gortContext.CauseParamType.Where(g => (g.Name == "Format") && (g.CauseTypeId == ctSortableSet.CauseTypeId)).First();
            cptSortableSet_Data = gortContext.CauseParamType.Where(g => (g.Name == "Data") && (g.CauseTypeId == ctSortableSet.CauseTypeId)).First();

            cptSortableSetImport_Workspace = gortContext.CauseParamType.Where(g => (g.Name == "Workspace") && (g.CauseTypeId == ctSortableSetImport.CauseTypeId)).First();
            cptSortableSetImport_Table = gortContext.CauseParamType.Where(g => (g.Name == "Table") && (g.CauseTypeId == ctSortableSetImport.CauseTypeId)).First();
            cptSortableSetImport_Record = gortContext.CauseParamType.Where(g => (g.Name == "Record") && (g.CauseTypeId == ctSortableSetImport.CauseTypeId)).First();

            cptSortableSetAllForOrder_Order = gortContext.CauseParamType.Where(g => (g.Name == "Order") && (g.CauseTypeId == ctSortableSetAllForOrder.CauseTypeId)).First();

            cptSortableSetOrbit_Perm = gortContext.CauseParamType.Where(g => (g.Name == "Perm") && (g.CauseTypeId == ctSortableSetOrbit.CauseTypeId)).First();
            cptSortableSetOrbit_MaxLen = gortContext.CauseParamType.Where(g => (g.Name == "MaxLen") && (g.CauseTypeId == ctSortableSetOrbit.CauseTypeId)).First();

            cptSortableSetStacked_Orders = gortContext.CauseParamType.Where(g => (g.Name == "Orders") && (g.CauseTypeId == ctSortableSetStacked.CauseTypeId)).First();

            cptSorter_Order = gortContext.CauseParamType.Where(g => (g.Name == "Order") && (g.CauseTypeId == ctSorter.CauseTypeId)).First();
            cptSorter_Data = gortContext.CauseParamType.Where(g => (g.Name == "Data") && (g.CauseTypeId == ctSorter.CauseTypeId)).First();

            cptSorterImport_Workspace = gortContext.CauseParamType.Where(g => (g.Name == "Workspace") && (g.CauseTypeId == ctSorterImport.CauseTypeId)).First();
            cptSorterImport_Table = gortContext.CauseParamType.Where(g => (g.Name == "Table") && (g.CauseTypeId == ctSorterImport.CauseTypeId)).First();
            cptSorterImport_Record = gortContext.CauseParamType.Where(g => (g.Name == "Record") && (g.CauseTypeId == ctSorterImport.CauseTypeId)).First();
            cptSorterImport_Path = gortContext.CauseParamType.Where(g => (g.Name == "Path") && (g.CauseTypeId == ctSorterImport.CauseTypeId)).First();

            cptSorterSetRef_Source = gortContext.CauseParamType.Where(g => (g.Name == "Source") && (g.CauseTypeId == ctSorterSetRef.CauseTypeId)).First();

            cptSorterSetImport_Workspace = gortContext.CauseParamType.Where(g => (g.Name == "Workspace") && (g.CauseTypeId == ctSorterSetImport.CauseTypeId)).First();
            cptSorterSetImport_Table = gortContext.CauseParamType.Where(g => (g.Name == "Table") && (g.CauseTypeId == ctSorterSetImport.CauseTypeId)).First();
            cptSorterSetImport_Record = gortContext.CauseParamType.Where(g => (g.Name == "Record") && (g.CauseTypeId == ctSorterSetImport.CauseTypeId)).First();
            cptSorterSetImport_Path = gortContext.CauseParamType.Where(g => (g.Name == "Path") && (g.CauseTypeId == ctSorterSetImport.CauseTypeId)).First();

            cptSorterSetRndBySwitch_Order = gortContext.CauseParamType.Where(g => (g.Name == "Order") && (g.CauseTypeId == ctSorterSetRndBySwitch.CauseTypeId)).First();
            cptSorterSetRndBySwitch_SwitchCount = gortContext.CauseParamType.Where(g => (g.Name == "SwitchCount") && (g.CauseTypeId == ctSorterSetRndBySwitch.CauseTypeId)).First();
            cptSorterSetRndBySwitch_RndGen = gortContext.CauseParamType.Where(g => (g.Name == "RndGen") && (g.CauseTypeId == ctSorterSetRndBySwitch.CauseTypeId)).First();
            cptSorterSetRndBySwitch_SorterCount = gortContext.CauseParamType.Where(g => (g.Name == "SorterCount") && (g.CauseTypeId == ctSorterSetRndBySwitch.CauseTypeId)).First();

            cptSorterSetRndByStage_Order = gortContext.CauseParamType.Where(g => (g.Name == "Order") && (g.CauseTypeId == ctSorterSetRndByStage.CauseTypeId)).First();
            cptSorterSetRndByStage_RndGen = gortContext.CauseParamType.Where(g => (g.Name == "RndGen") && (g.CauseTypeId == ctSorterSetRndByStage.CauseTypeId)).First();
            cptSorterSetRndByStage_StageCount = gortContext.CauseParamType.Where(g => (g.Name == "StageCount") && (g.CauseTypeId == ctSorterSetRndByStage.CauseTypeId)).First();
            cptSorterSetRndByStage_SorterCount = gortContext.CauseParamType.Where(g => (g.Name == "SorterCount") && (g.CauseTypeId == ctSorterSetRndByStage.CauseTypeId)).First();

            cptSorterSetRndBySymmetricStage_Order = gortContext.CauseParamType.Where(g => (g.Name == "Order") && (g.CauseTypeId == ctSorterSetRndByRflStage.CauseTypeId)).First();
            cptSorterSetRndBySymmetricStage_StageCount = gortContext.CauseParamType.Where(g => (g.Name == "StageCount") && (g.CauseTypeId == ctSorterSetRndByRflStage.CauseTypeId)).First();
            cptSorterSetRndBySymmetricStage_RndGen = gortContext.CauseParamType.Where(g => (g.Name == "RndGen") && (g.CauseTypeId == ctSorterSetRndByRflStage.CauseTypeId)).First();
            cptSorterSetRndBySymmetricStage_SorterCount = gortContext.CauseParamType.Where(g => (g.Name == "SorterCount") && (g.CauseTypeId == ctSorterSetRndByRflStage.CauseTypeId)).First();

            cptSorterSetRndBySymmetricStageBuddies_Order = gortContext.CauseParamType.Where(g => (g.Name == "Order") && (g.CauseTypeId == ctSorterSetRndByRflStageBuddies.CauseTypeId)).First();
            cptSorterSetRndBySymmetricStageBuddies_StageCount = gortContext.CauseParamType.Where(g => (g.Name == "StageCount") && (g.CauseTypeId == ctSorterSetRndByRflStageBuddies.CauseTypeId)).First();
            cptSorterSetRndBySymmetricStageBuddies_RndGen = gortContext.CauseParamType.Where(g => (g.Name == "RndGen") && (g.CauseTypeId == ctSorterSetRndByRflStageBuddies.CauseTypeId)).First();
            cptSorterSetRndBySymmetricStageBuddies_SorterCount = gortContext.CauseParamType.Where(g => (g.Name == "SorterCount") && (g.CauseTypeId == ctSorterSetRndByRflStageBuddies.CauseTypeId)).First();
            cptSorterSetRndBySymmetricStageBuddies_BuddyCount = gortContext.CauseParamType.Where(g => (g.Name == "BuddyCount") && (g.CauseTypeId == ctSorterSetRndByRflStageBuddies.CauseTypeId)).First();

            cptSorterPerf_Sorter = gortContext.CauseParamType.Where(g => (g.Name == "Sorter") && (g.CauseTypeId == ctSorterPerf.CauseTypeId)).First();
            cptSorterPerf_SortableSet = gortContext.CauseParamType.Where(g => (g.Name == "SortableSet") && (g.CauseTypeId == ctSorterPerf.CauseTypeId)).First();

            cptSorterSetPerfBins_SorterSet = gortContext.CauseParamType.Where(g => (g.Name == "SorterSet") && (g.CauseTypeId == ctSorterSetPerfBins.CauseTypeId)).First();
            cptSorterSetPerfBins_SortableSet = gortContext.CauseParamType.Where(g => (g.Name == "SortableSet") && (g.CauseTypeId == ctSorterSetPerfBins.CauseTypeId)).First();
            cptSorterSetPerfBins_SorterSaveMode = gortContext.CauseParamType.Where(g => (g.Name == "SorterSaveMode") && (g.CauseTypeId == ctSorterSetPerfBins.CauseTypeId)).First();

        }


        public static void GetCauseRndGenSet(GortContext gortContext)
        {
            causeRndGenSetA = gortContext.Cause.Where(g => g.Description == "rndGenSet 123 10").FirstOrDefault();
            if (causeRndGenSetA is null) return;
            gortContext.Entry(causeRndGenSetA).Collection(c => c.CauseParams).Load();
            var csPs = causeRndGenSetA.CauseParams.ToList();
        }

        public static void GetCauseSortableSetAllForOrderA(GortContext gortContext)
        {
            causeSortableSetAllForOrderA = gortContext.Cause.Where(g => g.Description == "allSortables 16").FirstOrDefault();
            if (causeSortableSetAllForOrderA is null) return;
            gortContext.Entry(causeSortableSetAllForOrderA).Collection(c => c.CauseParams).Load();
            var csPs = causeSortableSetAllForOrderA.CauseParams.ToList();
        }

    }

}
