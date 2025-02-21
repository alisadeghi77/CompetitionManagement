using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CompetitionManagement.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class HomeController : ControllerBase
{
        [HttpGet]
        public IActionResult Index2() => Ok(new { });
}