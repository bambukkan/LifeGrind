public interface ISkillRepository
{
    Task<List<SkillEntity>> GetSkills();
    Task<List<SkillEntity>> GetSkillsByUserId(Guid userId);
    Task<SkillEntity> Add(SkillEntity skill);
    
    Task<SkillEntity> Update(Guid skillId,
        string name,string description,int experience);
    Task<SkillEntity> Delete(Guid skillId);
}