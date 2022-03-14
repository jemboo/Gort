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
            causeTypeRandGen = gortContext.CauseType.Where(g => g.Name == "randGen").First();
            causeTypeRandGenSet = gortContext.CauseType.Where(g => g.Name == "randGenSet").First();

            causeTypeSortable = gortContext.CauseType.Where(g => g.Name == "sortable").First();
            causeTypeSortableImport = gortContext.CauseType.Where(g => g.Name == "sortableImport").First();

            causeTypeSortableSet = gortContext.CauseType.Where(g => g.Name == "sortableSet").First();
            causeTypeSortableSetImport = gortContext.CauseType.Where(g => g.Name == "sortableSetImport").First();
            causeTypeSortableSetAllForOrder = gortContext.CauseType.Where(g => g.Name == "sortableSetAllForOrder").First();
            causeTypeSortableSetOrbit = gortContext.CauseType.Where(g => g.Name == "sortableSetOrbit").First();
            causeTypeSortableSetStacked = gortContext.CauseType.Where(g => g.Name == "sortableSetStacked").First();

            causeTypeSorter = gortContext.CauseType.Where(g => g.Name == "sorter").First();
            causeTypeSorterImport = gortContext.CauseType.Where(g => g.Name == "sorterImport").First();

            causeTypeSorterSetRef = gortContext.CauseType.Where(g => g.Name == "sorterSet").First();
            causeTypeSorterSetImport = gortContext.CauseType.Where(g => g.Name == "sorterSetImport").First();

            causeTypeSorterSetRndBySwitch = gortContext.CauseType.Where(g => g.Name == "sorterSetRandBySwitch").First();
            causeTypeSorterSetRndByStage = gortContext.CauseType.Where(g => g.Name == "sorterSetRandByStage").First();
            causeTypeSorterSetRndByRflStage = gortContext.CauseType.Where(g => g.Name == "sorterSetRandByRflStage").First();
            causeTypeSorterSetRndByRflStageBuddies = gortContext.CauseType.Where(g => g.Name == "sorterSetRandByRflStageBuddies").First();

            causeTypeSorterPerf = gortContext.CauseType.Where(g => g.Name == "sorterPerf").First();
            causeTypeSorterSetPerfBins = gortContext.CauseType.Where(g => g.Name == "sorterSetPerfBins").First();

            gortContext.CauseType.Attach(causeTypeRandGen);
            gortContext.CauseType.Attach(causeTypeRandGenSet);

            gortContext.CauseType.Attach(causeTypeSortable);
            gortContext.CauseType.Attach(causeTypeSortableImport);

            gortContext.CauseType.Attach(causeTypeSortableSet);
            gortContext.CauseType.Attach(causeTypeSortableSetImport);
            gortContext.CauseType.Attach(causeTypeSortableSetAllForOrder);
            gortContext.CauseType.Attach(causeTypeSortableSetOrbit);
            gortContext.CauseType.Attach(causeTypeSortableSetStacked);

            gortContext.CauseType.Attach(causeTypeSorter);
            gortContext.CauseType.Attach(causeTypeSorterImport);

            gortContext.CauseType.Attach(causeTypeSorterSetRef);
            gortContext.CauseType.Attach(causeTypeSorterSetImport);

            gortContext.CauseType.Attach(causeTypeSorterSetRndBySwitch);
            gortContext.CauseType.Attach(causeTypeSorterSetRndByStage);
            gortContext.CauseType.Attach(causeTypeSorterSetRndByRflStage);
            gortContext.CauseType.Attach(causeTypeSorterSetRndByRflStageBuddies);

            gortContext.CauseType.Attach(causeTypeSorterPerf);
            gortContext.CauseType.Attach(causeTypeSorterSetPerfBins);
        }

        public static void GetCauseTypeParams(GortContext gortContext)
        {
            causeTypeParamRndGen_Seed = gortContext.CauseTypeParam.Where(g => (g.Name == "Seed") && (g.CauseTypeId == causeTypeRandGen.CauseTypeId)).First();
            causeTypeParamRndGen_Type = gortContext.CauseTypeParam.Where(g => (g.Name == "Type") && (g.CauseTypeId == causeTypeRandGen.CauseTypeId)).First();

            causeTypeParamRndGenSet_Seed = gortContext.CauseTypeParam.Where(g => (g.Name == "Seed") && (g.CauseTypeId == causeTypeRandGenSet.CauseTypeId)).First();
            causeTypeParamRndGenSet_Type = gortContext.CauseTypeParam.Where(g => (g.Name == "Type") && (g.CauseTypeId == causeTypeRandGenSet.CauseTypeId)).First();
            causeTypeParamRndGenSet_Count = gortContext.CauseTypeParam.Where(g => (g.Name == "Count") && (g.CauseTypeId == causeTypeRandGenSet.CauseTypeId)).First();

            causeTypeParamSortable_Order = gortContext.CauseTypeParam.Where(g => (g.Name == "Order") && (g.CauseTypeId == causeTypeSortable.CauseTypeId)).First();
            causeTypeParamSortable_Format = gortContext.CauseTypeParam.Where(g => (g.Name == "Format") && (g.CauseTypeId == causeTypeSortable.CauseTypeId)).First();
            causeTypeParamSortable_Data = gortContext.CauseTypeParam.Where(g => (g.Name == "Data") && (g.CauseTypeId == causeTypeSortable.CauseTypeId)).First();

            causeTypeParamSortableImport_Workspace = gortContext.CauseTypeParam.Where(g => (g.Name == "Workspace") && (g.CauseTypeId == causeTypeSortableImport.CauseTypeId)).First();
            causeTypeParamSortableImport_Table = gortContext.CauseTypeParam.Where(g => (g.Name == "Table") && (g.CauseTypeId == causeTypeSortableImport.CauseTypeId)).First();
            causeTypeParamSortableImport_Record = gortContext.CauseTypeParam.Where(g => (g.Name == "Record") && (g.CauseTypeId == causeTypeSortableImport.CauseTypeId)).First();
            causeTypeParamSortableImport_Path = gortContext.CauseTypeParam.Where(g => (g.Name == "Path") && (g.CauseTypeId == causeTypeSortableImport.CauseTypeId)).First();

            causeTypeParamSortableSet_Order = gortContext.CauseTypeParam.Where(g => (g.Name == "Order") && (g.CauseTypeId == causeTypeSortableSet.CauseTypeId)).First();
            causeTypeParamSortableSet_Count = gortContext.CauseTypeParam.Where(g => (g.Name == "Count") && (g.CauseTypeId == causeTypeSortableSet.CauseTypeId)).First();
            causeTypeParamSortableSet_Format = gortContext.CauseTypeParam.Where(g => (g.Name == "Format") && (g.CauseTypeId == causeTypeSortableSet.CauseTypeId)).First();
            causeTypeParamSortableSet_Data = gortContext.CauseTypeParam.Where(g => (g.Name == "Data") && (g.CauseTypeId == causeTypeSortableSet.CauseTypeId)).First();

            causeTypeParamSortableSetImport_Workspace = gortContext.CauseTypeParam.Where(g => (g.Name == "Workspace") && (g.CauseTypeId == causeTypeSortableSetImport.CauseTypeId)).First();
            causeTypeParamSortableSetImport_Table = gortContext.CauseTypeParam.Where(g => (g.Name == "Table") && (g.CauseTypeId == causeTypeSortableSetImport.CauseTypeId)).First();
            causeTypeParamSortableSetImport_Record = gortContext.CauseTypeParam.Where(g => (g.Name == "Record") && (g.CauseTypeId == causeTypeSortableSetImport.CauseTypeId)).First();

            causeTypeParamSortableSetAllForOrder_Order = gortContext.CauseTypeParam.Where(g => (g.Name == "Order") && (g.CauseTypeId == causeTypeSortableSetAllForOrder.CauseTypeId)).First();

            causeTypeParamSortableSetOrbit_Perm = gortContext.CauseTypeParam.Where(g => (g.Name == "Perm") && (g.CauseTypeId == causeTypeSortableSetOrbit.CauseTypeId)).First();
            causeTypeParamSortableSetOrbit_MaxLen = gortContext.CauseTypeParam.Where(g => (g.Name == "MaxLen") && (g.CauseTypeId == causeTypeSortableSetOrbit.CauseTypeId)).First();

            causeTypeParamSortableSetStacked_Orders = gortContext.CauseTypeParam.Where(g => (g.Name == "Orders") && (g.CauseTypeId == causeTypeSortableSetStacked.CauseTypeId)).First();

            causeTypeParamSorter_Order = gortContext.CauseTypeParam.Where(g => (g.Name == "Order") && (g.CauseTypeId == causeTypeSorter.CauseTypeId)).First();
            causeTypeParamSorter_Data = gortContext.CauseTypeParam.Where(g => (g.Name == "Data") && (g.CauseTypeId == causeTypeSorter.CauseTypeId)).First();

            causeTypeParamSorterImport_Workspace = gortContext.CauseTypeParam.Where(g => (g.Name == "Workspace") && (g.CauseTypeId == causeTypeSorterImport.CauseTypeId)).First();
            causeTypeParamSorterImport_Table = gortContext.CauseTypeParam.Where(g => (g.Name == "Table") && (g.CauseTypeId == causeTypeSorterImport.CauseTypeId)).First();
            causeTypeParamSorterImport_Record = gortContext.CauseTypeParam.Where(g => (g.Name == "Record") && (g.CauseTypeId == causeTypeSorterImport.CauseTypeId)).First();
            causeTypeParamSorterImport_Path = gortContext.CauseTypeParam.Where(g => (g.Name == "Path") && (g.CauseTypeId == causeTypeSorterImport.CauseTypeId)).First();

            causeTypeParamSorterSetRef_Source = gortContext.CauseTypeParam.Where(g => (g.Name == "Source") && (g.CauseTypeId == causeTypeSorterSetRef.CauseTypeId)).First();

            causeTypeParamSorterSetImport_Workspace = gortContext.CauseTypeParam.Where(g => (g.Name == "Workspace") && (g.CauseTypeId == causeTypeSorterSetImport.CauseTypeId)).First();
            causeTypeParamSorterSetImport_Table = gortContext.CauseTypeParam.Where(g => (g.Name == "Table") && (g.CauseTypeId == causeTypeSorterSetImport.CauseTypeId)).First();
            causeTypeParamSorterSetImport_Record = gortContext.CauseTypeParam.Where(g => (g.Name == "Record") && (g.CauseTypeId == causeTypeSorterSetImport.CauseTypeId)).First();
            causeTypeParamSorterSetImport_Path = gortContext.CauseTypeParam.Where(g => (g.Name == "Path") && (g.CauseTypeId == causeTypeSorterSetImport.CauseTypeId)).First();

            causeTypeParamSorterSetRndBySwitch_Order = gortContext.CauseTypeParam.Where(g => (g.Name == "Order") && (g.CauseTypeId == causeTypeSorterSetRndBySwitch.CauseTypeId)).First();
            causeTypeParamSorterSetRndBySwitch_SwitchCount = gortContext.CauseTypeParam.Where(g => (g.Name == "SwitchCount") && (g.CauseTypeId == causeTypeSorterSetRndBySwitch.CauseTypeId)).First();
            causeTypeParamSorterSetRndBySwitch_RndGen = gortContext.CauseTypeParam.Where(g => (g.Name == "RndGen") && (g.CauseTypeId == causeTypeSorterSetRndBySwitch.CauseTypeId)).First();
            causeTypeParamSorterSetRndBySwitch_SorterCount = gortContext.CauseTypeParam.Where(g => (g.Name == "SorterCount") && (g.CauseTypeId == causeTypeSorterSetRndBySwitch.CauseTypeId)).First();

            causeTypeParamSorterSetRndByStage_Order = gortContext.CauseTypeParam.Where(g => (g.Name == "Order") && (g.CauseTypeId == causeTypeSorterSetRndByStage.CauseTypeId)).First();
            causeTypeParamSorterSetRndByStage_RndGen = gortContext.CauseTypeParam.Where(g => (g.Name == "RndGen") && (g.CauseTypeId == causeTypeSorterSetRndByStage.CauseTypeId)).First();
            causeTypeParamSorterSetRndByStage_StageCount = gortContext.CauseTypeParam.Where(g => (g.Name == "StageCount") && (g.CauseTypeId == causeTypeSorterSetRndByStage.CauseTypeId)).First();
            causeTypeParamSorterSetRndByStage_SorterCount = gortContext.CauseTypeParam.Where(g => (g.Name == "SorterCount") && (g.CauseTypeId == causeTypeSorterSetRndByStage.CauseTypeId)).First();

            causeTypeParamSorterSetRndBySymmetricStage_Order = gortContext.CauseTypeParam.Where(g => (g.Name == "Order") && (g.CauseTypeId == causeTypeSorterSetRndByRflStage.CauseTypeId)).First();
            causeTypeParamSorterSetRndBySymmetricStage_StageCount = gortContext.CauseTypeParam.Where(g => (g.Name == "StageCount") && (g.CauseTypeId == causeTypeSorterSetRndByRflStage.CauseTypeId)).First();
            causeTypeParamSorterSetRndBySymmetricStage_RndGen = gortContext.CauseTypeParam.Where(g => (g.Name == "RndGen") && (g.CauseTypeId == causeTypeSorterSetRndByRflStage.CauseTypeId)).First();
            causeTypeParamSorterSetRndBySymmetricStage_SorterCount = gortContext.CauseTypeParam.Where(g => (g.Name == "SorterCount") && (g.CauseTypeId == causeTypeSorterSetRndByRflStage.CauseTypeId)).First();

            causeTypeParamSorterSetRndBySymmetricStageBuddies_Order = gortContext.CauseTypeParam.Where(g => (g.Name == "Order") && (g.CauseTypeId == causeTypeSorterSetRndByRflStageBuddies.CauseTypeId)).First();
            causeTypeParamSorterSetRndBySymmetricStageBuddies_StageCount = gortContext.CauseTypeParam.Where(g => (g.Name == "StageCount") && (g.CauseTypeId == causeTypeSorterSetRndByRflStageBuddies.CauseTypeId)).First();
            causeTypeParamSorterSetRndBySymmetricStageBuddies_RndGen = gortContext.CauseTypeParam.Where(g => (g.Name == "RndGen") && (g.CauseTypeId == causeTypeSorterSetRndByRflStageBuddies.CauseTypeId)).First();
            causeTypeParamSorterSetRndBySymmetricStageBuddies_SorterCount = gortContext.CauseTypeParam.Where(g => (g.Name == "SorterCount") && (g.CauseTypeId == causeTypeSorterSetRndByRflStageBuddies.CauseTypeId)).First();
            causeTypeParamSorterSetRndBySymmetricStageBuddies_BuddyCount = gortContext.CauseTypeParam.Where(g => (g.Name == "BuddyCount") && (g.CauseTypeId == causeTypeSorterSetRndByRflStageBuddies.CauseTypeId)).First();

            causeTypeParamSorterPerf_Sorter = gortContext.CauseTypeParam.Where(g => (g.Name == "Sorter") && (g.CauseTypeId == causeTypeSorterPerf.CauseTypeId)).First();
            causeTypeParamSorterPerf_SortableSet = gortContext.CauseTypeParam.Where(g => (g.Name == "SortableSet") && (g.CauseTypeId == causeTypeSorterPerf.CauseTypeId)).First();

            causeTypeParamSorterSetPerfBins_SorterSet = gortContext.CauseTypeParam.Where(g => (g.Name == "SorterSet") && (g.CauseTypeId == causeTypeSorterSetPerfBins.CauseTypeId)).First();
            causeTypeParamSorterSetPerfBins_SortableSet = gortContext.CauseTypeParam.Where(g => (g.Name == "SortableSet") && (g.CauseTypeId == causeTypeSorterSetPerfBins.CauseTypeId)).First();
            causeTypeParamSorterSetPerfBins_SorterSaveMode = gortContext.CauseTypeParam.Where(g => (g.Name == "SorterSaveMode") && (g.CauseTypeId == causeTypeSorterSetPerfBins.CauseTypeId)).First();

        }


        public static void GetCauseRndGenSet(GortContext gortContext)
        {
            causeRndGenSetA = gortContext.Cause.Where(g => g.Description == "rndGenSet 123 10").First();
            gortContext.Entry(causeRndGenSetA).Collection(c => c.CauseParams).Load();
            var csPs = causeRndGenSetA.CauseParams.ToList();
            //gortContext.Workspace.Attach(workspace1);
        }


    }

}
