using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Gort.DataStore.DataModel
{
    public interface IGortContext2
    {
        DbSet<CauseR> CauseR { get; set; }
        DbSet<CauseParamR> CauseParamR { get; set; }
        DbSet<Param> Param { get; set; }
        DbSet<RandGenR> RandGenR { get; set; }
        DbSet<SortableSetR> SortableSetR { get; set; }
        DbSet<SorterR> SorterR { get; set; }
        DbSet<SorterSetR> SorterSetR { get; set; }
        DbSet<SorterSetPerfR> SorterSetPerfR { get; set; }
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