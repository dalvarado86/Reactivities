using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class ActivityAttendeeConfiguration : IEntityTypeConfiguration<ActivityAttendee>
    {
        public void Configure(EntityTypeBuilder<ActivityAttendee> builder)
        {
            builder.HasKey(x => new { x.ApplicationUserId, x.ActivityId });
            
            builder.HasOne(u => u.ApplicationUser)
                .WithMany(a => a.Activities)
                .HasForeignKey(x => x.ApplicationUserId);

            builder.HasOne(u => u.Activity)
                .WithMany(a => a.Attendees)
                .HasForeignKey(x => x.ActivityId);
        }
    }
}