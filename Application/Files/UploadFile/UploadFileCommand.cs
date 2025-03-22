using Application.Common;
using Domain.Entities;
using Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Files.UploadFile;

public record UploadFileCommand(IFormFile? File) : IRequest<FileDto>;

public class UploadFileCommandHandler(IApplicationDbContext dbContext) : IRequestHandler<UploadFileCommand, FileDto>
{
    public async Task<FileDto> Handle(UploadFileCommand request, CancellationToken cancellationToken)
    {
        if (request.File == null || request.File.Length == 0)
            throw new BadRequestException("فایل انتخاب نشده");

        using var memoryStream = new MemoryStream();
        await request.File.CopyToAsync(memoryStream, cancellationToken);
        var binaryContent = memoryStream.ToArray();
        var bas64Content = Convert.ToBase64String(binaryContent);

        var fileEntity = FileEntity.Create(bas64Content, binaryContent, $"{Guid.NewGuid()}-{request.File.FileName}");
        dbContext.Files.Add(fileEntity);
        await dbContext.SaveChangesAsync(cancellationToken);
        return new FileDto(fileEntity.Id, fileEntity.FileName);
    }
}