using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;

namespace ProgramacionWeb3.PropertiesManager.Web.Services
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _env;
        private const string ReceiptsFolder = "uploads/receipts";
        private static readonly HashSet<string> _allowedExtensions = new() { ".pdf", ".jpg", ".jpeg", ".png" };

        public FileService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public async Task<string> SaveReceiptAsync(IFormFile file)
        {
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!_allowedExtensions.Contains(extension))
                throw new InvalidOperationException($"Tipo de archivo no permitido. Use: PDF, JPG o PNG.");

            var uploadsPath = Path.Combine(_env.WebRootPath, ReceiptsFolder);
            if (!Directory.Exists(uploadsPath))
                Directory.CreateDirectory(uploadsPath);

            var fileName = $"{Guid.NewGuid()}{extension}";
            var fullPath = Path.Combine(uploadsPath, fileName);

            using var stream = new FileStream(fullPath, FileMode.Create);
            await file.CopyToAsync(stream);

            return $"/{ReceiptsFolder}/{fileName}";
        }

        public void DeleteReceipt(string? relativePath)
        {
            var fullPath = GetReceiptPhysicalPath(relativePath);
            if (fullPath != null && File.Exists(fullPath))
                File.Delete(fullPath);
        }

        public string? GetReceiptPhysicalPath(string? relativePath)
        {
            if (string.IsNullOrEmpty(relativePath)) return null;
            return Path.Combine(_env.WebRootPath, relativePath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
        }
    }
}
