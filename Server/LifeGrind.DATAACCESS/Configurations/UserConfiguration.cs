using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class UserConfiguration : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.HasKey(u => u.Id);

        builder.HasIndex(u => u.Email).IsUnique();

        builder.HasMany(u => u.Skills)
            .WithOne(s => s.User).HasForeignKey(s => s.UserId);

        builder.HasMany(u => u.Quests)
            .WithOne(s => s.User).HasForeignKey(s => s.UserId);
        builder.HasMany(u => u.PersonalRewards)
            .WithOne(s => s.User).HasForeignKey(s => s.UserId);
    }
}