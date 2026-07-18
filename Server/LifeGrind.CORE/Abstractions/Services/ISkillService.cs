public interface ISkillService
{
    Task<List<SkillEntity>> GetSkills();
    Task<List<SkillEntity>> GetSkillsByUserId(Guid userId);
    Task Add(Guid userId, CreateSkillRequest request);
    
    Task Update(Guid skillId, UpdateSkillRequest request);
    Task Delete(Guid skillId);
}