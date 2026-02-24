using Core.Enums;

namespace Movies_web_app.Services
{
    public interface IImageService
    {
        Task<string> UploadImageAsync(IFormFile image, string FolderName, ImageType imageType=ImageType.Original);
        Task DeleteImageAsync(string relativePath);
    }
}
