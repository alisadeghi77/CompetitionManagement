using Api.Models;
using Application.Brackets.GetBracketMatches;
using Application.Matches.SetMatchWinner;
using Domain.Constant;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MatchController(ISender sender) : ControllerBase
{
    [HttpPost]
    [Authorize(Roles = $"{RoleConstant.Admin},{RoleConstant.Planner}")]
    public async Task<IActionResult> SetMatchWinner([FromBody] MatchWinnerRequest request)
    {
        await sender.Send(new SetMatchWinnerCommand(request.MatchId, request.ParticipantId));
        return Ok();
    }

    
    [HttpGet("{bracketKey}")]
    [Authorize(Roles = $"{RoleConstant.Admin},{RoleConstant.Planner}")]
    public async Task<IActionResult> GetBracketMatches([FromRoute] string bracketKey)
        => Ok(await sender.Send(new GetMatchesByBracketIdQuery(bracketKey)));


}