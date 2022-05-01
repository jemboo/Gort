using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.FSharp.Core;

namespace Gort.Data.DataModel
{
    public class RandGen
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid RandGenId { get; set; }
        public Guid CauseId { get; set; }
        public string CausePath { get; set; }
        public Guid StructId { get; set; }
        public virtual Cause Cause { get; set; }
        public int Seed { get; set; }
        public RndGenType RndGenType { get; set; }
    }

    public class Sortable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid SortableId { get; set; }
        public Guid CauseId { get; set; }
        public string CausePath { get; set; }
        public Guid StructId { get; set; }
        public Guid? SortableSetId { get; set; }
        public virtual Cause Cause { get; set; }
        public string Description { get; set; }
        public int Order { get; set; }
        public SortableFormat SortableFormat { get; set; }
        public byte[] Data { get; set; }
    }

    public class SortableSet
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid SortableSetId { get; set; }
        public Guid CauseId { get; set; }
        public string CausePath { get; set; }
        public Guid StructId { get; set; }
        public virtual Cause Cause { get; set; }
        public string Description { get; set; }
        public SortableSetRep SortableSetRep { get; set; }
        public int Order { get; set; }
        public SortableFormat SortableFormat { get; set; }
        public byte[] Data { get; set; }
    }

    public class Sorter
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid SorterId { get; set; }
        public Guid StructId { get; set; }
        public Guid CauseId { get; set; }
        public string CausePath { get; set; }
        public virtual Cause Cause { get; set; }
        public string Description { get; set; }
        public int Order { get; set; }
        public byte[] Data { get; set; }

    }

    public class SwitchList
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid SwitchListId { get; set; }
        public Guid StructId { get; set; }
        public Guid CauseId { get; set; }
        public string CausePath { get; set; }
        public virtual Cause Cause { get; set; }
        public string Description { get; set; }
        public int Order { get; set; }
        public byte[] Data { get; set; }
    }

    public class SorterPerf
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid SorterPerfId { get; set; }
        public Guid CauseId { get; set; }
        public virtual Cause Cause { get; set; }
        public string CausePath { get; set; }
        public string Description { get; set; }
        public Guid SorterId { get; set; }
        public virtual Sorter Sorter { get; set; }
        public Guid SortableSetId { get; set; }
        public virtual SortableSet SortableSet { get; set; }
        public NumberFormat NumberFormat { get; set; }
        public byte[] Data { get; set; }
    }

    public class SorterSet
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid SorterSetId { get; set; }
        public Guid CauseId { get; set; }
        public virtual Cause Cause { get; set; }
        public string CausePath { get; set; }
        public string Description { get; set; }
        public int Order { get; set; }
        public int SwitchLength { get; set; }
        public bool IsGenerated { get; set; }
        public byte[] Data { get; set; }
    }

    public class SorterSetPerf
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid SorterSetPerfId { get; set; }
        public Guid CauseId { get; set; }
        public virtual Cause Cause { get; set; }
        public string CausePath { get; set; }
        public string Description { get; set; }
        public Guid SorterSetId { get; set; }
        public virtual SorterSet SorterSet { get; set; }
        public Guid SortableSetId { get; set; }
        public virtual SortableSet SortableSet { get; set; }
        public NumberFormat NumberFormat { get; set; }
        public byte[] Data { get; set; }
    }

    public class SorterMutation
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid SorterMutationId { get; set; }
        public double MutationRate { get; set; }
        public MutationType MutationType { get; set; }
        public byte[] Data { get; set; }
    }

}
