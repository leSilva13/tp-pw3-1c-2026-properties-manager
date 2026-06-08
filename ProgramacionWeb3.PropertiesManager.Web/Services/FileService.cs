using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;

namespace ProgramacionWeb3.PropertiesManager.Web.Services
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _env;
        private const string ReceiptsFolder = "uploads/receipts";

        public FileService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public async Task<string> SaveReceiptAsync(IFormFile file)
        {
            var uploadsPath = Path.Combine(_env.WebRootPath, ReceiptsFolder);

            if (!Directory.Exists(uploadsPath))
                Directory.CreateDirectory(uploadsPath);

            var extension = Path.GetExtension(file.FileName);
            var fileName = $"{Guid.NewGuid()}{extension}";
            var fullPath = Path.Combine(uploadsPath, fileName);

            using var stream = new FileStream(fullPath, FileMode.Create);
            await file.CopyToAsync(stream);

            return $"/{ReceiptsFolder}/{fileName}";
        }

        public void DeleteReceipt(string? relativePath)
        {
            if (string.IsNullOrEmpty(relativePath)) return;

            var fullPath = Path.Combine(_env.WebRootPath, relativePath.TrimStart('/'));
            if (File.Exists(fullPath))
                File.Delete(fullPath);
        }
    }
}
