namespace Gort.Data
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
        WorkspaceId,
        RngSeed,
        RngType,
        RndGenId,
        RndGenCount,
        Order,
        SortableFormat,
        TableName,
        RecordId,
        RecordPath,
        SortableCount,
        SortableSetOrbitPerm,
        SortableSetOrbitMaxCount,
        SortableSetStackedOrders,
        SwitchCount,
        SwitchSequenceStartPos,
        SwitchSequenceLength,
        StageCount,
        SorterCount,
        StageBuddyCount,
        SorterId,
        SortableSetId,
        SorterSetName,
        SorterSetId,
        SorterSaveMode,
        Generation,
        MutationRangeStartPos,
        MutationRangeLength
    }

    public enum ParamTypeName
    {
        WorkspaceId,
        RngSeed,
        RngType,
        RndGenId,
        RndGenCount,
        Order,
        OrderStack,
        SortableFormat,
        TableName,
        RecordId,
        RecordPath,
        SortableCount,
        SortableSetOrbitPerm,
        SortableSetOrbitMaxCount,
        SwitchOrStage,
        SorterPosition,
        SorterExtent,
        StageCount,
        SorterCount,
        StageBuddyCount,
        SorterId,
        SortableSetId,
        SorterSetName,
        SorterSetId,
        SorterSaveMode,
        Generation,
        SorterMutationId,
        MutationRate,
        Temperature
    }

    public enum CauseTypeGroupName
    {
        Root,
        RandGen,
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
        SorterImport,
        SorterGroupName,
        SorterSetImport,
        SorterSetRandBySwitch,
        SorterSetRandByStage,
        SorterSetRandByRflStage,
        SorterSetRandByRflStageBuddies,
        SorterSetRandByMutation,
        SwitchListImport,
        SorterPerf,
        SorterSetPerfBins,
        SorterShcImport,
        SorterShc
    }

}
