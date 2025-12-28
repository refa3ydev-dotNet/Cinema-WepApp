namespace Movies_web_app.Services
{
    public interface IImageService
    {
        Task<string> UploadImageAsync(IFormFile image, string FolderName);
        Task DeleteImageAsync(string relativePath);
    }
}
