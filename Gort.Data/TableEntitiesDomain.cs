using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gort.Data
{
    public class SortableSet
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid WorkspaceId { get; set; }
        public string Name { get; set; }
        public ICollection<Cause> Causes { get; set; } =
                new ObservableCollection<Cause>();
    }

    public class Sorter
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid CauseId { get; set; }
        public string Description { get; set; } 
        public CauseType CauseType { get; set; }
        public ICollection<CauseParam> CauseParams { get; set; } =
            new ObservableCollection<CauseParam>();
        public int Index { get; set; }
        public Guid WorkspaceId { get; set; }
        public virtual Workspace Workspace { get; set; }
    }

    public class SorterSet
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid CauseParamId { get; set; }
        public Guid CauseId { get; set; }
        public virtual Cause Cause { get; set; }
        public Guid CauseTypeParamId { get; set; }
        public virtual CauseTypeParam CauseTypeParam { get; set; }
        public string Value { get; set; }
    }

    public class PerfBinSet
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid CauseTypeId { get; set; }
        public string Name { get; set; }
        public Guid CauseTypeGroupId { get; set; }
        public virtual CauseTypeGroup CauseTypeGroup { get; set; }
    }

}
