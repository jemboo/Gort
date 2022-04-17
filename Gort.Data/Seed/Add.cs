using System.Security.Cryptography;
using System.Text;

namespace Gort.Data.Seed
{
    public static partial class CauseSeed
    {

        public static void AddCauseTypeGroups(IGortContext gortContext)
        {
            ctgRoot = new CauseTypeGroup() { Name = CauseTypeGroupName.Root, Parent = null }.AddId();
            ctgRandGen = new CauseTypeGroup() { Name = CauseTypeGroupName.RandGen, Parent = ctgRoot }.AddId();
            ctgSortable = new CauseTypeGroup() { Name = CauseTypeGroupName.Sortable, Parent = ctgRoot }.AddId();
            ctgSortableSet = new CauseTypeGroup() { Name = CauseTypeGroupName.SortableSetDef, Parent = ctgRoot }.AddId();
            ctgSorter = new CauseTypeGroup() { Name = CauseTypeGroupName.Sorter, Parent = ctgRoot }.AddId();
            ctgSorterSet = new CauseTypeGroup() { Name = CauseTypeGroupName.SorterSetDef, Parent = ctgRoot }.AddId();
            ctgSorterSetRand = new CauseTypeGroup() { Name = CauseTypeGroupName.SorterSetRnd, Parent = ctgSorterSet }.AddId();
            ctgSorterPerf = new CauseTypeGroup() { Name = CauseTypeGroupName.SorterPerf, Parent = ctgRoot }.AddId();
            ctgSorterShc = new CauseTypeGroup() { Name = CauseTypeGroupName.SorterShc, Parent = ctgRoot }.AddId();

            gortContext.CauseTypeGroup.Add(ctgRoot);
            gortContext.CauseTypeGroup.Add(ctgRandGen);
            gortContext.CauseTypeGroup.Add(ctgSortable);
            gortContext.CauseTypeGroup.Add(ctgSortableSet);
            gortContext.CauseTypeGroup.Add(ctgSorter);
            gortContext.CauseTypeGroup.Add(ctgSorterSet);
            gortContext.CauseTypeGroup.Add(ctgSorterSetRand);
            gortContext.CauseTypeGroup.Add(ctgSorterPerf);
            gortContext.CauseTypeGroup.Add(ctgSorterShc);
        }


        public static void AddCauseTypes(IGortContext gortContext)
        {
            ctRandGen = new CauseType() { Name = CauseTypeName.Rng, CauseTypeGroup = ctgRandGen }.AddId();
            ctRandGenSet = new CauseType() { Name = CauseTypeName.RngSet, CauseTypeGroup = ctgRandGen }.AddId();

            //ctSortable = new CauseType() { Name = CauseTypeName.Sortable, CauseTypeGroup = ctgSortable }.AddId();
            ctSortableImport = new CauseType() { Name = CauseTypeName.SortableImport, CauseTypeGroup = ctgSortable }.AddId();

            //ctSortableSet = new CauseType() { Name = CauseTypeName.SortableSet, CauseTypeGroup = ctgSortableSet }.AddId();
            ctSortableSetImport = new CauseType() { Name = CauseTypeName.SortableSetImport, CauseTypeGroup = ctgSortableSet }.AddId();
            ctSortableSetAllForOrder = new CauseType() { Name = CauseTypeName.SortableSetAllForOrder, CauseTypeGroup = ctgSortableSet }.AddId();
            ctSortableSetOrbit = new CauseType() { Name = CauseTypeName.SortableSetOrbit, CauseTypeGroup = ctgSortableSet }.AddId();
            ctSortableSetStacked = new CauseType() { Name = CauseTypeName.SortableSetStacked, CauseTypeGroup = ctgSortableSet }.AddId();

            ctSorter = new CauseType() { Name = CauseTypeName.Sorter, CauseTypeGroup = ctgSorter }.AddId();
            ctSorterImport = new CauseType() { Name = CauseTypeName.SorterImport, CauseTypeGroup = ctgSorter }.AddId();

            ctSorterSetRef = new CauseType() { Name = CauseTypeName.SorterSet, CauseTypeGroup = ctgSorterSet }.AddId();
            ctSorterSetImport = new CauseType() { Name = CauseTypeName.SorterSetImport, CauseTypeGroup = ctgSorterSet }.AddId();

            ctSorterSetRndBySwitch = new CauseType() { Name = CauseTypeName.SorterSetRandBySwitch, CauseTypeGroup = ctgSorterSetRand }.AddId();
            ctSorterSetRndByStage = new CauseType() { Name = CauseTypeName.SorterSetRandByStage, CauseTypeGroup = ctgSorterSetRand }.AddId();
            ctSorterSetRndByRflStage = new CauseType() { Name = CauseTypeName.SorterSetRandByRflStage, CauseTypeGroup = ctgSorterSetRand }.AddId();
            ctSorterSetRndByRflStageBuddies = new CauseType() { Name = CauseTypeName.SorterSetRandByRflStageBuddies, CauseTypeGroup = ctgSorterSetRand }.AddId();

            ctSorterPerf = new CauseType() { Name = CauseTypeName.SorterPerf, CauseTypeGroup = ctgSorterPerf }.AddId();
            ctSorterSetPerfBins = new CauseType() { Name = CauseTypeName.SorterSetPerfBins, CauseTypeGroup = ctgSorterPerf }.AddId();


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

        public static void AddParamType(IGortContext gortContext, ParamTypeName ptn, DataType dt)
        {
            var p = new ParamType() { Name = ptn, DataType = dt }.AddId();
            gortContext.ParamType.Add(p);
        }

        public static void AddParamTypes(IGortContext gortContext)
        {
            AddParamType(gortContext, ParamTypeName.RngSeed, DataType.Int32);
            AddParamType(gortContext, ParamTypeName.RngType, DataType.Int32);
            AddParamType(gortContext, ParamTypeName.RndGenId, DataType.Guid);
            AddParamType(gortContext, ParamTypeName.RndGenCount, DataType.Int32);
            AddParamType(gortContext, ParamTypeName.Order, DataType.Int32);
            AddParamType(gortContext, ParamTypeName.SortableFormat, DataType.Int32);
            //AddParamType(gortContext, ParamTypeName.SortableData, DataType.ByteArray);
            AddParamType(gortContext, ParamTypeName.WorkspaceId, DataType.Guid);
            AddParamType(gortContext, ParamTypeName.TableName, DataType.String);
            AddParamType(gortContext, ParamTypeName.RecordId, DataType.Guid);
            AddParamType(gortContext, ParamTypeName.RecordPath, DataType.String);
            AddParamType(gortContext, ParamTypeName.SortableCount, DataType.Int32);
            AddParamType(gortContext, ParamTypeName.SortableSetOrbitPerm, DataType.IntArray);
            AddParamType(gortContext, ParamTypeName.SortableSetOrbitMaxCount, DataType.Int32);
            AddParamType(gortContext, ParamTypeName.SortableSetStacked, DataType.IntArray);
            //AddParamType(gortContext, ParamTypeName.SorterData, DataType.ByteArray);
            AddParamType(gortContext, ParamTypeName.SorterExtent, DataType.Int32);
            AddParamType(gortContext, ParamTypeName.StageCount, DataType.Int32);
            AddParamType(gortContext, ParamTypeName.SorterCount, DataType.Int32);
            AddParamType(gortContext, ParamTypeName.StageBuddyCount, DataType.Int32);
            AddParamType(gortContext, ParamTypeName.SorterId, DataType.Guid);
            AddParamType(gortContext, ParamTypeName.SortableSetId, DataType.Guid);
            AddParamType(gortContext, ParamTypeName.SorterSetId, DataType.Guid);
            AddParamType(gortContext, ParamTypeName.SorterSaveMode, DataType.Int32);
        }

        public static void AddParams(IGortContext gortContext)
        {


        }

        public static void AddParamTypesToCauseTypes(IGortContext gortContext)
        {
//            ctRandGen.CauseTypeParams.Add(ptRndGen_Seed);
//            ctRandGen.ParamTypes.Add(ptRndGen_Type);

//            ctRandGenSet.ParamTypes.Add(ptRndGenId);
//            ctRandGenSet.ParamTypes.Add(ptRndGenCount);

//            ctSortable.ParamTypes.Add(ptOrder);
//            ctSortable.ParamTypes.Add(ptSortableFormat);
//            ctSortable.ParamTypes.Add(ptSortableData);

//            ctSortableImport.ParamTypes.Add(ptWorkspaceId);
//            ctSortableImport.ParamTypes.Add(ptTableName);
//            ctSortableImport.ParamTypes.Add(ptRecordId);
//            ctSortableImport.ParamTypes.Add(ptRecordPath);

//            ctSortableSet.ParamTypes.Add(ptOrder);
//            ctSortableSet.ParamTypes.Add(ptSortableFormat);
//            ctSortableSet.ParamTypes.Add(ptSortableData);
//            ctSortableSet.ParamTypes.Add(ptSortableCount);

//            ctSortableSetImport.ParamTypes.Add(ptWorkspaceId);
//            ctSortableSetImport.ParamTypes.Add(ptTableName);
//            ctSortableSetImport.ParamTypes.Add(ptRecordId);
//            ctSortableSetImport.ParamTypes.Add(ptRecordPath);

//            ctSortableSetAllForOrder.ParamTypes.Add(ptOrder);

//            ctSortableSetOrbit.ParamTypes.Add(ptSortableSetOrbit_Perm);
//            ctSortableSetOrbit.ParamTypes.Add(ptSortableSetOrbit_MaxCount);

//            ctSortableSetStacked.ParamTypes.Add(ptSortableSetStacked_Orders);

//            ctSorter.ParamTypes.Add(ptOrder);
//            ctSorter.ParamTypes.Add(ptSorterData);

//            ctSorterImport.ParamTypes.Add(ptWorkspaceId);
//            ctSorterImport.ParamTypes.Add(ptTableName);
//            ctSorterImport.ParamTypes.Add(ptRecordId);
//            ctSorterImport.ParamTypes.Add(ptRecordPath);

//            ctSorterSetRef.ParamTypes.Add(ptSorterSetRef_Source);
//;
//            ctSorterSetImport.ParamTypes.Add(ptWorkspaceId);
//            ctSorterSetImport.ParamTypes.Add(ptTableName);
//            ctSorterSetImport.ParamTypes.Add(ptRecordId);
//            ctSorterSetImport.ParamTypes.Add(ptRecordPath);

//            ctSorterSetRndBySwitch.ParamTypes.Add(ptOrder);
//            ctSorterSetRndBySwitch.ParamTypes.Add(ptRndGenId);
//            ctSorterSetRndBySwitch.ParamTypes.Add(ptSwitchCount);
//            ctSorterSetRndBySwitch.ParamTypes.Add(ptSorterCount);

//            ctSorterSetRndByStage.ParamTypes.Add(ptOrder);
//            ctSorterSetRndByStage.ParamTypes.Add(ptRndGenId);
//            ctSorterSetRndByStage.ParamTypes.Add(ptStageCount);
//            ctSorterSetRndByStage.ParamTypes.Add(ptSorterCount);

//            ctSorterSetRndByRflStage.ParamTypes.Add(ptOrder);
//            ctSorterSetRndByRflStage.ParamTypes.Add(ptRndGenId);
//            ctSorterSetRndByRflStage.ParamTypes.Add(ptStageCount);
//            ctSorterSetRndByRflStage.ParamTypes.Add(ptSorterCount);

//            ctSorterSetRndByRflStageBuddies.ParamTypes.Add(ptOrder);
//            ctSorterSetRndByRflStageBuddies.ParamTypes.Add(ptRndGenId);
//            ctSorterSetRndByRflStageBuddies.ParamTypes.Add(ptStageCount);
//            ctSorterSetRndByRflStageBuddies.ParamTypes.Add(ptSorterCount);
//            ctSorterSetRndByRflStageBuddies.ParamTypes.Add(ptStageBuddyCount);

//            ctSorterPerf.ParamTypes.Add(ptSorterId);
//            ctSorterPerf.ParamTypes.Add(ptSortableSetId);

//            ctSorterSetPerfBins.ParamTypes.Add(ptSorterSetId);
//            ctSorterSetPerfBins.ParamTypes.Add(ptSortableSetId);
//            ctSorterSetPerfBins.ParamTypes.Add(cptSorterSetPerfBins_SorterSaveMode);
        }

        public static Param AddIntParam(IGortContext gortContext, ParamTypeName paramTypeName, int val)
        {
            try
            {
                var paramType = MetaDataUtils.GetParamType(paramTypeName, gortContext);
                var bVal = BitConverter.GetBytes(val);
                var pram = new Param { ParamTypeId = paramType.ParamTypeId, Value = bVal}.AddId();
                gortContext.Param.Add(pram);
                return pram;
            }
            catch (Exception ex)
            {
                throw new Exception($"{paramTypeName} not found in ParamType", ex);
            }
        }

        public static Cause AddCauseRndGen(IGortContext gortContext, CauseType ctRG, int seed, RndGenType rndGenType)
        {
            var descr = $"{ctRG.Name}_{rndGenType}_{seed}";
            causeRndGenA = new Cause() { Description = descr, CauseTypeID = ctRG.CauseTypeId, CauseStatus = CauseStatus.Pending, Index = 1, WorkspaceId = workspace1.WorkspaceId}.AddId();
            //cpRndGen_Seed = new CauseParam() { CauseId = causeRndGenA.CauseId, CauseTypeParamId = ptRndGen_Seed.ParamTypeId, Value = BitConverter.GetBytes(123) }.AddId();
            //cpRndGen_Type = new CauseParam() { CauseId = causeRndGenA.CauseId, CauseTypeParamId = ptRndGen_Type.ParamTypeId, Value = BitConverter.GetBytes((int)RndGenType.Lcg) }.AddId();
            //causeRndGenA.CauseParams.Add(cpRndGen_Seed);
            //causeRndGenA.CauseParams.Add(cpRndGen_Type);
            gortContext.Cause.Add(causeRndGenA);
            return causeRndGenA;
        }


        public static void AddCauseRndGenSet(IGortContext gortContext)
        {
            causeRndGenSetA = new Cause() { Description = "rndGenSet 123 10", CauseTypeID = ctRandGenSet.CauseTypeId, CauseStatus = CauseStatus.Pending, Index = 2, Workspace = workspace1 };
            //cpRndGenSet_Rng = new CauseParam() { Cause = causeRndGenSetA, CauseTypeParam = ptRndGen_Seed, Value = causeRndGenA.CauseId.ToByteArray()}.AddId();
            //cpRndGenSet_Count = new CauseParam() { Cause = causeRndGenSetA, CauseTypeParam = ptRndGenCount, Value = BitConverter.GetBytes(10) }.AddId();
            //causeRndGenSetA.CauseParams.Add(cpRndGenSet_Rng);
            //causeRndGenSetA.CauseParams.Add(cpRndGenSet_Count);
            gortContext.Cause.Add(causeRndGenSetA);
        }

        public static void AddCauseSortableSetAllForOrderA(IGortContext gortContext)
        {
            causeSortableSetAllForOrderA = new Cause() { Description = "allSortables 16", CauseTypeID = ctRandGenSet.CauseTypeId, CauseStatus = CauseStatus.Pending, Index = 3, Workspace = workspace1 };
            //cpSortableSetAllForOrder_Order = new CauseParam() { Cause = causeSortableSetAllForOrderA, CauseTypeParam = ptOrder, Value = BitConverter.GetBytes(16) }.AddId();
            //causeSortableSetAllForOrderA.CauseParams.Add(cpSortableSetAllForOrder_Order);
            gortContext.Cause.Add(causeSortableSetAllForOrderA);
        }
    }
}
