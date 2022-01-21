using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gort.Data
{
    public class Workspace
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid WorkspaceId { get; set; }
        public string Name { get; set; }
        public ICollection<Cause> Causes { get; set; } =
                new ObservableCollection<Cause>();
    }
}
