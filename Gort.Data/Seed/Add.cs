using System.Security.Cryptography;
using System.Text;

namespace Gort.Data.Seed
{
    internal static partial class CauseSeed
    {

        public static void AddCauseTypeGroups(IGortContext gortContext)
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


        public static void AddCauseTypes(IGortContext gortContext)
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


        public static void AddParamTypes(IGortContext gortContext)
        {
            ptRndGen_Seed = new ParamType() { Name = "Seed", DataType = DataType.Int }.AddId();
            ptRndGen_Type = new ParamType() { Name = "Type", DataType = DataType.Int }.AddId();
            ptRndGenId = new ParamType() { Name = "RndGenId", DataType = DataType.Guid }.AddId();
            ptRndGenCount = new ParamType() { Name = "RndGenCount", DataType = DataType.Int }.AddId();
            ptOrder = new ParamType() { Name = "Order", DataType = DataType.Int }.AddId();
            ptSortableFormat = new ParamType() { Name = "SortableFormat", DataType = DataType.Int }.AddId();
            ptSortableData = new ParamType() { Name = "SortableData", DataType = DataType.ByteArray }.AddId();
            ptWorkspaceId = new ParamType() { Name = "WorkspaceId", DataType = DataType.Guid }.AddId();
            ptTableName = new ParamType() { Name = "TableName", DataType = DataType.Guid }.AddId();
            ptRecordId = new ParamType() { Name = "RecordId", DataType = DataType.Guid }.AddId();
            ptRecordPath = new ParamType() { Name = "RecordPath", DataType = DataType.String }.AddId();
            ptSortableCount = new ParamType() { Name = "SortableCount", DataType = DataType.Int }.AddId();
            ptSortableSetOrbit_Perm = new ParamType() { Name = "SortableSetOrbitPerm", DataType = DataType.IntArray }.AddId();
            ptSortableSetOrbit_MaxCount = new ParamType() { Name = "SortableSetOrbitMaxCount", DataType = DataType.Int }.AddId();
            ptSortableSetStacked_Orders = new ParamType() { Name = "Orders", DataType = DataType.IntArray }.AddId();
            ptSorterData = new ParamType() { Name = "SorterData", DataType = DataType.ByteArray }.AddId();
            ptSorterSetRef_Source = new ParamType() { Name = "RefSource", DataType = DataType.String }.AddId();
            ptSwitchCount = new ParamType() { Name = "SwitchCount", DataType = DataType.Int }.AddId();
            ptStageCount = new ParamType() { Name = "StageCount", DataType = DataType.Int }.AddId();
            ptSorterCount = new ParamType() { Name = "SorterCount", DataType = DataType.Int }.AddId();
            ptStageBuddyCount = new ParamType() { Name = "StageBuddyCount", DataType = DataType.Int }.AddId();
            ptSorterId = new ParamType() { Name = "SorterId", DataType = DataType.Guid }.AddId();
            ptSortableSetId = new ParamType() { Name = "SortableSetId", DataType = DataType.Guid }.AddId();
            ptSorterSetId = new ParamType() { Name = "SorterSetId", DataType = DataType.Guid }.AddId();
            cptSorterSetPerfBins_SorterSaveMode = new ParamType() { Name = "SorterSaveMode", DataType = DataType.Int }.AddId();

            gortContext.ParamType.Add(ptRndGen_Seed);
            gortContext.ParamType.Add(ptRndGen_Type);
            gortContext.ParamType.Add(ptRndGenCount);
            gortContext.ParamType.Add(ptOrder);
            gortContext.ParamType.Add(ptSortableFormat);
            gortContext.ParamType.Add(ptSortableData);
            gortContext.ParamType.Add(ptWorkspaceId);
            gortContext.ParamType.Add(ptTableName);
            gortContext.ParamType.Add(ptRecordId);
            gortContext.ParamType.Add(ptRecordPath);
            gortContext.ParamType.Add(ptSortableCount);
            gortContext.ParamType.Add(ptSortableSetOrbit_Perm);
            gortContext.ParamType.Add(ptSortableSetOrbit_MaxCount);
            gortContext.ParamType.Add(ptSortableSetStacked_Orders);
            gortContext.ParamType.Add(ptSorterData);
            gortContext.ParamType.Add(ptSorterSetRef_Source);
            gortContext.ParamType.Add(ptSwitchCount);
            gortContext.ParamType.Add(ptStageCount);
            gortContext.ParamType.Add(ptSorterCount);
            gortContext.ParamType.Add(ptStageBuddyCount);
            gortContext.ParamType.Add(ptSorterId);
            gortContext.ParamType.Add(ptSortableSetId);
            gortContext.ParamType.Add(ptSorterSetId);
            gortContext.ParamType.Add(cptSorterSetPerfBins_SorterSaveMode);
        }

        public static void AddParamTypesToCauseTypes(IGortContext gortContext)
        {
            ctRandGen.ParamTypes.Add(ptRndGen_Seed);
            ctRandGen.ParamTypes.Add(ptRndGen_Type);

            ctRandGenSet.ParamTypes.Add(ptRndGenId);
            ctRandGenSet.ParamTypes.Add(ptRndGenCount);

            ctSortable.ParamTypes.Add(ptOrder);
            ctSortable.ParamTypes.Add(ptSortableFormat);
            ctSortable.ParamTypes.Add(ptSortableData);

            ctSortableImport.ParamTypes.Add(ptWorkspaceId);
            ctSortableImport.ParamTypes.Add(ptTableName);
            ctSortableImport.ParamTypes.Add(ptRecordId);
            ctSortableImport.ParamTypes.Add(ptRecordPath);

            ctSortableSet.ParamTypes.Add(ptOrder);
            ctSortableSet.ParamTypes.Add(ptSortableFormat);
            ctSortableSet.ParamTypes.Add(ptSortableData);
            ctSortableSet.ParamTypes.Add(ptSortableCount);

            ctSortableSetImport.ParamTypes.Add(ptWorkspaceId);
            ctSortableSetImport.ParamTypes.Add(ptTableName);
            ctSortableSetImport.ParamTypes.Add(ptRecordId);
            ctSortableSetImport.ParamTypes.Add(ptRecordPath);

            ctSortableSetAllForOrder.ParamTypes.Add(ptOrder);

            ctSortableSetOrbit.ParamTypes.Add(ptSortableSetOrbit_Perm);
            ctSortableSetOrbit.ParamTypes.Add(ptSortableSetOrbit_MaxCount);

            ctSortableSetStacked.ParamTypes.Add(ptSortableSetStacked_Orders);

            ctSorter.ParamTypes.Add(ptOrder);
            ctSorter.ParamTypes.Add(ptSorterData);

            ctSorterImport.ParamTypes.Add(ptWorkspaceId);
            ctSorterImport.ParamTypes.Add(ptTableName);
            ctSorterImport.ParamTypes.Add(ptRecordId);
            ctSorterImport.ParamTypes.Add(ptRecordPath);

            ctSorterSetRef.ParamTypes.Add(ptSorterSetRef_Source);
;
            ctSorterSetImport.ParamTypes.Add(ptWorkspaceId);
            ctSorterSetImport.ParamTypes.Add(ptTableName);
            ctSorterSetImport.ParamTypes.Add(ptRecordId);
            ctSorterSetImport.ParamTypes.Add(ptRecordPath);

            ctSorterSetRndBySwitch.ParamTypes.Add(ptOrder);
            ctSorterSetRndBySwitch.ParamTypes.Add(ptRndGenId);
            ctSorterSetRndBySwitch.ParamTypes.Add(ptSwitchCount);
            ctSorterSetRndBySwitch.ParamTypes.Add(ptSorterCount);

            ctSorterSetRndByStage.ParamTypes.Add(ptOrder);
            ctSorterSetRndByStage.ParamTypes.Add(ptRndGenId);
            ctSorterSetRndByStage.ParamTypes.Add(ptStageCount);
            ctSorterSetRndByStage.ParamTypes.Add(ptSorterCount);

            ctSorterSetRndByRflStage.ParamTypes.Add(ptOrder);
            ctSorterSetRndByRflStage.ParamTypes.Add(ptRndGenId);
            ctSorterSetRndByRflStage.ParamTypes.Add(ptStageCount);
            ctSorterSetRndByRflStage.ParamTypes.Add(ptSorterCount);

            ctSorterSetRndByRflStageBuddies.ParamTypes.Add(ptOrder);
            ctSorterSetRndByRflStageBuddies.ParamTypes.Add(ptRndGenId);
            ctSorterSetRndByRflStageBuddies.ParamTypes.Add(ptStageCount);
            ctSorterSetRndByRflStageBuddies.ParamTypes.Add(ptSorterCount);
            ctSorterSetRndByRflStageBuddies.ParamTypes.Add(ptStageBuddyCount);

            ctSorterPerf.ParamTypes.Add(ptSorterId);
            ctSorterPerf.ParamTypes.Add(ptSortableSetId);

            ctSorterSetPerfBins.ParamTypes.Add(ptSorterSetId);
            ctSorterSetPerfBins.ParamTypes.Add(ptSortableSetId);
            ctSorterSetPerfBins.ParamTypes.Add(cptSorterSetPerfBins_SorterSaveMode);
        }



        public static Cause AddCauseRndGen(IGortContext gortContext, CauseType ctRG, int seed, RndGenType rndGenType)
        {
            var descr = $"{ctRG.Name}_{rndGenType}_{seed}";
            causeRndGenA = new Cause() { Description = descr, CauseTypeID = ctRG.CauseTypeId, CauseStatus = CauseStatus.Pending, Index = 1, WorkspaceId = workspace1.WorkspaceId}.AddId();
            cpRndGen_Seed = new CauseParam() { CauseId = causeRndGenA.CauseId, ParamTypeId = ptRndGen_Seed.ParamTypeId, Value = BitConverter.GetBytes(123) }.AddId();
            cpRndGen_Type = new CauseParam() { CauseId = causeRndGenA.CauseId, ParamTypeId = ptRndGen_Type.ParamTypeId, Value = BitConverter.GetBytes((int)RndGenType.Lcg) }.AddId();
            causeRndGenA.CauseParams.Add(cpRndGen_Seed);
            causeRndGenA.CauseParams.Add(cpRndGen_Type);
            gortContext.Cause.Add(causeRndGenA);
            return causeRndGenA;
        }


        public static void AddCauseRndGenSet(IGortContext gortContext)
        {
            causeRndGenSetA = new Cause() { Description = "rndGenSet 123 10", CauseTypeID = ctRandGenSet.CauseTypeId, CauseStatus = CauseStatus.Pending, Index = 2, Workspace = workspace1 };
            cpRndGenSet_Rng = new CauseParam() { Cause = causeRndGenSetA, ParamType = ptRndGen_Seed, Value = causeRndGenA.CauseId.ToByteArray()}.AddId();
            cpRndGenSet_Count = new CauseParam() { Cause = causeRndGenSetA, ParamType = ptRndGenCount, Value = BitConverter.GetBytes(10) }.AddId();
            causeRndGenSetA.CauseParams.Add(cpRndGenSet_Rng);
            causeRndGenSetA.CauseParams.Add(cpRndGenSet_Count);
            gortContext.Cause.Add(causeRndGenSetA);
        }

        public static void AddCauseSortableSetAllForOrderA(IGortContext gortContext)
        {
            causeSortableSetAllForOrderA = new Cause() { Description = "allSortables 16", CauseTypeID = ctRandGenSet.CauseTypeId, CauseStatus = CauseStatus.Pending, Index = 3, Workspace = workspace1 };
            cpSortableSetAllForOrder_Order = new CauseParam() { Cause = causeSortableSetAllForOrderA, ParamType = ptOrder, Value = BitConverter.GetBytes(16) }.AddId();
            causeSortableSetAllForOrderA.CauseParams.Add(cpSortableSetAllForOrder_Order);
            gortContext.Cause.Add(causeSortableSetAllForOrderA);
        }
    }
}
