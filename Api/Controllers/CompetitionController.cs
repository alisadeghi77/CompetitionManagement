using Api.ExtensionMethods;
using Api.Models;
using Application.Competitions.ChangeCompetitionVisibility;
using Application.Competitions.CompetitionDefinition;
using Application.Competitions.GetCompetitionById;
using Application.Competitions.GetCompetitionList;
using Application.Competitions.StartCompetition;
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

    [HttpGet("{id}")]
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


    [HttpPut("params/{id}")]
    [Authorize(Roles = $"{RoleConstant.Admin}")]
    public async Task<IActionResult> UpdateCompetitionParamsInfo(
        [FromRoute] long id,
        [FromBody] List<CompetitionParam> param)
    {
        await sender.Send(new UpdateCompetitionParamsCommand(id, param));
        return Ok();
    }
    
    [HttpPatch("start-registration/{id}")]
    [Authorize(Roles = $"{RoleConstant.Admin}")]
    public async Task<IActionResult> StartRegistrationCompetition([FromRoute] long id)
    {
        await sender.Send(new StartCompetitionCommand(id));
        return Ok();
    }
    
    [HttpPatch("change-visibility/{id}")]
    [Authorize(Roles = $"{RoleConstant.Admin}")]
    public async Task<IActionResult> ChangeCompetitionVisibility([FromRoute] long id)
    {
        await sender.Send(new ChangeCompetitionVisibilityCommand(id));
        return Ok();
    }
    
    [HttpPatch("change-registration-status/{id}")]
    [Authorize(Roles = $"{RoleConstant.Admin}")]
    public async Task<IActionResult> ChangeCompetitionRegistrationStatus([FromRoute] long id)
    {
        await sender.Send(new ChangeCompetitionRegistrationCommand(id));
        return Ok();
    }
}