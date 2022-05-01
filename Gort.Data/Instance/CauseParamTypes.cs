using Gort.Data.DataModel;
using Gort.Data.Utils;

namespace Gort.Data.Instance
{
    public static class CauseParamTypes
    {
        static CauseParamTypes()
        {
            Rng_RngSeed = MakeCauseTypeParam(CauseTypes.Rng, CauseParamTypeName.RngSeed, ParamTypes.RngSeed);
            Rng_RngType = MakeCauseTypeParam(CauseTypes.Rng, CauseParamTypeName.RngType, ParamTypes.RngType);

            RngSet_RndGenId = MakeCauseTypeParam(CauseTypes.RngSet, CauseParamTypeName.RngId, ParamTypes.RngId);
            RngSet_RndGenCount = MakeCauseTypeParam(CauseTypes.RngSet, CauseParamTypeName.RngCount, ParamTypes.RngCount);

            SorterMutation_SorterId = MakeCauseTypeParam(CauseTypes.SorterMutation, CauseParamTypeName.SorterId, ParamTypes.SorterId);
            SorterMutation_SorterMutationId = MakeCauseTypeParam(CauseTypes.SorterMutation, CauseParamTypeName.SorterMutationId, ParamTypes.SorterMutationId);
            SorterMutation_SorterCount = MakeCauseTypeParam(CauseTypes.SorterMutation, CauseParamTypeName.SorterCount, ParamTypes.SorterCount);
            SorterMutation_RngId = MakeCauseTypeParam(CauseTypes.SorterMutation, CauseParamTypeName.RngId, ParamTypes.RngId);

            SortableImport_WorkspaceId = MakeCauseTypeParam(CauseTypes.SortableImport, CauseParamTypeName.WorkspaceId, ParamTypes.WorkspaceId);
            SortableImport_TableName = MakeCauseTypeParam(CauseTypes.SortableImport, CauseParamTypeName.TableName, ParamTypes.TableName);
            SortableImport_RecordId = MakeCauseTypeParam(CauseTypes.SortableImport, CauseParamTypeName.RecordId, ParamTypes.RecordId);
            SortableImport_RecordPath = MakeCauseTypeParam(CauseTypes.SortableImport, CauseParamTypeName.RecordPath, ParamTypes.RecordPath);

            SortableSetImport_WorkspaceId = MakeCauseTypeParam(CauseTypes.SortableSetImport, CauseParamTypeName.WorkspaceId, ParamTypes.WorkspaceId);
            SortableSetImport_TableName = MakeCauseTypeParam(CauseTypes.SortableSetImport, CauseParamTypeName.TableName, ParamTypes.TableName);
            SortableSetImport_RecordId = MakeCauseTypeParam(CauseTypes.SortableSetImport, CauseParamTypeName.RecordId, ParamTypes.RecordId);
            SortableSetImport_RecordPath = MakeCauseTypeParam(CauseTypes.SortableSetImport, CauseParamTypeName.RecordPath, ParamTypes.RecordPath);

            SortableSetRand_Order = MakeCauseTypeParam(CauseTypes.SortableSetRand, CauseParamTypeName.Order, ParamTypes.Order);
            SortableSetRand_RngId = MakeCauseTypeParam(CauseTypes.SortableSetRand, CauseParamTypeName.RngId, ParamTypes.RngId);
            SortableSetRand_SortableCount = MakeCauseTypeParam(CauseTypes.SortableSetRand, CauseParamTypeName.SortableCount, ParamTypes.RecordId);
            SortableSetRand_SortableFormat = MakeCauseTypeParam(CauseTypes.SortableSetRand, CauseParamTypeName.SortableFormat, ParamTypes.SortableFormat);

            SortableSetAllForOrder_Order = MakeCauseTypeParam(CauseTypes.SortableSetAllForOrder, CauseParamTypeName.Order, ParamTypes.Order);

            SortableSetOrbit_Order = MakeCauseTypeParam(CauseTypes.SortableSetOrbit, CauseParamTypeName.Order, ParamTypes.Order);
            SortableSetOrbit_Perm = MakeCauseTypeParam(CauseTypes.SortableSetOrbit, CauseParamTypeName.SortableSetOrbitPerm, ParamTypes.SortableSetOrbitPerm);
            SortableSetOrbit_MaxCount = MakeCauseTypeParam(CauseTypes.SortableSetOrbit, CauseParamTypeName.SortableSetOrbitMaxCount, ParamTypes.SortableSetOrbitMaxCount);

            SortableSetStacked_Orders = MakeCauseTypeParam(CauseTypes.SortableSetStacked, CauseParamTypeName.OrderStack, ParamTypes.OrderStack);

            SorterGroupName_SorterGroupName = MakeCauseTypeParam(CauseTypes.SorterGroupName, CauseParamTypeName.SorterGroupName, ParamTypes.SorterGroupName);

            SorterImport_WorkspaceId = MakeCauseTypeParam(CauseTypes.SorterImport, CauseParamTypeName.WorkspaceId, ParamTypes.WorkspaceId);
            SorterImport_TableName = MakeCauseTypeParam(CauseTypes.SorterImport, CauseParamTypeName.TableName, ParamTypes.TableName);
            SorterImport_RecordId = MakeCauseTypeParam(CauseTypes.SorterImport, CauseParamTypeName.RecordId, ParamTypes.RecordId);
            SorterImport_RecordPath = MakeCauseTypeParam(CauseTypes.SorterImport, CauseParamTypeName.RecordPath, ParamTypes.RecordPath);

            SorterSetImport_WorkspaceId = MakeCauseTypeParam(CauseTypes.SorterSetImport, CauseParamTypeName.WorkspaceId, ParamTypes.WorkspaceId);
            SorterSetImport_TableName = MakeCauseTypeParam(CauseTypes.SorterSetImport, CauseParamTypeName.TableName, ParamTypes.TableName);
            SorterSetImport_RecordId = MakeCauseTypeParam(CauseTypes.SorterSetImport, CauseParamTypeName.RecordId, ParamTypes.RecordId);
            SorterSetImport_RecordPath = MakeCauseTypeParam(CauseTypes.SorterSetImport, CauseParamTypeName.RecordPath, ParamTypes.RecordPath);

            SorterSetRandBySwitch_RngId = MakeCauseTypeParam(CauseTypes.SorterSetRandBySwitch, CauseParamTypeName.RngId, ParamTypes.RngId);
            SorterSetRandBySwitch_Order = MakeCauseTypeParam(CauseTypes.SorterSetRandBySwitch, CauseParamTypeName.Order, ParamTypes.Order);
            SorterSetRandBySwitch_SwitchLength = MakeCauseTypeParam(CauseTypes.SorterSetRandBySwitch, CauseParamTypeName.SwitchCount, ParamTypes.SwitchCount);
            SorterSetRandBySwitch_SorterCount = MakeCauseTypeParam(CauseTypes.SorterSetRandBySwitch, CauseParamTypeName.SorterCount, ParamTypes.SorterCount);

            SorterSetRandByStage_RngId = MakeCauseTypeParam(CauseTypes.SorterSetRandByStage, CauseParamTypeName.RngId, ParamTypes.RngId);
            SorterSetRandByStage_Order = MakeCauseTypeParam(CauseTypes.SorterSetRandByStage, CauseParamTypeName.Order, ParamTypes.Order);
            SorterSetRandByStage_StageLength = MakeCauseTypeParam(CauseTypes.SorterSetRandByStage, CauseParamTypeName.StageCount, ParamTypes.StageCount);
            SorterSetRandByStage_SorterCount = MakeCauseTypeParam(CauseTypes.SorterSetRandByStage, CauseParamTypeName.SorterCount, ParamTypes.SorterCount);

            SorterSetRandByRflStage_RngId = MakeCauseTypeParam(CauseTypes.SorterSetRandByRflStage, CauseParamTypeName.RngId, ParamTypes.RngId);
            SorterSetRandByRflStage_Order = MakeCauseTypeParam(CauseTypes.SorterSetRandByRflStage, CauseParamTypeName.Order, ParamTypes.Order);
            SorterSetRandByRflStage_StageLength = MakeCauseTypeParam(CauseTypes.SorterSetRandByRflStage, CauseParamTypeName.StageCount, ParamTypes.StageCount);
            SorterSetRandByRflStage_SorterCount = MakeCauseTypeParam(CauseTypes.SorterSetRandByRflStage, CauseParamTypeName.SorterCount, ParamTypes.SorterCount);

            SorterSetRandByRflStageBuddies_RngId = MakeCauseTypeParam(CauseTypes.SorterSetRandByRflStageBuddies, CauseParamTypeName.RngId, ParamTypes.RngId);
            SorterSetRandByRflStageBuddies_Order = MakeCauseTypeParam(CauseTypes.SorterSetRandByRflStageBuddies, CauseParamTypeName.Order, ParamTypes.Order);
            SorterSetRandByRflStageBuddies_StageLength = MakeCauseTypeParam(CauseTypes.SorterSetRandByRflStageBuddies, CauseParamTypeName.StageCount, ParamTypes.StageCount);
            SorterSetRandByRflStageBuddies_StageBuddyCount = MakeCauseTypeParam(CauseTypes.SorterSetRandByRflStageBuddies, CauseParamTypeName.StageBuddyCount, ParamTypes.StageBuddyCount);
            SorterSetRandByRflStageBuddies_SorterCount = MakeCauseTypeParam(CauseTypes.SorterSetRandByRflStageBuddies, CauseParamTypeName.SorterCount, ParamTypes.SorterCount);

            SorterSetRandByMutation_RngId = MakeCauseTypeParam(CauseTypes.SorterSetRandByMutation, CauseParamTypeName.RngId, ParamTypes.RngId);
            SorterSetRandByMutation_SorterId = MakeCauseTypeParam(CauseTypes.SorterSetRandByMutation, CauseParamTypeName.SorterId, ParamTypes.SorterId);
            SorterSetRandByMutation_SorterMutationId = MakeCauseTypeParam(CauseTypes.SorterSetRandByMutation, CauseParamTypeName.SorterMutationId, ParamTypes.SorterMutationId);
            SorterSetRandByMutation_SorterCount = MakeCauseTypeParam(CauseTypes.SorterSetRandByMutation, CauseParamTypeName.SorterCount, ParamTypes.SorterCount);

            SwitchListImport_WorkspaceId = MakeCauseTypeParam(CauseTypes.SwitchListImport, CauseParamTypeName.WorkspaceId, ParamTypes.WorkspaceId);
            SwitchListImport_TableName = MakeCauseTypeParam(CauseTypes.SwitchListImport, CauseParamTypeName.TableName, ParamTypes.TableName);
            SwitchListImport_RecordId = MakeCauseTypeParam(CauseTypes.SwitchListImport, CauseParamTypeName.RecordId, ParamTypes.RecordId);
            SwitchListImport_RecordPath = MakeCauseTypeParam(CauseTypes.SwitchListImport, CauseParamTypeName.RecordPath, ParamTypes.RecordPath);
            SwitchListImport_StartPosition = MakeCauseTypeParam(CauseTypes.SwitchListImport, CauseParamTypeName.SorterPosition, ParamTypes.SorterPosition);
            SwitchListImport_SequenceLength = MakeCauseTypeParam(CauseTypes.SwitchListImport, CauseParamTypeName.SwitchCount, ParamTypes.SwitchCount);

            SorterPerf_SorterId = MakeCauseTypeParam(CauseTypes.SorterPerf, CauseParamTypeName.SorterId, ParamTypes.SorterId);
            SorterPerf_SortableSetId = MakeCauseTypeParam(CauseTypes.SorterPerf, CauseParamTypeName.SortableSetId, ParamTypes.SortableSetId);

            SorterSetPerf_SorterSetId = MakeCauseTypeParam(CauseTypes.SorterSetPerf, CauseParamTypeName.SorterSetId, ParamTypes.SorterSetId);
            SorterSetPerf_SortableSetId = MakeCauseTypeParam(CauseTypes.SorterSetPerf, CauseParamTypeName.SortableSetId, ParamTypes.SortableSetId);

            SorterShc_SorterId = MakeCauseTypeParam(CauseTypes.SorterShc, CauseParamTypeName.SorterId, ParamTypes.SorterId);
            SorterShc_SortableSetId = MakeCauseTypeParam(CauseTypes.SorterShc, CauseParamTypeName.SortableSetId, ParamTypes.SortableSetId);
            SorterShc_MutationId = MakeCauseTypeParam(CauseTypes.SorterShc, CauseParamTypeName.SorterMutationId, ParamTypes.SorterMutationId);
            SorterShc_Temperature = MakeCauseTypeParam(CauseTypes.SorterShc, CauseParamTypeName.Temperature, ParamTypes.Temperature);
            SorterShc_MaxGeneration = MakeCauseTypeParam(CauseTypes.SorterShc, CauseParamTypeName.Generation, ParamTypes.Generation);
        }


        public static CauseParamType Rng_RngSeed { get; private set; }
        public static CauseParamType Rng_RngType { get; private set; }

        public static CauseParamType RngSet_RndGenId { get; private set; }
        public static CauseParamType RngSet_RndGenCount { get; private set; }

        public static CauseParamType SorterMutation_SorterId { get; private set; }
        public static CauseParamType SorterMutation_SorterMutationId { get; private set; }
        public static CauseParamType SorterMutation_SorterCount { get; private set; }
        public static CauseParamType SorterMutation_RngId { get; private set; }

        public static CauseParamType SortableImport_WorkspaceId { get; private set; }
        public static CauseParamType SortableImport_TableName { get; private set; }
        public static CauseParamType SortableImport_RecordId { get; private set; }
        public static CauseParamType SortableImport_RecordPath { get; private set; }

        public static CauseParamType SortableSetImport_WorkspaceId { get; private set; }
        public static CauseParamType SortableSetImport_TableName { get; private set; }
        public static CauseParamType SortableSetImport_RecordId { get; private set; }
        public static CauseParamType SortableSetImport_RecordPath { get; private set; }

        public static CauseParamType SwitchListImport_WorkspaceId { get; private set; }
        public static CauseParamType SwitchListImport_TableName { get; private set; }
        public static CauseParamType SwitchListImport_RecordId { get; private set; }
        public static CauseParamType SwitchListImport_RecordPath { get; private set; }
        public static CauseParamType SwitchListImport_StartPosition { get; private set; }
        public static CauseParamType SwitchListImport_SequenceLength { get; private set; }

        public static CauseParamType SortableSetRand_Order { get; private set; }
        public static CauseParamType SortableSetRand_RngId { get; private set; }
        public static CauseParamType SortableSetRand_SortableCount { get; private set; }
        public static CauseParamType SortableSetRand_SortableFormat { get; private set; }

        public static CauseParamType SortableSetAllForOrder_Order { get; private set; }

        public static CauseParamType SortableSetOrbit_Order { get; private set; }
        public static CauseParamType SortableSetOrbit_Perm { get; private set; }
        public static CauseParamType SortableSetOrbit_MaxCount { get; private set; }

        public static CauseParamType SortableSetStacked_Orders { get; private set; }

        public static CauseParamType SorterGroupName_SorterGroupName { get; private set; }

        public static CauseParamType SorterImport_WorkspaceId { get; private set; }
        public static CauseParamType SorterImport_TableName { get; private set; }
        public static CauseParamType SorterImport_RecordId { get; private set; }
        public static CauseParamType SorterImport_RecordPath { get; private set; }

        public static CauseParamType SorterSetImport_WorkspaceId { get; private set; }
        public static CauseParamType SorterSetImport_TableName { get; private set; }
        public static CauseParamType SorterSetImport_RecordId { get; private set; }
        public static CauseParamType SorterSetImport_RecordPath { get; private set; }


        public static CauseParamType SorterSetRandBySwitch_RngId { get; private set; }
        public static CauseParamType SorterSetRandBySwitch_Order { get; private set; }
        public static CauseParamType SorterSetRandBySwitch_SwitchLength { get; private set; }
        public static CauseParamType SorterSetRandBySwitch_SorterCount { get; private set; }


        public static CauseParamType SorterSetRandByStage_RngId { get; private set; }
        public static CauseParamType SorterSetRandByStage_Order { get; private set; }
        public static CauseParamType SorterSetRandByStage_StageLength { get; private set; }
        public static CauseParamType SorterSetRandByStage_SorterCount { get; private set; }


        public static CauseParamType SorterSetRandByRflStage_RngId { get; private set; }
        public static CauseParamType SorterSetRandByRflStage_Order { get; private set; }
        public static CauseParamType SorterSetRandByRflStage_StageLength { get; private set; }
        public static CauseParamType SorterSetRandByRflStage_SorterCount { get; private set; }


        public static CauseParamType SorterSetRandByRflStageBuddies_RngId { get; private set; }
        public static CauseParamType SorterSetRandByRflStageBuddies_Order { get; private set; }
        public static CauseParamType SorterSetRandByRflStageBuddies_StageLength { get; private set; }
        public static CauseParamType SorterSetRandByRflStageBuddies_StageBuddyCount { get; private set; }
        public static CauseParamType SorterSetRandByRflStageBuddies_SorterCount { get; private set; }


        public static CauseParamType SorterSetRandByMutation_RngId { get; private set; }
        public static CauseParamType SorterSetRandByMutation_SorterId { get; private set; }
        public static CauseParamType SorterSetRandByMutation_SorterMutationId { get; private set; }
        public static CauseParamType SorterSetRandByMutation_SorterCount { get; private set; }




        public static CauseParamType SorterPerf_SorterId { get; private set; }
        public static CauseParamType SorterPerf_SortableSetId { get; private set; }


        public static CauseParamType SorterSetPerf_SorterSetId { get; private set; }
        public static CauseParamType SorterSetPerf_SortableSetId { get; private set; }


        public static CauseParamType SorterShc_SorterId { get; private set; }
        public static CauseParamType SorterShc_SortableSetId { get; private set; }
        public static CauseParamType SorterShc_MutationId { get; private set; }
        public static CauseParamType SorterShc_Temperature { get; private set; }
        public static CauseParamType SorterShc_MaxGeneration { get; private set; }


        static CauseParamType MakeCauseTypeParam(CauseType causeType, CauseParamTypeName ptn, ParamType paramType)
        {
            var ctp = new CauseParamType() { CauseTypeId = causeType.CauseTypeId, Name = ptn.ToString(), ParamTypeId = paramType.ParamTypeId }.AddId();
            _members.Add(ctp);
            return ctp;
        }

        private static readonly List<CauseParamType> _members = new List<CauseParamType>();   
        public static IEnumerable<CauseParamType> Members
        {
            get { return _members; }
        }
    }
}
