public interface ISkillRepository
{

    Task<SkillEntity?> GetSkill(Guid skillId);
    Task<List<SkillEntity>> GetSkillsByUserId(Guid userId);
    Task Add(SkillEntity skill);

    Task Update(Guid skillId,
        string name, string description, int experience);
    Task AddExperience(Guid skillId, int experience);
    Task Delete(Guid skillId);
}
