using Api.ExtensionMethods;
using Api.Models;
using Application.Participants.GetParticipantList;
using Application.Participants.RegisterChangeStatus;
using Application.Participants.RegisterParticipant;
using Application.Participants.RegisterParticipantByAdmin;
using Domain.Constant;
using Domain.Contracts;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ParticipantController(ISender sender, ICurrentUser currentUser) : ControllerBase
{
    [HttpGet]
    [Authorize(Roles = $"{RoleConstant.Admin},{RoleConstant.Planner}")]
    public async Task<IActionResult> GetList([FromQuery] long competitionId)
        => Ok(await sender.Send(new GetParticipantListQuery(competitionId, currentUser.UserId!, currentUser.Roles)));

    [HttpPost]
    [Authorize(Roles = $"{RoleConstant.Participant}")]
    public async Task<IActionResult> Register(RegisterParticipantRequest request) =>
        Ok(await sender.Send(new RegisterParticipantCommand(
            this.GetUserId()!,
            request.CoachId,
            request.CoachPhoneNumber,
            request.CompetitionId,
            request.Params)));

            
    [HttpPost("by-admin")]
    [Authorize(Roles = $"{RoleConstant.Admin}")]
    public async Task<IActionResult> RegisterByAdmin(RegisterParticipantByAdminCommand request) => Ok(await sender.Send(request));


    [HttpPatch("approve/{participantId}")]
    [Authorize(Roles = $"{RoleConstant.Planner},{RoleConstant.Admin}")]
    public async Task<IActionResult> RegisterApprove([FromRoute] long participantId)
    {
        await sender.Send(new ParticipantRegisterChangeStatusCommand(participantId, RegisterStatus.Approved));
        return Ok();
    }


    [HttpPatch("reject/{participantId}")]
    [Authorize(Roles = $"{RoleConstant.Planner},{RoleConstant.Admin}")]
    public async Task<IActionResult> RegisterReject([FromRoute] long participantId)
    {
        await sender.Send(new ParticipantRegisterChangeStatusCommand(participantId, RegisterStatus.Rejected));
        return Ok();
    }
}