using CompetitionManagement.Domain.Common;
using CompetitionManagement.Domain.Validations;
using FluentValidation;

namespace CompetitionManagement.Domain.Entities;

public class FileEntity : BaseAuditableEntity
{
    public string? Base64Content { get; private set; }
    public byte[]? BinaryContent { get; private set; }
    public string FileName { get; private set; }

    private FileEntity(string? base64Content, byte[]? binaryContent, string fileName)
    {
        Base64Content = base64Content;
        BinaryContent = binaryContent;
        FileName = fileName;
    }

    public static FileEntity Create(string? base64Content, byte[]? binaryContent, string fileName)
    {
        var file = new FileEntity(base64Content, binaryContent, fileName);
        new FilesValidator().ValidateAndThrow(file);
        return file;
    }
}
