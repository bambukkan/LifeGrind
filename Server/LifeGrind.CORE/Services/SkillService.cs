public class SkillService : ISkillService
{
    private readonly ISkillRepository skillRepository;
    public SkillService(ISkillRepository _SkillRepository)
    {
        skillRepository = _SkillRepository;
    }
    public async Task<List<SkillEntity>> GetSkills(){
        return await skillRepository.GetSkills();
    }
    public async Task<List<SkillEntity>> GetSkillsByUserId(Guid userId){
        return await skillRepository.GetSkillsByUserId(userId);
    }
    public async Task Add(Guid userId,CreateSkillRequest request)
    {
        var skill = new SkillEntity()
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            Experience = request.Experience,
            UserId = userId
        };
        await skillRepository.Add(skill);
    }

    
    public async Task Update(Guid skillId, UpdateSkillRequest request)
    {
        await skillRepository.Update(skillId,request.Name,request.Description,request.Experience);
    }
    public async Task Delete(Guid skillId){
        await skillRepository.Delete(skillId);
    }

    // Все проверки тут будут с FV, так что тут по идее мне и нечего писать
}