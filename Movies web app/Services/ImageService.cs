using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Jpeg;
using Microsoft.Net.Http.Headers;
using Core.Enums;
namespace Movies_web_app.Services
{
    public class ImageService : IImageService
    {
        private readonly IWebHostEnvironment _env;
        public ImageService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public async Task<string> UploadImageAsync(IFormFile image, string FolderName, ImageType imageType=ImageType.Original)
        {
            if (image == null || image.Length == 0)
            {
                return null;
            }
            string uploadsFolder = Path.Combine(_env.WebRootPath, "images", FolderName);
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            string uniqueFileName = Guid.NewGuid().ToString() + "_" + image.FileName;
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);
            using(var fileStream =image.OpenReadStream())
            using (var loadedImage = Image.Load(fileStream))
            {
                int width = 0;
                int height = 0;
                switch (imageType)
                {
                    case ImageType.Profile:
                        width = 500;
                        height = 500;
                        break;

                    case ImageType.Poster:
                        width = 600;
                        height = 0;
                        break;
                    case ImageType.Background:
                        width = 1920;
                        height = 0;
                        break;
                    default:
                        width = 0;
                        height = 0;
                        break;
                }
                if(width > 0 && height > 0)
                {
                    if (loadedImage.Width > width)
                    loadedImage.Mutate(x => x.Resize(new ResizeOptions
                    {
                        Size = new Size(width, height),
                        Mode = ResizeMode.Max
                    }));
                }
                var encoder = new JpegEncoder { Quality = 75 };
                await loadedImage.SaveAsJpegAsync(filePath, encoder);
            }
            return $"/images/{FolderName}/{uniqueFileName}";
        }
        public async Task DeleteImageAsync(string relativePath)
        {
            if (string.IsNullOrEmpty(relativePath))
            {
                return;
            }
            relativePath = relativePath.TrimStart('/');
            if (relativePath.StartsWith("images/", StringComparison.OrdinalIgnoreCase))
                relativePath = "Images/" + relativePath.Substring("images/".Length);
            string fullPath = Path.Combine(_env.WebRootPath, relativePath.Replace("/", Path.DirectorySeparatorChar.ToString()));
            if (File.Exists(fullPath))
            {
                await Task.Run(() => File.Delete(fullPath));
            }
        }
    }
}
