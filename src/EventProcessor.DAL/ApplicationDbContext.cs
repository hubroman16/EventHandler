using EventProcessor.DAL.Configurations;
using EventProcessor.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace EventProcessor.DAL;

public class ApplicationDbContext: DbContext
{
    public DbSet<Incident> Incidents { get; set; }
    public DbSet<Event> Events { get; set; }
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new IncidentConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}