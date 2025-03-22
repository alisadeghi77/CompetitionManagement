using Application.Auth.CurrentUser;
using Application.Auth.Login;
using Application.Auth.Verify;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(ISender sender) : ControllerBase
{
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginCommand command)
    {
        await sender.Send(command);
        return Ok();
    }
    
    [HttpPost("verify")]
    [AllowAnonymous]
    public async Task<IActionResult> OtpLogin([FromBody] VerifyCommand command)
    {
        var token = await sender.Send(command);
        return Ok(token);
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> GetCurrentUser()
    {
        var user = await sender.Send(new GetCurrentUserQuery(User));
        return Ok(user);
    }
}