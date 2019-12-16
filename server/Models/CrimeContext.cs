using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace CrimeRecordsManager.Models
{
    public class CrimeContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public CrimeContext() : base("name=CrimeContext")
        {
            Configuration.LazyLoadingEnabled = false;

            Database.SetInitializer(new MigrateDatabaseToLatestVersion<CrimeContext, CrimeRecordsManager.Migrations.Configuration>());
        }

        public DbSet<PoliceStation> PoliceStations { get; set; }
        public DbSet<PoliceOfficer> PoliceOfficers { get; set; }
        public DbSet<Suspect> Suspects { get; set; }
        public DbSet<Complaint> Complaints { get; set; }
        public DbSet<Complainant> Complainants { get; set; }
        public DbSet<InvestigationReport> InvestigationReports { get; set; }
    }
}
