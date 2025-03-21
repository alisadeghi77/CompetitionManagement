using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CompetitionManagement.WebApi.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class HomeController : ControllerBase
{
        [HttpGet]
        public IActionResult Index2() => Ok(new { });
}