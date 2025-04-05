using Application.CompetitionBrackets.GenerateBrackets;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CompetitionBracketController(ISender sender) : ControllerBase
{
    [HttpGet("{competitionId}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetList(int competitionId)
    {
        await sender.Send(new GenerateBracketsCommand(competitionId));
        return Ok();
    }
}