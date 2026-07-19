using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Authorize]
[Route ("Quests")]
public class QuestController : ControllerBase
{
    private readonly IQuestService QuestService;
    public QuestController(IQuestService _QuestService)
    {
        QuestService = _QuestService;
    }

    [HttpGet]
    public async Task<ActionResult<QuestEntity>> GetQuests(){
        var Quests = await QuestService.GetQuests();
        return Ok(Quests);
    }
    [HttpGet("by-userId")]
    public async Task<ActionResult<SkillEntity>> GetQuestsByUserId(){
        var userId = GetCurrentUserId();
        if(userId == null)
        {
            return Unauthorized();
        }
        var skills= await QuestService.GetQuestsByUserId(userId.Value);
        return Ok(skills);
    }
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] CreateQuestRequest request){
        var userId = GetCurrentUserId();
        if(userId == null)
        {
            return Unauthorized();
        }

        await QuestService.Add(userId.Value,request);
        return Ok();
    }
    [HttpPut("{questId:guid}")]
    public async Task<IActionResult> Update([FromRoute] Guid questId,[FromBody] UpdateQuestRequest request){
        var userId = GetCurrentUserId();
        if(userId == null)
        {
            return Unauthorized();
        }
        await QuestService.Update(userId.Value,questId,request);
        return Ok();
    }
    [HttpDelete("{questId:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid questId)
    {
        var userId = GetCurrentUserId();
        if(userId == null)
        {
            return Unauthorized();
        }
        await QuestService.Delete(userId.Value,questId);
        return Ok();
    }
    [HttpPost("{questId:guid}/complete")]
    public async Task<IActionResult> CompleteQuest([FromRoute] Guid questId)
    {
        var userId = GetCurrentUserId();
        if(userId == null)
        {
            return Unauthorized();
        }
        await QuestService.CompleteQuest(userId.Value,questId);
        return Ok();
    } 
    [HttpPost("{questId:guid}/cancel")]
    public async Task<IActionResult> CancelQuest([FromRoute] Guid questId)
    {
        var userId = GetCurrentUserId();
        if(userId == null)
        {
            return Unauthorized();
        }
        await QuestService.CancelQuest(userId.Value,questId);
        return Ok();
    } 

    private Guid? GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst("UserId")?.Value;
        return Guid.TryParse(userIdClaim, out var userId) ? userId : null;
    }
}
