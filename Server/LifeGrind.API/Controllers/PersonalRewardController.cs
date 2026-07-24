using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Authorize]
[Route("personalRewards")]
public class PersonalRewardController : ControllerBase
{
    private readonly IPersonalRewardService prService;

    public PersonalRewardController(IPersonalRewardService _prService)
    {
        prService = _prService;
    }

    [HttpGet]
    public async Task<ActionResult<List<PersonalRewardResponse>>> GetRewardsByUserId()
    {
        var userId = GetCurrentUserId();
        if(userId == null){
            return Unauthorized();
        }
        var pr = await prService.GetRewardsByUserId(userId.Value);
        return Ok(pr.Select(ToResponse).ToList());
    }
    [HttpPost]
    public async Task<IActionResult> AddPersonalReward(CreatePersonalRewardRequest request)
    {
        var userId = GetCurrentUserId();
        if(userId == null){
            return Unauthorized();
        }
        await prService.AddPersonalReward(userId.Value,request);
        return Ok();
    }
    [HttpPut("{pRewardId:guid}")]
    public async Task<IActionResult> UpdatePersonalReward([FromRoute] Guid pRewardId,
    UpdatePersonalRewardRequest request)
    {
        var userId = GetCurrentUserId();
        if(userId == null){
            return Unauthorized();
        }
        await prService.UpdatePersonalReward(userId.Value, pRewardId, request);
        return Ok();
    }
    [HttpDelete("{pRewardId:guid}")]
    public async Task<IActionResult> DeletePersonalReward([FromRoute] Guid pRewardId)
    {

        var userId = GetCurrentUserId();
        if(userId == null){
            return Unauthorized();
        }
        await prService.DeletePersonalReward(userId.Value, pRewardId);
        return Ok();
    }
    [HttpPost("{pRewardId:guid}/purchase")]
    public async Task<IActionResult> PurchasePersonalReward([FromRoute] Guid pRewardId)
    {
        var userId = GetCurrentUserId();
        if(userId == null){
            return Unauthorized();
        }
        await prService.PurchasePersonalReward(pRewardId,userId.Value);
        return Ok();
    }
    private Guid? GetCurrentUserId()
    {
        return Guid.TryParse(User.FindFirst("UserId")?.Value,out var userId) ? userId : null;
    }
    private static PersonalRewardResponse ToResponse(PersonalRewardEntity pr)
    {
        return new PersonalRewardResponse(
            pr.Id,
            pr.Name,
            pr.Description,
            pr.Cost
        );
    }
}
