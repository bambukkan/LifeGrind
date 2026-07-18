using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class SkillConfiguration : IEntityTypeConfiguration<SkillEntity>
{
    public void Configure(EntityTypeBuilder<SkillEntity> builder)
    {
        builder.HasKey(u => u.Id);

        builder.HasMany(s => s.Quests)
            .WithOne(q => q.Skill).HasForeignKey(q => q.SkillId);
    }
}