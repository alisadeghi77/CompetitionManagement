using CompetitionManagement.Application.Auth.CurrentUser;
using CompetitionManagement.Application.Auth.Login;
using CompetitionManagement.Application.Auth.Register;
using CompetitionManagement.Application.Auth.VerifyCommand;
using CompetitionManagement.Domain.Constant;
using CompetitionManagement.WebApi.ApiModel;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CompetitionManagement.WebApi.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginCommand command)
    {
        await _mediator.Send(command);
        return Ok();
    }


    [HttpPost("register-planner")]
    [AllowAnonymous]
    public async Task<IActionResult> RegisterPlanner([FromBody] RegisterRequest request)
    {
        await _mediator.Send(new RegisterCommand(
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
        await _mediator.Send(new RegisterCommand(
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
        var token = await _mediator.Send(command);
        return Ok(token);
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> GetCurrentUser()
    {
        var user = await _mediator.Send(new GetCurrentUserQuery(User));
        return Ok(user);
    }
}