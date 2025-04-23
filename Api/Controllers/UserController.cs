using Api.Models;
using Application.Users.GetCoaches;
using Application.Users.Register;
using Domain.Constant;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController(ISender sender) : ControllerBase
{
    [HttpPut("Coach")]
    [Authorize($"{RoleConstant.Coach}")]
    public async Task<IActionResult> EditCoachInformation([FromBody] RegisterRequest request)
    {
        await sender.Send(new RegisterCommand(
            request.PhoneNumber,
            request.FirstName,
            request.LastName,
            RoleConstant.Planner));
        return Ok();
    }


    [HttpPost("Planner")]
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


    [HttpPost("Participant")]
    [AllowAnonymous]
    public async Task<IActionResult> RegisterParticipant([FromBody] RegisterRequest request)
    {
        await sender.Send(new RegisterCommand(
            request.PhoneNumber,
            request.FirstName,
            request.LastName,
            RoleConstant.Participant));
        return Ok();
    }


    [HttpGet("Coaches")]
    [Authorize]
    public async Task<IActionResult> GetCoaches([FromQuery] string phoneNumber)
    {
        return Ok(await sender.Send(new GetCoachesQuery(phoneNumber)));  
    }
}