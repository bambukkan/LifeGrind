public interface ISkillRepository
{
    Task<List<SkillEntity>> GetSkills();
    Task<List<SkillEntity>> GetSkillsByUserId(Guid userId);
    Task<SkillEntity> Add();
    
    Task<SkillEntity> Update();
    Task<SkillEntity> Delete();
}