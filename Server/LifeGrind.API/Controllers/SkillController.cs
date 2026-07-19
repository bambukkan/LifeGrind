using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Authorize]
[Route ("Skills")]
public class SkillController : ControllerBase
{
    private readonly ISkillService SkillService;
    public SkillController(ISkillService _SkillService)
    {
        SkillService = _SkillService;
    }

    [HttpGet]
    public async Task<ActionResult<SkillEntity>> GetSkills(){
        var Skills = await SkillService.GetSkills();
        return Ok(Skills);
    }
    [HttpGet("by-userId")]
    public async Task<ActionResult<SkillEntity>> GetSkillsByUserId(){
        var userId = GetCurrentUserId();
        if(userId == null)
        {
            return Unauthorized();
        }

        var skills= await SkillService.GetSkillsByUserId(userId.Value);
        return Ok(skills);
    }
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] CreateSkillRequest request){
        var userId = GetCurrentUserId();
        if(userId == null)
        {
            return Unauthorized();
        }

        await SkillService.Add(userId.Value,request);
        return Ok();
    } // Временно будет приходить из клиенда айди пользователя, 
    // щас дозакончу контроллеры и сделаю атворизацию
    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] Guid skillId,[FromBody] UpdateSkillRequest request){
        await SkillService.Update(skillId,request);
        return Ok();
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] Guid skillId)
    {
        await SkillService.Delete(skillId);
        return Ok();
    }

    private Guid? GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst("UserId")?.Value;
        return Guid.TryParse(userIdClaim, out var userId) ? userId : null;
    }
}
