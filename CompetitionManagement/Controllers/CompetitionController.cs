using CompetitionManagement.Application.Competitions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CompetitionManagement.Controllers;

// [Authorize]
[ApiController]
[Route("api/[controller]")]
public class CompetitionController(ISender sender) : ControllerBase
{
        [HttpGet]
        public async Task<IActionResult> GetList([FromQuery] GetCompetitionListRequest request)
                => Ok(await sender.Send(request));

        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] long id)
        {
                var result = await sender.Send(new GetCompetitionByIdRequest(id));
                return result is null ? BadRequest() : Ok(result);
        }
}