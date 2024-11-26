using CyberCareServices.Areas.Identity.Models;
using CyberCareServices.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CyberCareServices.Data
{
    public class CyberCareServicesContext : IdentityDbContext<ApplicationUser>
    {
        public CyberCareServicesContext(DbContextOptions<CyberCareServicesContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Component> Components { get; set; }

        public virtual DbSet<ComponentType> ComponentTypes { get; set; }

        public virtual DbSet<Customer> Customers { get; set; }

        public virtual DbSet<Employee> Employees { get; set; }

        public virtual DbSet<Order> Orders { get; set; }

        public virtual DbSet<Service> Services { get; set; }

    }
}
