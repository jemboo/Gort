﻿using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gort.Data.DataModel
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

    public class Cause
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid CauseId { get; set; }
        public string? CauseDescr { get; set; } 
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
        public Guid CauseParamTypeId { get; set; }
        public virtual CauseParamType CauseParamType { get; set; }
        public Guid ParamId { get; set; }
        public virtual Param Param { get; set; }
    }

    public class CauseType
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid CauseTypeId { get; set; }
        public string? Name { get; set; }
        public Guid CauseTypeGroupId { get; set; }
        public ICollection<CauseParamType> CauseParamTypes { get; set; } =
                new ObservableCollection<CauseParamType>();
        public virtual CauseTypeGroup CauseTypeGroup { get; set; }
    }


    public class CauseTypeGroup
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid CauseTypeGroupId { get; set; }
        public string? Name { get; set; }
        public Guid? ParentId { get; set; }
        public virtual CauseTypeGroup? Parent { get; set; }
    }


    public class CauseParamType
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid CauseParamTypeId { get; set; }
        public Guid CauseTypeId { get; set; }
        public string? Name { get; set; }
        public Guid ParamTypeId { get; set; }
        public virtual ParamType ParamType { get; set; }
    }

    public class ParamType
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid ParamTypeId { get; set; } 
        public string? Name { get; set; } 
        public ICollection<Param> Params { get; set; } =
                new ObservableCollection<Param>();
        public ParamDataType ParamDataType { get; set; }
    }

    public class Param
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid ParamId { get; set; }
        public Guid ParamTypeId { get; set; }
        public virtual ParamType ParamType { get; set; }
        public byte[]? Value { get; set; }
    }
}
