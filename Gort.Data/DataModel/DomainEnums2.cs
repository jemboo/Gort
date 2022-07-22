namespace Gort.Data.DataModel2
{

    public enum MutationType
    {
        Switch,
        BoundedSwitch,
        Stage,
        BoundedStage,
        StageRfl,
        BoundedStageRfl
    }

    public enum NumberFormat
    {
        u8,
        u16,
        u32,
        s32
    }
    public enum RandGenType
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

    public enum SortableFormat
    {
        b64,
        u8,
        u16
    }

}
