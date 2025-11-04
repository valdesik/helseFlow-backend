using HelseFlow_Backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HelseFlow_Backend.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<VitalLog> VitalLogs { get; set; }
    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<Appointment> Appointments { get; set; }
    public DbSet<Guideline> Guidelines { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Here you can configure your entities if needed
        // For example, setting up primary keys, relationships, etc.
        // EF Core's conventions are often sufficient for simple cases.

        modelBuilder.Entity<User>().HasKey(u => u.Id);
        modelBuilder.Entity<VitalLog>().HasKey(v => v.Id);
        modelBuilder.Entity<Doctor>().HasKey(d => d.Id);
        modelBuilder.Entity<Appointment>().HasKey(a => a.Id);
        modelBuilder.Entity<Guideline>().HasKey(g => g.Id);
    }
}
