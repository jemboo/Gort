namespace Gort.DataStore.DataModel
{
    public static class Versions
    {
        public const string RandGen_rndGen = "rndGen";
    }
    public class BitPackRecord
    {
        public int BitPackRecordId { get; set; }
        public int BitsPerSymbol { get; set; }
        public int SymbolCount { get; set; }
        public byte[] Data { get; set; }
    }

    public class RandGen
    {
        public int RandGenId { get; set; }
        public int CauseId { get; set; }
        public virtual Cause Cause { get; set; }
        public string CausePath { get; set; }
        public string Version { get; set; }
        public string Cereal { get; set; }
    }

    public class Sortable
    {
        public int SortableId { get; set; }
        public int CauseId { get; set; }
        public virtual Cause Cause { get; set; }
        public string CausePath { get; set; }
        public string Version { get; set; }
        public string Cereal { get; set; }
    }

    public class SortableSet
    {
        public int SortableSetId { get; set; }
        public int CauseId { get; set; }
        public virtual Cause Cause { get; set; }
        public string CausePath { get; set; }
        public string Version { get; set; }
        public string Cereal { get; set; }
        public int BitPackId { get; set; }
        public virtual BitPackRecord BitPack { get; set; }
    }

    public class SortableGen
    {
        public int SortableGenId { get; set; }
        public int CauseId { get; set; }
        public virtual Cause Cause { get; set; }
        public string CausePath { get; set; }
        public string Version { get; set; }
        public string Cereal { get; set; }
        public int BitPackId { get; set; }
        public virtual BitPackRecord BitPack { get; set; }
    }

    public class Sorter
    {
        public int SorterId { get; set; }
        public int CauseId { get; set; }
        public virtual Cause Cause { get; set; }
        public string CausePath { get; set; }
        public string Version { get; set; }
        public string Cereal { get; set; }
        public int BitPackId { get; set; }
        public virtual BitPackRecord BitPack { get; set; }
    }

    public class SorterSet
    {
        public int SorterSetId { get; set; }
        public int CauseId { get; set; }
        public virtual Cause Cause { get; set; }
        public string CausePath { get; set; }
        public string Version { get; set; }
        public string Cereal { get; set; }
        public int BitPackId { get; set; }
        public virtual BitPackRecord BitPack { get; set; }
    }

    public class SorterGen
    {
        public int SorterGenId { get; set; }
        public int CauseId { get; set; }
        public virtual Cause Cause { get; set; }
        public string CausePath { get; set; }
        public string Version { get; set; }
        public string Cereal { get; set; }
        public int BitPackId { get; set; }
        public virtual BitPackRecord BitPack { get; set; }
    }

    public class SorterMutator
    {
        public int SorterMutatorId { get; set; }
        public int CauseId { get; set; }
        public virtual Cause Cause { get; set; }
        public string CausePath { get; set; }
        public string Version { get; set; }
        public string Cereal { get; set; }
        public int BitPackId { get; set; }
        public virtual BitPackRecord BitPack { get; set; }
    }

    public class SorterPerf
    {
        public int SorterPerfId { get; set; }
        public int CauseId { get; set; }
        public virtual Cause Cause { get; set; }
        public string CausePath { get; set; }
        public int SorterSetId { get; set; }
        public virtual SorterSet SorterSet { get; set; }
        public string Version { get; set; }
        public string Cereal { get; set; }
        public int BitPackId { get; set; }
        public virtual BitPackRecord BitPack { get; set; }
    }

    public class SorterSetPerf
    {
        public int SorterSetPerfId { get; set; }
        public int CauseId { get; set; }
        public virtual Cause Cause { get; set; }
        public string CausePath { get; set; }
        public int SorterSetId { get; set; }
        public virtual SorterSet SorterSet { get; set; }
        public string Version { get; set; }
        public string Cereal { get; set; }
        public int BitPackId { get; set; }
        public virtual BitPackRecord BitPack { get; set; }
    }
}
