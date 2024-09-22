using KPImanDental.Model;
using KPImanDental.Model.Lookup;
using KPImanDental.Model.Patient;
using Microsoft.EntityFrameworkCore;

namespace KPImanDental.Data
{
    #pragma warning disable CS1591
    public class DataContext : DbContext
    {
        //Constructor with option
        //Need to register in services
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        //To access KpImanUser table in DB
        public DbSet<KpImanUser> Users { get; set; }
        public DbSet<Modules> Modules { get; set; }

        public DbSet<Department> Departments { get; set; } //Department and Position
        public DbSet<Position> Posititon { get; set; }


        #region Patient
        public DbSet<Patient> Patients { get; set; }
        public DbSet<PatientTreatment> PatientTreatments { get; set; }
        public DbSet<PatientDocuments> PatientDocuments { get; set; }
        #endregion


        #region Lookup
        public DbSet<TreatmentLookup> TreatmentLookup { get; set; }
        #endregion
    }
#pragma warning restore CS1591
}
