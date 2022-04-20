using System.Security.Cryptography;
using System.Text;

namespace Gort.Data.Seed
{
    public static partial class CauseSeed
    {
        public static void GetCauseTypeGroups(IGortContext gortContext)
        {
            ctgRoot = gortContext.CauseTypeGroup.Where(g => g.Name == CauseTypeGroupName.Root).First();
            ctgRandGen = gortContext.CauseTypeGroup.Where(g => g.Name == CauseTypeGroupName.RandGen).First();
            ctgSortable = gortContext.CauseTypeGroup.Where(g => g.Name == CauseTypeGroupName.Sortable).First();
            ctgSortableSet = gortContext.CauseTypeGroup.Where(g => g.Name == CauseTypeGroupName.SortableSetDef).First();
            ctgSorter = gortContext.CauseTypeGroup.Where(g => g.Name == CauseTypeGroupName.Sorter).First();
            ctgSorterSet = gortContext.CauseTypeGroup.Where(g => g.Name == CauseTypeGroupName.SorterSetDef).First();
            ctgSorterSetRand = gortContext.CauseTypeGroup.Where(g => g.Name == CauseTypeGroupName.SorterSetRnd).First();
            ctgSorterPerf = gortContext.CauseTypeGroup.Where(g => g.Name == CauseTypeGroupName.SorterPerf).First();
            ctgSorterShc = gortContext.CauseTypeGroup.Where(g => g.Name == CauseTypeGroupName.SorterShc).First();

            //gortContext.CauseTypeGroup.Attach(ctgRoot);
            //gortContext.CauseTypeGroup.Attach(ctgRandGen);
            //gortContext.CauseTypeGroup.Attach(ctgSortable);
            //gortContext.CauseTypeGroup.Attach(ctgSortableSet);
            //gortContext.CauseTypeGroup.Attach(ctgSorter);
            //gortContext.CauseTypeGroup.Attach(ctgSorterSet);
            //gortContext.CauseTypeGroup.Attach(ctgSorterSetRand);
            //gortContext.CauseTypeGroup.Attach(ctgSorterPerf);
            //gortContext.CauseTypeGroup.Attach(ctgSorterShc);
        }

        public static void GetCauseTypes(IGortContext gortContext)
        {
            ctRandGen = gortContext.CauseType.Where(g => g.Name == CauseTypeName.Rng).First();
            ctRandGenSet = gortContext.CauseType.Where(g => g.Name == CauseTypeName.RngSet).First();

           // ctSortable = gortContext.CauseType.Where(g => g.Name == CauseTypeName.Sortable).First();
            ctSortableImport = gortContext.CauseType.Where(g => g.Name == CauseTypeName.SortableImport).First();

            //ctSortableSet = gortContext.CauseType.Where(g => g.Name == CauseTypeName.SortableSet).First();
            ctSortableSetImport = gortContext.CauseType.Where(g => g.Name == CauseTypeName.SortableSetImport).First();
            ctSortableSetAllForOrder = gortContext.CauseType.Where(g => g.Name == CauseTypeName.SortableSetAllForOrder).First();
            ctSortableSetOrbit = gortContext.CauseType.Where(g => g.Name == CauseTypeName.SortableSetOrbit).First();
            ctSortableSetStacked = gortContext.CauseType.Where(g => g.Name == CauseTypeName.SortableSetStacked).First();

            //ctSorter = gortContext.CauseType.Where(g => g.Name == CauseTypeName.Sorter).First();
            ctSorterImport = gortContext.CauseType.Where(g => g.Name == CauseTypeName.SorterImport).First();

            //ctSorterSetRef = gortContext.CauseType.Where(g => g.Name == CauseTypeName.SorterSet).First();
            ctSorterSetImport = gortContext.CauseType.Where(g => g.Name == CauseTypeName.SorterSetImport).First();

            ctSorterSetRndBySwitch = gortContext.CauseType.Where(g => g.Name == CauseTypeName.SorterSetRandBySwitch).First();
            ctSorterSetRndByStage = gortContext.CauseType.Where(g => g.Name == CauseTypeName.SorterSetRandByStage).First();
            ctSorterSetRndByRflStage = gortContext.CauseType.Where(g => g.Name == CauseTypeName.SorterSetRandByRflStage).First();
            ctSorterSetRndByRflStageBuddies = gortContext.CauseType.Where(g => g.Name == CauseTypeName.SorterSetRandByRflStageBuddies).First();

            ctSorterPerf = gortContext.CauseType.Where(g => g.Name == CauseTypeName.SorterPerf).First();
            ctSorterSetPerfBins = gortContext.CauseType.Where(g => g.Name == CauseTypeName.SorterSetPerfBins).First();

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
            ptRndGen_Seed = gortContext.ParamType.Where(g => (g.Name == ParamTypeName.RngSeed)).First();
            ptRndGen_Type = gortContext.ParamType.Where(g => (g.Name == ParamTypeName.RngType)).First();
            ptRndGenId = gortContext.ParamType.Where(g => (g.Name == ParamTypeName.RngId)).First();
            ptRndGenCount = gortContext.ParamType.Where(g => (g.Name == ParamTypeName.RngCount)).First();
            ptOrder = gortContext.ParamType.Where(g => (g.Name == ParamTypeName.Order)).First();
            ptSortableFormat = gortContext.ParamType.Where(g => (g.Name == ParamTypeName.SortableFormat)).First();
            //ptSortableData = gortContext.ParamType.Where(g => (g.Name == ParamTypeName.SortableData)).First();
            ptWorkspaceId = gortContext.ParamType.Where(g => (g.Name == ParamTypeName.WorkspaceId)).First();
            ptTableName = gortContext.ParamType.Where(g => (g.Name == ParamTypeName.TableName)).First();
            ptRecordId = gortContext.ParamType.Where(g => (g.Name == ParamTypeName.RecordId)).First();
            ptRecordPath = gortContext.ParamType.Where(g => (g.Name == ParamTypeName.RecordPath)).First();
            ptSortableCount = gortContext.ParamType.Where(g => (g.Name == ParamTypeName.SortableCount)).First();
            ptSortableSetOrbit_Perm = gortContext.ParamType.Where(g => (g.Name == ParamTypeName.SortableSetOrbitPerm)).First();
            ptSortableSetOrbit_MaxCount = gortContext.ParamType.Where(g => (g.Name == ParamTypeName.SortableSetOrbitMaxCount)).First();
            ptSortableSetStacked_OrderStack = gortContext.ParamType.Where(g => (g.Name == ParamTypeName.OrderStack)).First();
            //ptSorterData = gortContext.ParamType.Where(g => (g.Name == ParamTypeName.SorterData)).First();
            //ptSorterSetRef_Source = gortContext.ParamType.Where(g => (g.Name == ParamTypeName.RefSource)).First();
            ptSwitchCount = gortContext.ParamType.Where(g => (g.Name == ParamTypeName.SorterExtent)).First();
            ptStageCount = gortContext.ParamType.Where(g => (g.Name == ParamTypeName.StageCount)).First();
            ptSorterCount = gortContext.ParamType.Where(g => (g.Name == ParamTypeName.SorterCount)).First();
            ptStageBuddyCount = gortContext.ParamType.Where(g => (g.Name == ParamTypeName.StageBuddyCount)).First();
            ptSorterId = gortContext.ParamType.Where(g => (g.Name == ParamTypeName.SorterId)).First();
            ptSortableSetId = gortContext.ParamType.Where(g => (g.Name == ParamTypeName.SortableSetId)).First();
            ptSorterSetId = gortContext.ParamType.Where(g => (g.Name == ParamTypeName.SorterSetId)).First();
            cptSorterSetPerfBins_SorterSaveMode = gortContext.ParamType.Where(g => (g.Name == ParamTypeName.SorterSaveMode)).First();
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
