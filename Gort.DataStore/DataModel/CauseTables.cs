using Gort.DataStore.CauseBuild;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gort.DataStore.DataModel
{
    public class Workspace
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid WorkspaceId { get; set; }
        public string? Name { get; set; }
        public ICollection<Cause> Causes { get; set; } =
                new ObservableCollection<Cause>();
    }

    [Index(nameof(WorkspaceId), nameof(Index), IsUnique = true)]
    public class Cause
    {
        public int CauseId { get; set; }
        public string Genus { get; set; }
        public string Species { get; set; }
        public string? Comments { get; set; }
        public CauseStatus CauseStatus { get; set; }
        public ICollection<CauseParam> CauseParams { get; set; } =
            new ObservableCollection<CauseParam>();

        public int Index { get; set; }
        public Guid WorkspaceId { get; set; }
        public virtual Workspace Workspace { get; set; }
    }

    public class CauseParam
    {
        public int CauseParamId { get; set; }
        public int CauseId { get; set; }
        public virtual Cause Cause { get; set; }
        public string Name { get; set; }
        public int ParamId { get; set; }
        public virtual Param Param { get; set; }
    }

    public class Param
    {
        public int ParamId { get; set; }
        public string? Name { get; set; }
        public ParamDataType ParamDataType { get; set; }
        public byte[]? Value { get; set; }
    }

    public static class IdUtils
    {
        public static Workspace AddId(this Workspace ctg)
        {
            var id = Guid.NewGuid();
            return new Workspace() { WorkspaceId = id, Name = ctg.Name };
        }

    }

}
