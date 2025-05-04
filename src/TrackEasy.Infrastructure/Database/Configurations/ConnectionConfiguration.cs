using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrackEasy.Domain.Connections;

namespace TrackEasy.Infrastructure.Database.Configurations;

internal sealed class ConnectionConfiguration : IEntityTypeConfiguration<Connection>
{
    public void Configure(EntityTypeBuilder<Connection> builder)
    {
        builder.ToTable("Connections");
        
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(15);

        builder.HasOne(x => x.Operator)
            .WithMany();

        builder.ComplexProperty(x => x.PricePerKilometer);
        
        builder.HasOne(x => x.Train)
            .WithMany();

        builder.OwnsOne(x => x.Schedule, sb =>
        {
            sb.ToTable("Schedules");
        });

        builder.OwnsMany(x => x.Stations, sb =>
        {
            sb.ToTable("ConnectionStations");

            sb.HasKey(x => x.Id);
            
            sb.Property(x => x.Id)
                .ValueGeneratedNever();

            sb.HasOne(x => x.Station)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);
        });

        builder.OwnsOne(x => x.Request, rb =>
        {
            rb.ToTable("ConnectionRequests");

            rb.HasKey(x => x.Id);
            
            rb.Property(x => x.Id)
                .ValueGeneratedNever();

            rb.OwnsOne(x => x.Schedule, sb =>
            {
                sb.ToTable("ConnectionRequestsSchedules");
            });
            
            rb.OwnsMany(x => x.Stations, sb =>
            {
                sb.ToTable("ConnectionRequestsStations");

                sb.HasKey(x => x.Id);
                
                sb.Property(x => x.Id)
                    .ValueGeneratedNever();

                sb.HasOne(x => x.Station)
                    .WithMany()
                    .OnDelete(DeleteBehavior.NoAction);
            });
        });
    }
}