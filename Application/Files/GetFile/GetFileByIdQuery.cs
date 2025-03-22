using Application.Common;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Files.GetFile;

public record GetFileByIdQuery(long FileId) : IRequest<FileEntity>;

public class GetFileDetailsQueryHandler(IApplicationDbContext dbContext)
    : IRequestHandler<GetFileByIdQuery, FileEntity>
{
    public async Task<FileEntity> Handle(GetFileByIdQuery request, CancellationToken cancellationToken)
    {
        var fileEntity = await dbContext.Files.FirstOrDefaultAsync(w => w.Id == request.FileId, cancellationToken);
        if (fileEntity is null)
            throw new FileNotFoundException("فایل یافت نشد.");
        return fileEntity;
    }
}