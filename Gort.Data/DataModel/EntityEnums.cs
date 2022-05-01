namespace Gort.Data.DataModel
{

    public enum CauseStatus
    {
        Pending,
        InProgress,
        Complete,
        Error
    }

    public enum DataType
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

    public enum RndGenType
    {
        Lcg,
        Clr
    }

    public enum MutationType
    {
        Switch,
        BoundedSwitch,
        Stage,
        BoundedStage,
        StageRfl,
        BoundedStageRfl
    }

    public enum SorterSaveMode
    {
        All,
        GoodSet,
        FullSet
    }

    public enum SortableSetRep
    {
        Explicit,
        AllBits,
        Orbit,
        Stack
    }

    public enum SortableFormat
    {
        b64,
        u8,
        u16
    }

    public enum NumberFormat
    {
        u8,
        u16,
        u32,
        s32
    }

    public enum CauseParamTypeName
    {
        Generation,
        MutationRate,
        Order,
        OrderStack,
        RecordId,
        RecordPath,
        RngCount,
        RngId,
        RngSeed,
        RngType,
        SortableCount,
        SortableFormat,
        SortableSetId,
        SortableSetOrbitMaxCount,
        SortableSetOrbitPerm,
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

    public enum ParamTypeName
    {
        Generation,
        MutationRate,
        Order,
        OrderStack,
        RecordId,
        RecordPath,
        RngCount,
        RngId,
        RngSeed,
        RngType,
        SortableCount,
        SortableFormat,
        SortableSetId,
        SortableSetOrbitMaxCount,
        SortableSetOrbitPerm,
        SorterCount,
        SorterExtent,
        SorterId,
        SorterMutationId,
        SorterPosition,
        SorterSaveMode,
        SorterSetId,
        SorterSetName,
        StageBuddyCount,
        StageCount,
        SwitchCount,
        SwitchOrStage,
        TableName,
        Temperature,
        WorkspaceId
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

}
