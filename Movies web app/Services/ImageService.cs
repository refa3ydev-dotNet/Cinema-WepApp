using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Jpeg;
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

        public async Task<string> UploadImageAsync(IFormFile image, string folderName, ImageType imageType = ImageType.Original)
        {
            if (image == null || image.Length == 0)
            {
                return null;
            }

            string uploadsFolder = Path.Combine(_env.WebRootPath, "images", folderName);
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            string extension = Path.GetExtension(image.FileName).ToLowerInvariant();
            string uniqueFileName = $"{Guid.NewGuid()}{extension}";
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using var fileStream = image.OpenReadStream();
            using var loadedImage = Image.Load(fileStream);

            int width = imageType switch
            {
                ImageType.Profile => 500,
                ImageType.Poster => 600,
                ImageType.Background => 1920,
                _ => 0
            };

            int height = imageType == ImageType.Profile ? 500 : 0;

            if (width > 0 && loadedImage.Width > width)
            {
                loadedImage.Mutate(x => x.Resize(new ResizeOptions
                {
                    Size = new Size(width, height),
                    Mode = ResizeMode.Max
                }));
            }

            var encoder = new JpegEncoder { Quality = 75 };
            await loadedImage.SaveAsJpegAsync(filePath, encoder);

            return $"/images/{folderName}/{uniqueFileName}";
        }

        public async Task DeleteImageAsync(string relativePath)
        {
            if (string.IsNullOrEmpty(relativePath))
            {
                return;
            }

            relativePath = relativePath.TrimStart('/');

            if (relativePath.StartsWith("images/", StringComparison.OrdinalIgnoreCase))
            {
                relativePath = relativePath.Substring("images/".Length - 1);
            }

            string fullPath = Path.Combine(_env.WebRootPath, relativePath.Replace('/', Path.DirectorySeparatorChar));
            var fullFilePath = Path.GetFullPath(fullPath);
            var webRootPath = Path.GetFullPath(_env.WebRootPath);

            if (!fullFilePath.StartsWith(webRootPath, StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException("Invalid path - access outside web root is not allowed");
            }

            if (File.Exists(fullFilePath))
            {
                try
                {
                    await Task.Run(() => File.Delete(fullFilePath));
                }
                catch (Exception ex)
                {
                    // Log the exception but don't expose details to caller
                    System.Diagnostics.Debug.WriteLine($"Error deleting file: {ex.Message}");
                }
            }
        }
    }
}
