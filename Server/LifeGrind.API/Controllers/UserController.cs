using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("Users")]
public class UserController : ControllerBase
{
    private readonly IUserService userService;

    public UserController(IUserService _userService)
    {
        userService = _userService;
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<UserResponse>> GetMe()
    {
        var userId = GetCurrentUserId();
        if (userId == null)
        {
            return Unauthorized();
        }
        var user = await userService.GetMe(userId.Value);

        return Ok(ToResponse(user));
    }

    [HttpPost]
    public async Task<IActionResult> Register([FromBody] CreateUserRequest request)
    {
        var token = await userService.Register(request);

        Response.Cookies.Append(
            "Access-cookies",
            token,
            CreateAuthCookieOptions());

        return Ok();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserRequest request)
    {
        var token = await userService.Login(request);

        Response.Cookies.Append(
            "Access-cookies",
            token,
            CreateAuthCookieOptions());

        return Ok();
    }
    [HttpDelete("logout")]
    public async Task<IActionResult> Logout()
    {
        Response.Cookies.Delete("Access-cookies");

        return Ok();
    }

    [HttpPut]
    [Authorize]
    public async Task<IActionResult> Update([FromBody] UpdateUserRequest request)
    {
        var userId = GetCurrentUserId();
        if (userId == null)
        {
            return Unauthorized();
        }

        await userService.Update(userId.Value, request);
        return Ok();
    }

    [HttpDelete]
    [Authorize]
    public async Task<IActionResult> Delete()
    {
        var userId = GetCurrentUserId();
        if (userId == null)
        {
            return Unauthorized();
        }

        await userService.Delete(userId.Value);
        return Ok();
    }

    private CookieOptions CreateAuthCookieOptions()
    {
        return new CookieOptions
        {
            HttpOnly = true,
            Secure = Request.IsHttps,
            SameSite = SameSiteMode.Lax
        };
    }

    private Guid? GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst("UserId")?.Value;
        return Guid.TryParse(userIdClaim, out var userId) ? userId : null;
    }

    private static UserResponse ToResponse(UserEntity user)
    {
        return new UserResponse(
            user.Id,
            user.Name,
            user.Email,
            user.TotalExperience,
            user.Coins,
            Level: user.TotalExperience / 100 + 1,
            ExperienceForNextLevel: 100 - user.TotalExperience % 100 
        );
    }
}
