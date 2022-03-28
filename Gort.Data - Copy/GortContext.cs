using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gort.Data
{
    public class GortContext : DbContext
    {
        public GortContext()
        {
            // Database.SetInitializer(new GortDBInitializer());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(connectionString: @"server=localhost;database=GortDb;uid=root;password='barney'",
                serverVersion: new MySqlServerVersion(new Version(8, 0, 27)));
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Cause>()
                    .Property(c => c.CauseStatus)
                    .HasConversion<int>();


            modelBuilder.Entity<CauseParamType>()
                    .Property(c => c.DataType)
                    .HasConversion<int>();


            modelBuilder.Entity<RandGen>()
                    .Property(c => c.RndGenType)
                    .HasConversion<int>();


            modelBuilder.Entity<Sortable>()
                    .Property(c => c.SortableFormat)
                    .HasConversion<int>();


            modelBuilder.Entity<SortableSet>()
                    .Property(c => c.SortableSetRep)
                    .HasConversion<int>();


            modelBuilder.Entity<SortableSet>()
                    .Property(c => c.SortableFormat)
                    .HasConversion<int>();


            modelBuilder.Entity<SorterPerf>()
                    .Property(c => c.NumberFormat)
                    .HasConversion<int>();


            modelBuilder.Entity<SorterSetPerf>()
                    .Property(c => c.NumberFormat)
                    .HasConversion<int>();


            modelBuilder.Entity<Workspace>().HasIndex(c => c.Name).IsUnique();
            modelBuilder.Entity<Cause>().HasIndex(c => c.Index).IsUnique();


            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Workspace> Workspace { get; set; }
        public DbSet<CauseTypeGroup> CauseTypeGroup { get; set; }
        public DbSet<CauseType> CauseType { get; set; }
        public DbSet<CauseParamType> CauseParamType { get; set; }
        public DbSet<Cause> Cause { get; set; }
        public DbSet<CauseParam> CauseParam { get; set; }
        public DbSet<RandGen> RandGen { get; set; }
        public DbSet<SortableSet> SortableSet { get; set; }
        public DbSet<Sorter> Sorter { get; set; }
        public DbSet<SorterSet> SorterSet { get; set; }
        public DbSet<SorterPerf> SorterPerf { get; set; }
        public DbSet<SorterSetPerf> SorterSetPerf { get; set; }
    }

}
