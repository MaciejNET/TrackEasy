using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrackEasy.Domain.Managers;

namespace TrackEasy.Infrastructure.Database.Configurations;

internal sealed class ManagerConfiguration : IEntityTypeConfiguration<Manager>
{
    public void Configure(EntityTypeBuilder<Manager> builder)
    {
        builder.ToTable("Managers");

        builder.HasKey(x => new { x.OperatorId, x.UserId });
        
        builder.HasOne(x => x.User)
            .WithOne()
            .HasForeignKey<Manager>(x => x.UserId);

        builder.HasOne(x => x.Operator)
            .WithMany(x => x.Managers)
            .HasForeignKey(x => x.OperatorId);
    }
}