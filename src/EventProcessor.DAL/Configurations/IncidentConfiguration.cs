using EventProcessor.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventProcessor.DAL.Configurations;

public class IncidentConfiguration: IEntityTypeConfiguration<Incident>
{
    public void Configure(EntityTypeBuilder<Incident> builder)
    {
        builder.HasMany(i => i.Events)
            .WithOne()
            .HasForeignKey("IncidentId")
            .OnDelete(DeleteBehavior.Cascade);
    }
}