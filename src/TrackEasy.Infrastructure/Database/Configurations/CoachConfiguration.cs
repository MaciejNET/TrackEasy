using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrackEasy.Domain.Coaches;

namespace TrackEasy.Infrastructure.Database.Configurations;

internal sealed class CoachConfiguration : IEntityTypeConfiguration<Coach>
{
    public void Configure(EntityTypeBuilder<Coach> builder)
    {
        builder.ToTable("Coaches");
        
        builder.HasKey(c => c.Id);
        
        builder.Property(c => c.Id)
            .ValueGeneratedNever();
        
        builder.Property(c => c.Code)
            .IsRequired()
            .HasMaxLength(15);

        builder.OwnsMany(c => c.Seats, sb =>
        {
            sb.ToTable("Seats");
        });
    }
}