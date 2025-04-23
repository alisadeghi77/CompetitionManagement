using Application.Common;
using Domain.Constant;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Users.GetCoaches;

public record GetCoachesQuery(string PhoneNumber) : IRequest<List<CoachDto>>;

public class GetCoachesQueryHandler(IApplicationDbContext dbContext) : IRequestHandler<GetCoachesQuery, List<CoachDto>>
{
    public async Task<List<CoachDto>> Handle(GetCoachesQuery request, CancellationToken cancellationToken)
    {
        var coachRole = await dbContext.Roles
            .FirstAsync(w => w.NormalizedName == RoleConstant.Coach, cancellationToken);

        var result = await dbContext.Users
            .Where(w => w.Roles.Any(r => r.RoleId == coachRole.Id))
            .Where(w => w.PhoneNumber!.Contains(request.PhoneNumber))
            .Select(s => new CoachDto(s.Id, s.FullName, s.UserName!))
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