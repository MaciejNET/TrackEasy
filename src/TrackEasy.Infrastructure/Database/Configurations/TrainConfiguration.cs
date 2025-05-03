using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrackEasy.Domain.Trains;

namespace TrackEasy.Infrastructure.Database.Configurations;

internal sealed class TrainConfiguration : IEntityTypeConfiguration<Train>
{
    public void Configure(EntityTypeBuilder<Train> builder)
    {
        builder.ToTable("Trains");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Id)
            .ValueGeneratedNever();

        builder.Property(t => t.Name)
            .IsRequired()
            .HasMaxLength(15);

        builder.OwnsMany(t => t.Coaches, cb =>
        {
            cb.ToTable("TrainCoaches");
            
            cb.HasKey(x => x.Id);
            
            cb.Property(x => x.Id)
                .ValueGeneratedNever();
            
            cb.HasOne(x => x.Coach)
                .WithOne();

            cb.Property(x => x.Number)
                .IsRequired();
        });
    }
}