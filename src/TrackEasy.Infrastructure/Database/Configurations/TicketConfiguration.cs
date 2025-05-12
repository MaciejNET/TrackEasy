using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrackEasy.Domain.Tickets;

namespace TrackEasy.Infrastructure.Database.Configurations;

internal sealed class TicketConfiguration : IEntityTypeConfiguration<Ticket>
{
    public void Configure(EntityTypeBuilder<Ticket> builder)
    {
        builder.ToTable("Tickets");
        
        builder.HasKey(t => t.Id);
        
        builder.Property(t => t.Id)
            .ValueGeneratedNever();

        builder.ComplexProperty(x => x.Price);

        builder.Property(x => x.TicketNumber)
            .ValueGeneratedOnAdd();

        builder.OwnsMany(x => x.Passengers, pb =>
        {
            pb.ToTable("People");

            pb.Property(x => x.FirstName)
                .IsRequired()
                .HasMaxLength(50);

            pb.Property(x => x.LastName)
                .IsRequired()
                .HasMaxLength(50);

            pb.Property(x => x.DateOfBirth)
                .IsRequired();

            pb.Property(x => x.Discount)
                .HasMaxLength(50);
        });

        builder.OwnsMany(x => x.Stations, sb =>
        {
            sb.ToTable("TicketConnectionStations");
        });
    }
}