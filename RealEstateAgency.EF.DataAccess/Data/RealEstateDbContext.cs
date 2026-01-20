using Microsoft.EntityFrameworkCore;
using RealEstateAgency.EF.DataAccess.Models;
using System.Collections.Generic;

namespace RealEstateAgency.EF.DataAccess.Data
{
    public class RealEstateDbContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Contract> Contracts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=RealEstateAgencyEF;Trusted_Connection=True;");
        }
    }
}