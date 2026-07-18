using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class QuestConfiguration : IEntityTypeConfiguration<QuestEntity>
{
    public void Configure(EntityTypeBuilder<QuestEntity> builder)
    {
        builder.HasKey(u => u.Id);
    }
}