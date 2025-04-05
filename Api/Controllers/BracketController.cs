using Application.Brackets.DeleteBrackets;
using Application.Brackets.GenerateBracketByKey;
using Application.Brackets.GenerateBrackets;
using Application.Brackets.GetBracketsWinnerReport;
using Application.Brackets.GetCoachMedalReport;
using Domain.Constant;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BracketController(ISender sender) : ControllerBase
{
 
    [HttpGet("brackets-report/{bracketId}")]
    [Authorize(Roles = $"{RoleConstant.Admin},{RoleConstant.Planner}")]
    public async Task<IActionResult> GetBracketsWinnerReport(long bracketId) 
        => Ok(await sender.Send(new GetBracketsWinnerReportQuery(bracketId)));
    
    [HttpGet("coach-score-report/{bracketId}")]
    [Authorize(Roles = $"{RoleConstant.Admin},{RoleConstant.Planner}")]
    public async Task<IActionResult> GetCoachScoreReport(long bracketId) 
        => Ok(await sender.Send(new GetCoachMedalReportQuery(bracketId)));

    [HttpPost("{competitionId}")]
    [Authorize(Roles = $"{RoleConstant.Admin},{RoleConstant.Planner}")]
    public async Task<IActionResult> GenerateBrackets([FromRoute] int competitionId)
    {
        await sender.Send(new GenerateBracketsCommand(competitionId));
        return Ok();
    }

    [HttpPost("{competitionId}/{bracketKey}")]
    [Authorize(Roles = $"{RoleConstant.Admin},{RoleConstant.Planner}")]
    public async Task<IActionResult> GenerateBracketByKey([FromRoute] int competitionId, [FromRoute] string bracketKey)
    {
        await sender.Send(new GenerateBracketByKeyCommand(competitionId, bracketKey));
        return Ok();
    }


    [HttpDelete("{competitionId}")]
    [Authorize(Roles = $"{RoleConstant.Admin},{RoleConstant.Planner}")]
    public async Task<IActionResult> DeleteBrackets([FromRoute] int competitionId)
    {
        await sender.Send(new DeleteBracketsCommand(competitionId));
        return Ok();
    }

    [HttpDelete("{competitionId}/{bracketKey}")]
    [Authorize(Roles = $"{RoleConstant.Admin},{RoleConstant.Planner}")]
    public async Task<IActionResult> DeleteBrackets([FromRoute] int competitionId, [FromRoute] string bracketKey)
    {
        await sender.Send(new DeleteBracketsCommand(competitionId));
        return Ok();
    }
}