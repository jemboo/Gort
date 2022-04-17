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

    public class Cause
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid CauseId { get; set; }
        public string Description { get; set; } 
        public Guid CauseTypeID { get; set; }
        public virtual CauseType CauseType { get; set; }
        public CauseStatus CauseStatus { get; set; }
        public ICollection<CauseParam> CauseParams { get; set; } =
            new ObservableCollection<CauseParam>();
        public int Index { get; set; }
        public Guid WorkspaceId { get; set; }
        public virtual Workspace Workspace { get; set; }
    }

    public class CauseParam
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid CauseParamId { get; set; }
        public Guid CauseId { get; set; }
        public virtual Cause Cause { get; set; }
        public Guid CauseTypeParamId { get; set; }
        public virtual CauseParamType CauseTypeParam { get; set; }
        public Param Param { get; set; }
    }

    public class CauseType
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid CauseTypeId { get; set; }
        public CauseTypeName Name { get; set; }
        public Guid CauseTypeGroupId { get; set; }
        public ICollection<CauseParamType> CauseTypeParams { get; set; } =
                new ObservableCollection<CauseParamType>();
        public virtual CauseTypeGroup CauseTypeGroup { get; set; }
    }


    public class CauseTypeGroup
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid CauseTypeGroupId { get; set; }
        public CauseTypeGroupName Name { get; set; }
        public Guid? ParentId { get; set; }
        public virtual CauseTypeGroup? Parent { get; set; }
    }


    public class CauseParamType
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid CauseTypeParamId { get; set; }
        public Guid CauseTypeId { get; set; }
        public CauseParamTypeName Name { get; set; }
        public Guid ParamTypeId { get; set; }
    }

    public class ParamType
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid ParamTypeId { get; set; } 
        public ParamTypeName Name { get; set; } 
        public ICollection<Param> Params { get; set; } =
                new ObservableCollection<Param>();

        public DataType DataType { get; set; }
    }

    public class Param
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid ParamId { get; set; }
        public Guid ParamTypeId { get; set; }
        public byte[] Value { get; set; }
    }



    //public class RndGen
    //{
    //    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    //    [Key]
    //    public Guid RndGenId { get; set; }
    //    public RndGenType RndGenType { get; set; }
    //    public int Seed { get; set; }
    //}

}
