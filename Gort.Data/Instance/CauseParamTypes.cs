namespace Gort.Data.Instance
{
    public static class CauseParamTypes
    {
        static CauseParamTypes()
        {
            Rng_RngSeed = MakeCauseTypeParam(CauseTypes.Rng, CauseParamTypeName.RngSeed, ParamTypes.RngSeed);
            Rng_RngType = MakeCauseTypeParam(CauseTypes.Rng, CauseParamTypeName.RngType, ParamTypes.RngType);

            RngSet_RndGenId = MakeCauseTypeParam(CauseTypes.RngSet, CauseParamTypeName.RndGenId, ParamTypes.RndGenId);
            RngSet_RndGenCount = MakeCauseTypeParam(CauseTypes.RngSet, CauseParamTypeName.RndGenCount, ParamTypes.RndGenCount);

            SortableImport_WorkspaceId = MakeCauseTypeParam(CauseTypes.SortableImport, CauseParamTypeName.WorkspaceId, ParamTypes.WorkspaceId);
            SortableImport_TableName = MakeCauseTypeParam(CauseTypes.SortableImport, CauseParamTypeName.TableName, ParamTypes.TableName);
            SortableImport_RecordId = MakeCauseTypeParam(CauseTypes.SortableImport, CauseParamTypeName.RecordId, ParamTypes.RecordId);
            SortableImport_RecordPath = MakeCauseTypeParam(CauseTypes.SortableImport, CauseParamTypeName.RecordPath, ParamTypes.RecordPath);

            SortableSetImport_WorkspaceId = MakeCauseTypeParam(CauseTypes.SortableSetImport, CauseParamTypeName.WorkspaceId, ParamTypes.WorkspaceId);
            SortableSetImport_TableName = MakeCauseTypeParam(CauseTypes.SortableSetImport, CauseParamTypeName.TableName, ParamTypes.TableName);
            SortableSetImport_RecordId = MakeCauseTypeParam(CauseTypes.SortableSetImport, CauseParamTypeName.RecordId, ParamTypes.RecordId);
            SortableSetImport_RecordPath = MakeCauseTypeParam(CauseTypes.SortableSetImport, CauseParamTypeName.RecordPath, ParamTypes.RecordPath);

            SortableSetRand_Order = MakeCauseTypeParam(CauseTypes.SortableSetRand, CauseParamTypeName.Order, ParamTypes.Order);
            SortableSetRand_RngId = MakeCauseTypeParam(CauseTypes.SortableSetRand, CauseParamTypeName.RndGenId, ParamTypes.RndGenId);
            SortableSetRand_SortableCount = MakeCauseTypeParam(CauseTypes.SortableSetRand, CauseParamTypeName.SortableCount, ParamTypes.RecordId);
            SortableSetRand_SortableFormat = MakeCauseTypeParam(CauseTypes.SortableSetRand, CauseParamTypeName.SortableFormat, ParamTypes.SortableFormat);

            SortableSetAllForOrder_Order = MakeCauseTypeParam(CauseTypes.SortableSetAllForOrder, CauseParamTypeName.Order, ParamTypes.Order);

            SortableSetOrbit_Order = MakeCauseTypeParam(CauseTypes.SortableSetOrbit, CauseParamTypeName.Order, ParamTypes.Order);
            SortableSetOrbit_Perm = MakeCauseTypeParam(CauseTypes.SortableSetOrbit, CauseParamTypeName.SortableSetOrbitPerm, ParamTypes.SortableSetOrbitPerm);
            SortableSetOrbit_MaxCount = MakeCauseTypeParam(CauseTypes.SortableSetOrbit, CauseParamTypeName.SortableSetOrbitMaxCount, ParamTypes.SortableSetOrbitMaxCount);

            SortableSetStacked_Orders = MakeCauseTypeParam(CauseTypes.SortableSetStacked, CauseParamTypeName.SortableSetStackedOrders, ParamTypes.SortableSetStacked);

            SorterImport_WorkspaceId = MakeCauseTypeParam(CauseTypes.SorterImport, CauseParamTypeName.WorkspaceId, ParamTypes.WorkspaceId);
            SorterImport_TableName = MakeCauseTypeParam(CauseTypes.SorterImport, CauseParamTypeName.TableName, ParamTypes.TableName);
            SorterImport_RecordId = MakeCauseTypeParam(CauseTypes.SorterImport, CauseParamTypeName.RecordId, ParamTypes.RecordId);
            SorterImport_RecordPath = MakeCauseTypeParam(CauseTypes.SorterImport, CauseParamTypeName.RecordPath, ParamTypes.RecordPath);

            SorterSetImport_WorkspaceId = MakeCauseTypeParam(CauseTypes.SorterSetImport, CauseParamTypeName.WorkspaceId, ParamTypes.WorkspaceId);
            SorterSetImport_TableName = MakeCauseTypeParam(CauseTypes.SorterSetImport, CauseParamTypeName.TableName, ParamTypes.TableName);
            SorterSetImport_RecordId = MakeCauseTypeParam(CauseTypes.SorterSetImport, CauseParamTypeName.RecordId, ParamTypes.RecordId);
            SorterSetImport_RecordPath = MakeCauseTypeParam(CauseTypes.SorterSetImport, CauseParamTypeName.RecordPath, ParamTypes.RecordPath);

            SwitchListImport_WorkspaceId = MakeCauseTypeParam(CauseTypes.SwitchListImport, CauseParamTypeName.WorkspaceId, ParamTypes.WorkspaceId);
            SwitchListImport_TableName = MakeCauseTypeParam(CauseTypes.SwitchListImport, CauseParamTypeName.TableName, ParamTypes.TableName);
            SwitchListImport_RecordId = MakeCauseTypeParam(CauseTypes.SwitchListImport, CauseParamTypeName.RecordId, ParamTypes.RecordId);
            SwitchListImport_RecordPath = MakeCauseTypeParam(CauseTypes.SwitchListImport, CauseParamTypeName.RecordPath, ParamTypes.RecordPath);
            SwitchListImport_StartPosition = MakeCauseTypeParam(CauseTypes.SwitchListImport, CauseParamTypeName.SwitchSequenceStartPos, ParamTypes.SwitchCount);
            SwitchListImport_SequenceLength = MakeCauseTypeParam(CauseTypes.SwitchListImport, CauseParamTypeName.SwitchSequenceLength, ParamTypes.SwitchCount);



            Order = MakeCauseTypeParam(CauseTypes.SortableSetAllForOrder, CauseParamTypeName.Order, ParamTypes.Order);
            SortableFormat = MakeCauseTypeParam(CauseTypes.SortableSetAllForOrder, CauseParamTypeName.SortableFormat, ParamTypes.SortableFormat);
            TableName = MakeCauseTypeParam(CauseTypes.SortableImport, CauseParamTypeName.TableName, ParamTypes.TableName);
            RecordId = MakeCauseTypeParam(CauseTypes.SortableImport, CauseParamTypeName.RecordId, ParamTypes.RecordId);
            SortableSetId = MakeCauseTypeParam(CauseTypes.SorterSetPerfBins, CauseParamTypeName.SortableSetId, ParamTypes.SortableSetId);
            RecordPath = MakeCauseTypeParam(CauseTypes.SorterSetImport, CauseParamTypeName.RecordPath, ParamTypes.RecordPath);

            SwitchCount = MakeCauseTypeParam(CauseTypes.SorterSetRandBySwitch, CauseParamTypeName.SwitchCount, ParamTypes.SwitchCount);
            SorterCount = MakeCauseTypeParam(CauseTypes.SorterSetRandBySwitch, CauseParamTypeName.SorterCount, ParamTypes.SorterCount);
            StageCount = MakeCauseTypeParam(CauseTypes.SorterSetRandByStage, CauseParamTypeName.StageCount, ParamTypes.StageCount);
            StageBuddyCount = MakeCauseTypeParam(CauseTypes.SorterSetRandByRflStageBuddies, CauseParamTypeName.StageBuddyCount, ParamTypes.StageBuddyCount);
            SorterId = MakeCauseTypeParam(CauseTypes.SorterPerf, CauseParamTypeName.SorterId, ParamTypes.SorterId);
            SorterSetName = MakeCauseTypeParam(CauseTypes.SorterGroupName, CauseParamTypeName.SorterSetName, ParamTypes.SorterSetName);
            SorterSetId = MakeCauseTypeParam(CauseTypes.SorterSetPerfBins, CauseParamTypeName.SorterSetId, ParamTypes.SorterSetId);
            SorterSaveMode = MakeCauseTypeParam(CauseTypes.SorterSetRandByStage, CauseParamTypeName.SorterSaveMode, ParamTypes.SorterSaveMode);
        }

        public static CauseParamType Rng_RngSeed { get; private set; }
        public static CauseParamType Rng_RngType { get; private set; }

        public static CauseParamType RngSet_RndGenId { get; private set; }
        public static CauseParamType RngSet_RndGenCount { get; private set; }

        public static CauseParamType SortableImport_WorkspaceId { get; private set; }
        public static CauseParamType SortableImport_TableName { get; private set; }
        public static CauseParamType SortableImport_RecordId { get; private set; }
        public static CauseParamType SortableImport_RecordPath { get; private set; }

        public static CauseParamType SortableSetImport_WorkspaceId { get; private set; }
        public static CauseParamType SortableSetImport_TableName { get; private set; }
        public static CauseParamType SortableSetImport_RecordId { get; private set; }
        public static CauseParamType SortableSetImport_RecordPath { get; private set; }

        public static CauseParamType SorterImport_WorkspaceId { get; private set; }
        public static CauseParamType SorterImport_TableName { get; private set; }
        public static CauseParamType SorterImport_RecordId { get; private set; }
        public static CauseParamType SorterImport_RecordPath { get; private set; }

        public static CauseParamType SorterSetImport_WorkspaceId { get; private set; }
        public static CauseParamType SorterSetImport_TableName { get; private set; }
        public static CauseParamType SorterSetImport_RecordId { get; private set; }
        public static CauseParamType SorterSetImport_RecordPath { get; private set; }

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

        public static CauseParamType SorterShc_SorterId { get; private set; }
        public static CauseParamType SorterShc_SortableSetId { get; private set; }
        public static CauseParamType SorterShc_MutationRate { get; private set; }
        public static CauseParamType SorterShc_Temperature { get; private set; }
        public static CauseParamType SorterShc_MaxGeneration { get; private set; }






        public static CauseParamType WorkspaceId { get; private set; }

        public static CauseParamType Order { get; private set; }
        public static CauseParamType SortableFormat { get; private set; }
        public static CauseParamType TableName { get; private set; }
        public static CauseParamType RecordId { get; private set; }
        public static CauseParamType RecordPath { get; private set; }
        public static CauseParamType SortableSetId { get; private set; }
        public static CauseParamType SwitchCount { get; private set; }
        public static CauseParamType SorterCount { get; private set; }
        public static CauseParamType StageCount { get; private set; }
        public static CauseParamType StageBuddyCount { get; private set; }
        public static CauseParamType SorterId { get; private set; }
        public static CauseParamType SorterSetName { get; private set; }
        public static CauseParamType SorterSetId { get; private set; }
        public static CauseParamType SorterSaveMode { get; private set; }

        static CauseParamType MakeCauseTypeParam(CauseType causeType, CauseParamTypeName ptn, ParamType paramType)
        {
            return new CauseParamType() { CauseTypeId = causeType.CauseTypeId, Name = ptn, ParamTypeId = paramType.ParamTypeId }.AddId();
        }
    }
}
