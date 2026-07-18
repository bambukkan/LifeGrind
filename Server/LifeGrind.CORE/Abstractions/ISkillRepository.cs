public interface ISkillRepository
{
    Task<List<SkillEntity>> GetSkills();
    Task<SkillEntity> Add();
    
    Task<SkillEntity> Update();
    Task<SkillEntity> Delete();
}