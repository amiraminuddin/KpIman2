using KPImanDental.Model;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

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
    }
    #pragma warning restore CS1591
}
