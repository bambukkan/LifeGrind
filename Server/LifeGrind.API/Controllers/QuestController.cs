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
    public async Task<ActionResult<SkillEntity>> GetQuestByUserId(){
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
    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] Guid questId,[FromBody] UpdateQuestRequest request){
        await QuestService.Update(questId,request);
        return Ok();
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] Guid questId)
    {
        await QuestService.Delete(questId);
        return Ok();
    }
    [HttpPost("{id}/complete")]
    public async Task<IActionResult> CompleteQuest([FromRoute] Guid questId)
    {
        await QuestService.CompleteQuest(questId);
        return Ok();
    } 
    [HttpPost("{id}/cancel")]
    public async Task<IActionResult> CancelQuest([FromRoute] Guid questId)
    {
        await QuestService.CancelQuest(questId);
        return Ok();
    } 

    private Guid? GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst("UserId")?.Value;
        return Guid.TryParse(userIdClaim, out var userId) ? userId : null;
    }
}
