using LifeGrind.CORE.Exceptions;

public class SkillService : ISkillService
{
    private readonly ISkillRepository skillRepository;

    public SkillService(ISkillRepository _SkillRepository)
    {
        skillRepository = _SkillRepository;
    }

    public async Task<List<SkillEntity>> GetSkills()
    {
        return await skillRepository.GetSkills();
    }

    public async Task<List<SkillEntity>> GetSkillsByUserId(Guid userId)
    {
        return await skillRepository.GetSkillsByUserId(userId);
    }

    public async Task Add(Guid userId, CreateSkillRequest request)
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

    public async Task Update(Guid userId, Guid skillId, UpdateSkillRequest request)
    {
        var skill = await skillRepository.GetSkill(skillId);
        if (skill == null)
        {
            throw new SkillNotExistException();
        }

        if (skill.UserId != userId)
        {
            throw new EntityNotFoundException("Attempt to update another user's skill.");
        }

        await skillRepository.Update(skillId, request.Name, request.Description, request.Experience);
    }

    public async Task Delete(Guid userId, Guid skillId)
    {
        var skill = await skillRepository.GetSkill(skillId);
        if (skill == null)
        {
            throw new SkillNotExistException();
        }

        if (skill.UserId != userId)
        {
            throw new EntityNotFoundException("Attempt to delete another user's skill.");
        }

        await skillRepository.Delete(skillId);
    }
}
