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
    public async Task<ActionResult<List<SkillResponse>>> GetSkillsByUserId(){
        var userId = GetCurrentUserId();
        if(userId == null)
        {
            return Unauthorized();
        }

        var skills= await SkillService.GetSkillsByUserId(userId.Value);
        return Ok(skills.Select(ToResponse).ToList());
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
    [HttpPut("{skillId:guid}")]
    public async Task<IActionResult> Update([FromRoute] Guid skillId,[FromBody] UpdateSkillRequest request){
        var userId = GetCurrentUserId();
        if(userId == null)
        {
            return Unauthorized();
        }
        await SkillService.Update(userId.Value,skillId,request);
        return Ok();
    }
    [HttpDelete("{skillId:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid skillId)
    {
        var userId = GetCurrentUserId();
        if(userId == null)
        {
            return Unauthorized();
        }
        await SkillService.Delete(userId.Value,skillId);
        return Ok();
    }

    private Guid? GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst("UserId")?.Value;
        return Guid.TryParse(userIdClaim, out var userId) ? userId : null;
    }

    private static SkillResponse ToResponse(SkillEntity skill)
    {
        return new SkillResponse(
            skill.Id,
            skill.Name,
            skill.Description,
            skill.Experience,
            Level: skill.Experience / 100 + 1,
            ExperienceForNextLevel: 100 - skill.Experience % 100 
        );
    }
}
