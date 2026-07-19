public interface ISkillRepository
{
    Task<List<SkillEntity>> GetSkills();
    Task<SkillEntity?> GetSkill(Guid skillId);
    Task<SkillEntity?> GetSkillByUserId(Guid userId);
    Task<List<SkillEntity>> GetSkillsByUserId(Guid userId);
    Task Add(SkillEntity skill);

    Task Update(Guid skillId,
        string name, string description, int experience);
    Task Delete(Guid skillId);
}
