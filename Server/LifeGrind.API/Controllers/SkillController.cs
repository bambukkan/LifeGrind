using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route ("Skills")]
public class SkillController : ControllerBase
{
    private readonly ISkillService SkillService;
    public SkillController(ISkillService _SkillService)
    {
        SkillService = _SkillService;
    }

    public async Task<ActionResult<SkillEntity>> GetSkills(){
        var Skills = await SkillService.GetSkills();
        return Ok(Skills);
    }
    public async Task<ActionResult<SkillEntity>> GetSkillsByUserId(Guid userId){
        var skills= await SkillService.GetSkillsByUserId(userId);
        return Ok(skills);
    }
    public async Task<IActionResult> Add(Guid userId,CreateSkillRequest request){
        await SkillService.Add(userId,request);
        return Ok();
    } // Временно будет приходить из клиенда айди пользователя, 
    // щас дозакончу контроллеры и сделаю атворизацию
    public async Task<IActionResult> Update(Guid SkillId,UpdateSkillRequest request){
        await SkillService.Update(SkillId,request);
        return Ok();
    }
    public async Task<IActionResult> Delete(Guid SkillId)
    {
        await SkillService.Delete(SkillId);
        return Ok();
    }
}