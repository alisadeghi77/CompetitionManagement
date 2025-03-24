using Api.ExtensionMethods;
using Api.Models;
using Application.Participants.RegisterChangeStatus;
using Application.Participants.RegisterParticipant;
using Domain.Constant;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ParticipantController(ISender sender) : ControllerBase
{
    [HttpGet]
    [Authorize(Roles = $"{RoleConstant.Admin},{RoleConstant.Planner}")]
    public async Task<IActionResult> GetList() => throw new NotImplementedException();


    [HttpPost]
    [Authorize(Roles = $"{RoleConstant.Participant}")]
    public async Task<IActionResult> CompetitionDefinition(RegisterParticipantRequest request) =>
        Ok(await sender.Send(new RegisterParticipantCommand(
            this.GetUserId()!,
            request.CoachId,
            request.CoachPhoneNumber,
            request.CompetitionId,
            request.Params)));


    [HttpPatch("approve/{ParticipantId}")]
    [Authorize(Roles = $"{RoleConstant.Planner},{RoleConstant.Admin}")]
    public async Task<IActionResult> RegisterApprove([FromRoute] long id)
    {
        await sender.Send(new ParticipantRegisterChangeStatusCommand(id, RegisterStatus.Approved));
        return Ok();
    }
    
    
    [HttpPatch("reject/{ParticipantId}")]
    [Authorize(Roles = $"{RoleConstant.Planner},{RoleConstant.Admin}")]
    public async Task<IActionResult> RegisterReject([FromRoute] long id)
    {
        await sender.Send(new ParticipantRegisterChangeStatusCommand(id, RegisterStatus.Rejected));
        return Ok();
    }
}