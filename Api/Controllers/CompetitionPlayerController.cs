using Api.ExtensionMethods;
using Api.Models;
using Application.Competitions.GetCompetitionList;
using Application.Competitions.RegisterParticipant;
using Domain.Constant;
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
    public async Task<IActionResult> CompetitionDefinition(RegisterParticipantRequest request)
    {
        var result = await sender.Send(new RegisterParticipantCommand(
            this.GetUserId()!,
            request.CoachId,
            request.CoachPhoneNumber,
            request.CompetitionId,
            request.Params));

        return Ok(result);
    }
}