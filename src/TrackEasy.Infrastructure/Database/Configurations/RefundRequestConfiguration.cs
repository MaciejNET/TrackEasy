using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrackEasy.Domain.RefundRequests;

namespace TrackEasy.Infrastructure.Database.Configurations;

internal sealed class RefundRequestConfiguration : IEntityTypeConfiguration<RefundRequest>
{
    public void Configure(EntityTypeBuilder<RefundRequest> builder)
    {
        builder.ToTable("RefundRequests");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.Id)
            .ValueGeneratedNever();

        builder.Property(x => x.Reason)
            .IsRequired()
            .HasMaxLength(255);

        builder.HasOne(x => x.Ticket)
            .WithMany()
            .OnDelete(DeleteBehavior.NoAction);
    }
}