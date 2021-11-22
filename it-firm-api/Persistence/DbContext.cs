using it_firm_api.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace it_firm_api.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
           : base(options)
        {
        }

        public virtual DbSet<Employee> Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Employee>(c =>
            {
                c.ToTable("employees");
                c.Property(x => x.Email).HasMaxLength(64);
                c.Property(x => x.FistName).HasMaxLength(64);
                c.Property(x => x.LastName).HasMaxLength(64);
                c.Property(x => x.HashedPassword).HasMaxLength(255);
            });

            
        }
    }
}
