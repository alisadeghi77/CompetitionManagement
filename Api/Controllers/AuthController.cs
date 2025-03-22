using Api.Models;
using Application.Auth.CurrentUser;
using Application.Auth.Login;
using Application.Auth.Register;
using Application.Auth.Verify;
using Domain.Constant;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/auth")]
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


    [HttpPost("register-planner")]
    [AllowAnonymous]
    public async Task<IActionResult> RegisterPlanner([FromBody] RegisterRequest request)
    {
        await sender.Send(new RegisterCommand(
            request.PhoneNumber,
            request.FirstName,
            request.LastName,
            RoleConstant.Planner));
        return Ok();
    }
    
    [HttpPost("register-player")]
    [AllowAnonymous]
    public async Task<IActionResult> RegisterPlayer([FromBody] RegisterRequest request)
    {
        await sender.Send(new RegisterCommand(
            request.PhoneNumber,
            request.FirstName,
            request.LastName,
            RoleConstant.Player));
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