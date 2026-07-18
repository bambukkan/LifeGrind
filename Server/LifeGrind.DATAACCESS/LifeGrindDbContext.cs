
using Microsoft.EntityFrameworkCore;

public class LifeGrindDbContext(DbContextOptions<LifeGrindDbContext> options) : DbContext(options)
{
    public DbSet<UserEntity> Users {get;set;}
    public DbSet<SkillEntity> Skills {get;set;}
    public DbSet<QuestEntity> Quests {get;set;}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new QuestConfiguration());
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new SkillConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}
