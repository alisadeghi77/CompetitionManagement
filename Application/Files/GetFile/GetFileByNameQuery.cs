using Application.Common;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Files.GetFile;

public record GetFileByNameQuery(string FileName) : IRequest<FileEntity>;

public class GetFileByNameQueryHandler(IApplicationDbContext dbContext)
    : IRequestHandler<GetFileByNameQuery, FileEntity>
{
    public async Task<FileEntity> Handle(GetFileByNameQuery request, CancellationToken cancellationToken)
    {
        var fileEntity = await dbContext.Files
            .FirstOrDefaultAsync(w => w.FileName == request.FileName, cancellationToken);
        
        if (fileEntity is null)
            throw new FileNotFoundException("فایل یافت نشد.");
        
        return fileEntity;
    }
}