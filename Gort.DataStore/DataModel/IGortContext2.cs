using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Gort.DataStore.DataModel
{
    public interface IGortContext2
    {
        DbSet<Cause> Cause { get; set; }
        DbSet<CauseParam> CauseParam { get; set; }
        DbSet<SorterGen> SorterGen { get; set; }
        DbSet<SorterMutator> SorterMutator { get; set; }
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

    public static class IGortContext2Ext
    {
        public static T GetOrMake<T>(this DbSet<T> dbSet, 
                                          Func<T, bool> selector,
                                          Func<T> maker) where T : class
        {
            var res = dbSet.FirstOrDefault(selector);
            if (res == null)
            {
                var tVal = maker();
                dbSet.Add(tVal);
                return tVal;
            }
            return res;
        }

    }
}