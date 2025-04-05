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
    public async Task<IActionResult> Verify([FromBody] VerifyCommand command) 
        => Ok(await sender.Send(command));

    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> GetCurrentUser() 
        => Ok(await sender.Send(new GetCurrentUserQuery(User)));
}