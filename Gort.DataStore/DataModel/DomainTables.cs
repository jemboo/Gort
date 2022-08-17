namespace Gort.DataStore.DataModel
{
    public static class Versions
    {
        public static class RandGen
        {
            public const string rndGen = "rndGen";
        }
        public static class SorterMutator
        {
            public const string uniform = "uniform";
        }
    }
    public class BitPackR
    {
        public int BitPackRId { get; set; }
        public int BitsPerSymbol { get; set; }
        public int SymbolCount { get; set; }
        public byte[] Data { get; set; }
    }

    public class ComponentR
    {
        public int ComponentRId { get; set; }
        public int CauseId { get; set; }
        public virtual CauseR CauseR { get; set; }
        public string CausePath { get; set; }
        public string Category { get; set; }
        public string Version { get; set; }
        public string Json { get; set; }
    }

    public class SortableSetR
    {
        public int SortableSetRId { get; set; }
        public int CauseRId { get; set; }
        public virtual CauseR CauseR { get; set; }
        public string CausePath { get; set; }
        public string Version { get; set; }
        public string Json { get; set; }
        public int? BitPackRId { get; set; }
        public virtual BitPackR BitPackR { get; set; }
    }

    public class SorterR
    {
        public int SorterRId { get; set; }
        public int CauseRId { get; set; }
        public virtual CauseR CauseR { get; set; }
        public string CausePath { get; set; }
        public string Version { get; set; }
        public string Json { get; set; }
        public int? BitPackRId { get; set; }
        public virtual BitPackR BitPackR { get; set; }
    }

    public class SorterSetR
    {
        public int SorterSetRId { get; set; }
        public int CauseRId { get; set; }
        public virtual CauseR CauseR { get; set; }
        public string CausePath { get; set; }
        public string Version { get; set; }
        public string Json { get; set; }
        public int? BitPackRId { get; set; }
        public virtual BitPackR BitPackR { get; set; }
    }

    public class SorterSetPerfR
    {
        public int SorterSetPerfRId { get; set; }
        public int CauseRId { get; set; }
        public virtual CauseR CauseR { get; set; }
        public string CausePath { get; set; }
        public int SorterSetRId { get; set; }
        public virtual SorterSetR SorterSetR { get; set; }
        public string Version { get; set; }
        public string Json { get; set; }
        public int? BitPackRId { get; set; }
        public virtual BitPackR BitPackR { get; set; }
    }
}
