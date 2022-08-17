using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;


namespace Gort.DataStore.DataModel
{
    public class GortContext2 : DbContext, IGortContext2
    {
        public GortContext2()
        {
            // Database.SetInitializer(new GortDBInitializer());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(connectionString: @"server=localhost;database=GortDb2;uid=root;password='barney'",
                serverVersion: new MySqlServerVersion(new Version(8, 0, 27)));

            base.OnConfiguring(optionsBuilder);
        }

        public DbSet<Workspace> Workspace { get; set; }
        public DbSet<BitPackR> BitPackR { get; set; }
        public DbSet<CauseR> CauseR { get; set; }
        public DbSet<CauseParamR> CauseParamR { get; set; }
        public EntityEntry<T> Entry<T>(object entity) where T : class
        {
            return (EntityEntry<T>)this.Entry(entity);
        }

        public DbSet<Param> Param { get; set; }
        public DbSet<ComponentR> RandGenR { get; set; }
        public DbSet<SortableSetR> SortableSetR { get; set; }
        public DbSet<SorterR> SorterR { get; set; }
        public DbSet<SorterSetR> SorterSetR { get; set; }
        public DbSet<SorterSetPerfR> SorterSetPerfR { get; set; }

    }
}
