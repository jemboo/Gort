using System.Collections.ObjectModel;

namespace Gort.Data
{
    public class Cause
    {
        public int CauseId { get; set; }
        public string Name { get; set; } 
        public CauseType CauseType { get; set; }
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
        public int NamedParamId { get; set; }
        public virtual NamedParam NamedParam { get; set; }
        public string Value { get; set; }
    }

    public class CauseType
    {
        public int CauseTypeId { get; set; }
        public string Name { get; set; }
        public int CauseTypeGroupId { get; set; }
        public virtual CauseTypeGroup CauseTypeGroup { get; set; }
    }


    //modelBuilder.Entity<CauseTypeGroup>().HasData(new CauseTypeGroup { CauseTypeGroupId = 1, Name = "root" });
public class CauseTypeGroup
    {
        public int CauseTypeGroupId { get; set; }
        public string Name { get; set; }
        public int? ParentId { get; set; }
        public virtual CauseTypeGroup? Parent { get; set; }
    }

    public class NamedParam
    {
        public int NamedParamId { get; set; }
        public string Name { get; set; }
        public int CauseTypeId { get; set; }
        public virtual CauseType CauseType { get; set; }
        public string DataType { get; set; }
    }
}
