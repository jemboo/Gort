using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Gort.Data
{
    public interface IGortContext
    {
        DbSet<Cause> Cause { get; set; }
        DbSet<CauseParam> CauseParam { get; set; }
        DbSet<CauseType> CauseType { get; set; }
        DbSet<CauseTypeGroup> CauseTypeGroup { get; set; }
        DbSet<ParamType> ParamType { get; set; }
        DbSet<Param> Param { get; set; }
        DbSet<RandGen> RandGen { get; set; }
        DbSet<SortableSet> SortableSet { get; set; }
        DbSet<Sorter> Sorter { get; set; }
        DbSet<SorterPerf> SorterPerf { get; set; }
        DbSet<SorterSet> SorterSet { get; set; }
        DbSet<SorterSetPerf> SorterSetPerf { get; set; }
        DbSet<Workspace> Workspace { get; set; }
        EntityEntry<T> Entry<T>(object entity) where T : class;
        int SaveChanges();
    }
}