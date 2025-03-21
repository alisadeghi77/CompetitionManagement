using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace CompetitionManagement.WebApi.ExtensionMethods;

public static class ControllerMethods
{
    public static string? GetUserId(this ControllerBase controllerBase) 
        => controllerBase.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? null;
}