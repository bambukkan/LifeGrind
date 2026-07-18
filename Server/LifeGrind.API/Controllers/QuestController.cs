using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route ("Quests")]
public class QuestController : ControllerBase
{
    private readonly IQuestService QuestService;
    public QuestController(IQuestService _QuestService)
    {
        QuestService = _QuestService;
    }

    public async Task<ActionResult<QuestEntity>> GetQuests(){
        var Quests = await QuestService.GetQuests();
        return Ok(Quests);
    }
    public async Task<ActionResult<SkillEntity>> GetQuestByUserId(Guid userId){
        var skills= await QuestService.GetQuestsByUserId(userId);
        return Ok(skills);
    }
    public async Task<IActionResult> Add(Guid userId,CreateQuestRequest request){
        await QuestService.Add(userId,request);
        return Ok();
    }
    public async Task<IActionResult> Update(Guid QuestId,UpdateQuestRequest request){
        await QuestService.Update(QuestId,request);
        return Ok();
    }
    public async Task<IActionResult> Delete(Guid QuestId)
    {
        await QuestService.Delete(QuestId);
        return Ok();
    }
    public async Task<IActionResult> CompleteQuest(Guid QuestId)
    {
        await QuestService.CompleteQuest(QuestId);
        return Ok();
    } 
    public async Task<IActionResult> CancelQuest(Guid QuestId)
    {
        await QuestService.CancelQuest(QuestId);
        return Ok();
    } 
}