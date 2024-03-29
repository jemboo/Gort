﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Gort.Data.DataModel
{
    public class GortContext : DbContext, IGortContext
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

            modelBuilder.Entity<ParamType>()
                    .Property(c => c.ParamDataType)
                    .HasConversion<int>();

            //modelBuilder.Entity<CauseTypeGroup>()
            //        .Property(c => c.Name)
            //        .HasConversion<int>();

            //modelBuilder.Entity<CauseType>()
            //        .Property(c => c.Name)
            //        .HasConversion<int>();

            //modelBuilder.Entity<ParamType>()
            //        .Property(c => c.Name)
            //        .HasConversion<int>();

            modelBuilder.Entity<RandGen>()
                    .Property(c => c.RandGenType)
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

            modelBuilder.Entity<SorterMutation>()
                    .Property(c => c.MutationType)
                    .HasConversion<int>();

            modelBuilder.Entity<Workspace>().HasIndex(c => c.Name).IsUnique();

            base.OnModelCreating(modelBuilder);
        }

        public EntityEntry<T> Entry<T>(object entity) where T : class
        {
            return (EntityEntry<T>)this.Entry(entity);
        }

        public DbSet<CauseTypeGroup> CauseTypeGroup { get; set; }
        public DbSet<CauseType> CauseType { get; set; }
        public DbSet<Cause> Cause { get; set; }
        public DbSet<CauseParam> CauseParam { get; set; }
        public DbSet<CauseParamType> CauseParamType { get; set; }
        public DbSet<Param> Param { get; set; }
        public DbSet<ParamType> ParamType { get; set; }
        public DbSet<RandGen> RandGen { get; set; }
        public DbSet<SortableSet> SortableSet { get; set; }
        public DbSet<Sorter> Sorter { get; set; }
        public DbSet<SorterSet> SorterSet { get; set; }
        public DbSet<SorterPerf> SorterPerf { get; set; }
        public DbSet<SorterSetPerf> SorterSetPerf { get; set; }
        public DbSet<SorterMutation> SorterMutation { get; set; }
        public DbSet<Workspace> Workspace { get; set; }
    }

}
