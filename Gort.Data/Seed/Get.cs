using System.Security.Cryptography;
using System.Text;

namespace Gort.Data.Seed
{
    internal static partial class CauseSeed
    {


        public static void GetCauseTypeGroups(IGortContext gortContext)
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

        public static void GetCauseTypes(IGortContext gortContext)
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

        public static void GetParamTypes(IGortContext gortContext)
        {
            ptRndGen_Seed = gortContext.ParamType.Where(g => (g.Name == "Seed")).First();
            ptRndGen_Type = gortContext.ParamType.Where(g => (g.Name == "Type")).First();
            ptRndGenId = gortContext.ParamType.Where(g => (g.Name == "RndGenId")).First();
            ptRndGenCount = gortContext.ParamType.Where(g => (g.Name == "RndGenCount")).First();
            ptOrder = gortContext.ParamType.Where(g => (g.Name == "Order")).First();
            ptSortableFormat = gortContext.ParamType.Where(g => (g.Name == "SortableFormat")).First();
            ptSortableData = gortContext.ParamType.Where(g => (g.Name == "SortableData")).First();
            ptWorkspaceId = gortContext.ParamType.Where(g => (g.Name == "WorkspaceId")).First();
            ptTableName = gortContext.ParamType.Where(g => (g.Name == "TableName")).First();
            ptRecordId = gortContext.ParamType.Where(g => (g.Name == "RecordId")).First();
            ptRecordPath = gortContext.ParamType.Where(g => (g.Name == "RecordPath")).First();
            ptSortableCount = gortContext.ParamType.Where(g => (g.Name == "SortableCount")).First();
            ptSortableSetOrbit_Perm = gortContext.ParamType.Where(g => (g.Name == "SortableSetOrbitPerm")).First();
            ptSortableSetOrbit_MaxCount = gortContext.ParamType.Where(g => (g.Name == "SortableSetOrbitMaxCount")).First();
            ptSortableSetStacked_Orders = gortContext.ParamType.Where(g => (g.Name == "Orders")).First();
            ptSorterData = gortContext.ParamType.Where(g => (g.Name == "SorterData")).First();
            ptSorterSetRef_Source = gortContext.ParamType.Where(g => (g.Name == "RefSource")).First();
            ptSwitchCount = gortContext.ParamType.Where(g => (g.Name == "SwitchCount")).First();
            ptStageCount = gortContext.ParamType.Where(g => (g.Name == "StageCount")).First();
            ptSorterCount = gortContext.ParamType.Where(g => (g.Name == "SorterCount")).First();
            ptStageBuddyCount = gortContext.ParamType.Where(g => (g.Name == "StageBuddyCount")).First();
            ptSorterId = gortContext.ParamType.Where(g => (g.Name == "SorterId")).First();
            ptSortableSetId = gortContext.ParamType.Where(g => (g.Name == "SortableSetId")).First();
            ptSorterSetId = gortContext.ParamType.Where(g => (g.Name == "SorterSetId")).First();
            cptSorterSetPerfBins_SorterSaveMode = gortContext.ParamType.Where(g => (g.Name == "SorterSaveMode")).First();
        }


        public static void GetCauseRndGenSet(IGortContext gortContext)
        {
            causeRndGenSetA = gortContext.Cause.Where(g => g.Description == "rndGenSet 123 10").FirstOrDefault();
            if (causeRndGenSetA is null) return;
            gortContext.Entry<Cause>(causeRndGenSetA).Collection(c => c.CauseParams).Load();
            var csPs = causeRndGenSetA.CauseParams.ToList();
        }

        public static void GetCauseSortableSetAllForOrderA(IGortContext gortContext)
        {
            causeSortableSetAllForOrderA = gortContext.Cause.Where(g => g.Description == "allSortables 16").FirstOrDefault();
            if (causeSortableSetAllForOrderA is null) return;
            gortContext.Entry<Cause>(causeSortableSetAllForOrderA).Collection(c => c.CauseParams).Load();
            var csPs = causeSortableSetAllForOrderA.CauseParams.ToList();
        }

    }

}
