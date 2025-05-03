using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrackEasy.Domain.Notifications;
using TrackEasy.Domain.Users;

namespace TrackEasy.Infrastructure.Database.Configurations;

internal sealed class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.ToTable("Notifications");

        builder.HasKey(n => n.Id);

        builder.Property(n => n.Id)
            .ValueGeneratedNever();

        builder.Property(n => n.Message)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(n => n.Title)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(n => n.UserId)
            .IsRequired();

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(x => x.UserId);

        builder.Property(n => n.CreatedAt)
            .IsRequired();
    }
}