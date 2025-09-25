using Application.Common;
using Application.Users.GetUsersByRole;
using Domain.Constant;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Users.GetCoaches;

public record GetUsersByRoleQuery(string PhoneNumber, string? Role) : IRequest<List<MinimalUserDto>>;

public class GetUsersByRoleQueryHandler(IApplicationDbContext dbContext) : IRequestHandler<GetUsersByRoleQuery, List<MinimalUserDto>>
{
    public async Task<List<MinimalUserDto>> Handle(GetUsersByRoleQuery request, CancellationToken cancellationToken)
    {
        var selectedRole = await dbContext.Roles
            .FirstOrDefaultAsync(w => w.NormalizedName == request.Role!.ToUpper(), cancellationToken);


        var query = dbContext.Users
            .Where(w => w.PhoneNumber!.Contains(request.PhoneNumber));

        if (selectedRole is not null)
            query = query.Where(w => w.Roles.Any(r => r.RoleId == selectedRole.Id));

        var result = await query
                        .Select(s => new MinimalUserDto(s.Id, s.FullName, s.UserName!))
                        .ToListAsync(cancellationToken);

        return result
            .Select(s => s with { PhoneNumber = MaskPhoneNumber(s.PhoneNumber) })
            .ToList();
    }

    private static string MaskPhoneNumber(string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber) || phoneNumber.Length < 8)
            return phoneNumber;

        return phoneNumber[..4] + "***" + phoneNumber[^4..];
    }
}