namespace CompetitionManagement.Domain.Constant;

public static class RoleConstant
{
    public const string Admin = "ADMIN";
    public const string Planner = "PLANNER";
    public const string Coach = "COACH";
    public const string Athlete = "ATHLETE";

    public static string GetRole(string roleName) =>
        roleName.ToUpper() switch
        {
            "ADMIN" => Admin,
            "PLANNER" => Planner,
            "COACH" => Coach,
            "ATHLETE" => Athlete,
            _ => ""
        };
}