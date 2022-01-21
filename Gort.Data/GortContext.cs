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

        public DbSet<Workspace> Workspaces { get; set; }

        public DbSet<Cause> Causes { get; set; }
        public DbSet<CauseTypeGroup> CauseTypeGroups { get; set; }
    }

    //public class GortDBInitializer : DropCreateDatabaseAlways<GortContext>
    //{
    //    protected override void Seed(GortContext context)
    //    {
    //        IList<CauseTypeGroup> defaultCauseTypeGroups = new List<CauseTypeGroup>();

    //        defaultCauseTypeGroups.Add(new CauseTypeGroup() { CauseTypeGroupId = 1, Name = "root", ParentGroup=null });
    //        context.CauseTypeGroups.AddRange(defaultCauseTypeGroups);

    //        base.Seed(context);
    //    }
    //}


}
