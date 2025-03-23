using Application.Files.GetFile;
using Application.Files.UploadFile;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[AllowAnonymous]
public class FilesController(ISender sender) : ControllerBase
{
    [HttpPost("upload")]
    public async Task<IActionResult> UploadFile(IFormFile? file)
        => Ok(await sender.Send(new UploadFileCommand(file)));

    [HttpGet("{ParticipantId}")]
    public async Task<FileResult> GetFileDetails(long id)
    {
        var result = await sender.Send(new GetFileByIdQuery(id));
        return File(result.BinaryContent!, GetContentType(result.FileName), result.FileName);
    }

    [HttpGet("fileName/{fileName}")]
    public async Task<FileResult> GetFileDetails(string fileName)
    {
        var result = await sender.Send(new GetFileByNameQuery(fileName));
        return new FileStreamResult(new MemoryStream(result.BinaryContent!), GetContentType(result.FileName));
    }

    private string GetContentType(string fileName)
        => !new FileExtensionContentTypeProvider().TryGetContentType(fileName, out var contentType)
            ? "application/octet-stream"
            : contentType;
}