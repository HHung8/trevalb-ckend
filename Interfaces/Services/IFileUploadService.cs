namespace TrevalApp.Interfaces.Services;

public interface IFileUploadService
{
    Task<string> UploadImageAsync(Stream stream, string fileName, string folder);
    Task DeleteImageAsync(string publicId);
}