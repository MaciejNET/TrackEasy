using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrackEasy.Domain.Stations;

namespace TrackEasy.Infrastructure.Database.Configurations;

internal sealed class StationConfiguration : IEntityTypeConfiguration<Station>
{
    public void Configure(EntityTypeBuilder<Station> builder)
    {
        builder.ToTable("Stations");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Id)
            .ValueGeneratedNever();

        builder.Property(s => s.Name)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(s => s.CityId)
            .IsRequired();

        builder.HasOne(s => s.City)
            .WithMany()
            .HasForeignKey(x => x.CityId);

        builder.ComplexProperty(x => x.GeographicalCoordinates);
    }
}