public interface ISkillService
{
    Task<List<SkillEntity>> GetSkills();
    Task<List<SkillEntity>> GetSkillsByUserId(Guid userId);
    Task Add(SkillEntity skill);
    
    Task Update(Guid skillId,
        string name,string description,int experience);
    Task Delete(Guid skillId);
}