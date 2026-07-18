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
    public async Task Add(SkillEntity skill)
    {
        await skillRepository.Add(skill);
    }
    
    public async Task Update(Guid skillId,
        string name,string description,int experience)
    {
        await skillRepository.Update(skillId,name,description,experience);
    }
    public async Task Delete(Guid skillId){
        await skillRepository.Delete(skillId);
    }
}