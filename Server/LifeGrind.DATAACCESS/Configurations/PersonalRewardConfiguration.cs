using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class PersonalRewardConfiguration : IEntityTypeConfiguration<PersonalRewardEntity>
{
    public void Configure(EntityTypeBuilder<PersonalRewardEntity> builder)
    {
        builder.HasKey(u => u.Id);

        builder.HasOne(p => p.User)
            .WithMany(u => u.PersonalRewards)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
