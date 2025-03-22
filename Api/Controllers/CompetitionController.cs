using Api.ExtensionMethods;
using Api.Models;
using Application.Competitions.CompetitionDefinition;
using Application.Competitions.GetCompetitionById;
using Application.Competitions.GetCompetitionList;
using Domain.Constant;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

// [Authorize]
[ApiController]
[Route("api/[controller]")]
public class CompetitionController(ISender sender) : ControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetList([FromQuery] GetCompetitionListQuery query)
        => Ok(await sender.Send(query));


    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById([FromRoute] long id)
    {
        var result = await sender.Send(new GetCompetitionByIdQuery(id));
        return result is null ? BadRequest() : Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = $"{RoleConstant.Admin},{RoleConstant.Planner}")]
    public async Task<IActionResult> CompetitionDefinition(CompetitionDefinitionRequest request)
    {
        var result = await sender.Send(new CompetitionDefinitionCommand(
            this.GetUserId()!,
            request.CompetitionTitle,
            request.CompetitionDate,
            request.CompetitionAddress,
            request.LicenseFileId ?? 0,
            request.BannerFileId ?? 0));

        return Ok(result);
    }
}