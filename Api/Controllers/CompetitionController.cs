using Api.ExtensionMethods;
using Api.Models;
using Application.Competitions.CompetitionDefinition;
using Application.Competitions.GetCompetitionById;
using Application.Competitions.GetCompetitionList;
using Application.Competitions.UpdateParams;
using Domain.Constant;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CompetitionController(ISender sender) : ControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetList([FromQuery] GetCompetitionListQuery query) => Ok(await sender.Send(query));

    [HttpGet("{ParticipantId}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById([FromRoute] long id)
        => Ok(await sender.Send(new GetCompetitionByIdQuery(id)));

    [HttpPost]
    [Authorize(Roles = $"{RoleConstant.Admin},{RoleConstant.Planner}")]
    public async Task<IActionResult> CompetitionDefinition(CompetitionDefinitionRequest request) => Ok(
        await sender.Send(new CompetitionDefinitionCommand(
            this.GetUserId()!,
            request.CompetitionTitle,
            request.CompetitionDate,
            request.CompetitionAddress,
            request.LicenseFileId ?? 0,
            request.BannerFileId ?? 0)));


    [HttpPut("params/{ParticipantId}")]
    [Authorize(Roles = $"{RoleConstant.Admin}")]
    public async Task<IActionResult> UpdateCompetitionParamsInfo(
        [FromRoute] long id,
        [FromBody] List<CompetitionParam> param)
    {
        await sender.Send(new UpdateCompetitionParamsCommand(id, param));
        return Ok();
    }
}