using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.FSharp.Core;

namespace Gort.Data
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
        public string Description { get; set; }
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
        public SortableFormat BinaryFormat { get; set; }
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
        public SortableFormat BinaryFormat { get; set; }
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
        public SorterPerfRep SorterPerfRep { get; set; }
        public SortableFormat BinaryFormat { get; set; }
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
        public SorterSetRep SorterSetRep { get; set; }
        public SortableFormat BinaryFormat { get; set; }
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
        public SorterSetPerfRep SorterSetPerfRep { get; set; }
        public byte[] SorterSetPerfData { get; set; }
    }



}
