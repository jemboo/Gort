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
        public DbSet<Cause> Cause { get; set; }
        public DbSet<CauseParam> CauseParam { get; set; }
        public EntityEntry<T> Entry<T>(object entity) where T : class
        {
            return (EntityEntry<T>)this.Entry(entity);
        }

        public DbSet<Param> Param { get; set; }
        public DbSet<RandGen> RandGen { get; set; }
        public DbSet<Sortable> Sortable { get; set; }
        public DbSet<SortableSet> SortableSet { get; set; }
        public DbSet<SortableGen> SortableGen { get; set; }
        public DbSet<Sorter> Sorter { get; set; }
        public DbSet<SorterSet> SorterSet { get; set; }
        public DbSet<SorterGen> SorterGen { get; set; }
        public DbSet<SorterMutator> SorterMutator { get; set; }
        public DbSet<SorterPerf> SorterPerf { get; set; }
        public DbSet<SorterSetPerf> SorterSetPerf { get; set; }

    }
}
