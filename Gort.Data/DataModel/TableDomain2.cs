using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.FSharp.Core;

namespace Gort.Data.DataModel2
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
        public int Order { get; set; }
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
        public Guid CauseId { get; set; }
        public virtual Cause Cause { get; set; }
        public string CausePath { get; set; }
        public string Description { get; set; }
        public byte[] Data { get; set; }
    }

    public class SortableMutation
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid SortableMutationId { get; set; }
        public Guid CauseId { get; set; }
        public virtual Cause Cause { get; set; }
        public string CausePath { get; set; }
        public string Description { get; set; }
        public byte[] Data { get; set; }
    }


}
