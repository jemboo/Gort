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


            modelBuilder.Entity<CauseTypeParam>()
                    .Property(c => c.DataType)
                    .HasConversion<int>();


            modelBuilder.Entity<RandGen>()
                    .Property(c => c.RndGenType)
                    .HasConversion<int>();


            modelBuilder.Entity<SortableSet>()
                    .Property(c => c.SortableSetRep)
                    .HasConversion<int>();


            modelBuilder.Entity<SorterSet>()
                    .Property(c => c.SorterSetRep)
                    .HasConversion<int>();


            modelBuilder.Entity<SorterPerf>()
                    .Property(c => c.SorterPerfRep)
                    .HasConversion<int>();


            modelBuilder.Entity<SorterSetPerf>()
                    .Property(c => c.SorterSetPerfRep)
                    .HasConversion<int>();


            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Workspace> Workspaces { get; set; }
        public DbSet<CauseTypeGroup> CauseTypeGroups { get; set; }
        public DbSet<CauseType> CauseTypes { get; set; }
        public DbSet<CauseTypeParam> CauseTypeParams { get; set; }
        public DbSet<Cause> Causes { get; set; }
        public DbSet<CauseParam> CauseParams { get; set; }
        public DbSet<RandGen> RandGens { get; set; }
        public DbSet<SortableSet> SortableSets { get; set; }
        public DbSet<Sorter> Sorters { get; set; }
        public DbSet<SorterSet> SorterSets { get; set; }
        public DbSet<SorterPerf> SorterPerfs { get; set; }
        public DbSet<SorterSetPerf> SorterSetPerfs { get; set; }
    }

}
