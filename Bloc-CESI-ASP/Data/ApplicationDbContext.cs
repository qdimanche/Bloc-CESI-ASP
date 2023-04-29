using Bloc_CESI_ASP.Models;
using Microsoft.EntityFrameworkCore;

namespace Bloc_CESI_ASP.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
        
    }
    
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Site> Sites { get; set; }
    public DbSet<Service> Services { get; set; }
}