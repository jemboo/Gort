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
        Integer,
        Float,
        String,
        Rng,
        Sorter,
        SorterSet,
        Sortable,
        SortableSet,
        SorterSaveMode
    }

    public enum RndGenType
    {
        Lcg,
        Clr
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

    public enum SortableSetDataFormat
    {
        b64,
        u8,
        u16
    }

    public enum SorterSetRep
    {
        Explicit,
        Generated
    }

    public enum SorterPerfRep
    {
        PerfBin
    }

    public enum SorterSetPerfRep
    {
        PerfBins
    }

}
