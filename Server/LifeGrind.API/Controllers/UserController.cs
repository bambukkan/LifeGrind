using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route ("Users")]
public class UserController : ControllerBase
{
    private readonly IUserService userService;
    public UserController(IUserService _userService)
    {
        userService = _userService;
    }
    public async Task<ActionResult<UserEntity>> GetUsers(){
        var users = await userService.GetUsers();
        return Ok(users);
    }
    public async Task<IActionResult> Add(CreateUserRequest request){
        await userService.Add(request);
        return Ok();
    }
    public async Task<IActionResult> Update(Guid userId,UpdateUserRequest request){
        await userService.Update(userId,request);
        return Ok();
    }
    public async Task<IActionResult> UpdateUserExpAndCoins(Guid userId,UpdateUserExpAndCoinsRequest request)
    {
        await userService.UpdateUserExpAndCoins(userId,request);
        return Ok();
    }
    public async Task<IActionResult> Delete(Guid userId)
    {
        await userService.Delete(userId);
        return Ok();
    }
}
