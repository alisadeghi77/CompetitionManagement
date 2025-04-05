using Api.Models;
using Application.CompetitionBracketMatches.SetMatchWinner;
using Domain.Constant;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CompetitionBracketMatchController(ISender sender) : ControllerBase
{
    [HttpPost]
    [Authorize(Roles = $"{RoleConstant.Admin},{RoleConstant.Planner}")]
    public async Task<IActionResult> SetMatchWinner([FromBody] MatchWinnerRequest request)
    {
        await sender.Send(new SetMatchWinnerCommand(request.MatchId, request.ParticipantId));
        return Ok();
    }
}