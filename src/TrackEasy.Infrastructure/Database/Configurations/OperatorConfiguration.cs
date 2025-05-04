using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrackEasy.Domain.Operators;

namespace TrackEasy.Infrastructure.Database.Configurations;

internal sealed class OperatorConfiguration : IEntityTypeConfiguration<Operator>
{
    public void Configure(EntityTypeBuilder<Operator> builder)
    {
        builder.ToTable("Operators");
        
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.Code)
            .IsRequired()
            .HasMaxLength(3);

        builder.HasMany(x => x.Coaches)
            .WithOne()
            .HasForeignKey(x => x.OperatorId);

        builder.HasMany(x => x.Trains)
            .WithOne();

        builder.HasMany(x => x.Managers)
            .WithOne(x => x.Operator)
            .HasForeignKey(x => x.OperatorId);
    }
}