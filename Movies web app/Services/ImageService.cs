namespace Movies_web_app.Services
{
    public class ImageService : IImageService
    {
        private readonly IWebHostEnvironment _env;
        public ImageService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public async Task<string> UploadImageAsync(IFormFile image, string FolderName)
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
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }
            return uniqueFileName;
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
