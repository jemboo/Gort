namespace Gort.Data.DataModel
{

    public enum CauseParamTypeName
    {
        Generation,
        MutationRate,
        MutationType,
        NumberFormat,
        Order,
        OrderStack,
        RecordId,
        RecordPath,
        RandGenCount,
        RandGenId,
        RandGenSeed,
        RandGenType,
        SortableCount,
        SortableFormat,
        SortableSetId,
        SortableSetOrbitMaxCount,
        SortableSetOrbitPerm,
        SortableSetRep,
        SorterCount,
        SorterExtent,
        SorterId,
        SorterMutationId,
        SorterPosition,
        SorterSaveMode,
        SorterSetId,
        SorterGroupName,
        StageBuddyCount,
        StageCount,
        SwitchCount,
        SwitchOrStage,
        TableName,
        Temperature,
        WorkspaceId
    }

    public enum CauseStatus
    {
        Pending = 0,
        InProgress = 1,
        Complete = 2,
        Error = 3
    }

    public enum CauseTypeGroupName
    {
        Root,
        Utils,
        Sortable,
        SortableSetDef,
        SortableSetRnd,
        Sorter,
        SorterSetDef,
        SorterSetRnd,
        SwitchList,
        SorterPerf,
        SorterShc
    }

    public enum CauseTypeName
    {
        Rng,
        RngSet,
        SorterMutation,
        SortableImport,
        SortableSetImport,
        SortableSetRand,
        SortableSetAllForOrder,
        SortableSetOrbit,
        SortableSetStacked,
        SorterGroupName,
        SorterImport,
        SorterSetImport,
        SorterSetRandBySwitch,
        SorterSetRandByStage,
        SorterSetRandByRflStage,
        SorterSetRandByRflStageBuddies,
        SorterSetRandByMutation,
        SwitchListImport,
        SorterPerf,
        SorterSetPerf,
        SorterShc
    }

    public enum ParamDataType
    {
        Int32,
        IntArray,
        Double,
        DoubleArray,
        String,
        StringArray,
        Guid,
        GuidArray,
        ByteArray
    }

    public enum ParamTypeName
    {
        Generation,
        MutationRate,
        MutationType,
        NumberFormat,
        Order,
        OrderStack,
        RecordId,
        RecordPath,
        RngCount,
        RandGenId,
        RandGenSeed,
        RandGenType,
        SortableCount,
        SortableFormat,
        SortableSetId,
        SortableSetOrbitMaxCount,
        SortableSetOrbitPerm,
        SortableSetRep,
        SorterCount,
        SorterExtent,
        SorterId,
        SorterMutationId,
        SorterPosition,
        SorterSaveMode,
        SorterSetId,
        SorterGroupName,
        StageBuddyCount,
        StageCount,
        SwitchCount,
        SwitchOrStage,
        TableName,
        Temperature,
        WorkspaceId
    }

}
