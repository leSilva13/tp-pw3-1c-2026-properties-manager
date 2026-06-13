using System.IO;
using ProgramacionWeb3.PropertiesManager.Core.Interfaces.Services;

namespace ProgramacionWeb3.PropertiesManager.Infrastructure.Services
{
    public class FileService : IFileService
    {
        private readonly string _basePath;
        private const string ReceiptsFolder = "uploads/receipts";
        private const long MaxReceiptBytes = 5 * 1024 * 1024;
        private static readonly HashSet<string> _allowedExtensions = new() { ".pdf", ".jpg", ".jpeg", ".png" };

        public FileService(string basePath)
        {
            _basePath = basePath;
        }

        public async Task<string> SaveReceiptAsync(Stream content, string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                throw new InvalidOperationException("El nombre del archivo del comprobante no es válido.");

            if (content.CanSeek && content.Length > MaxReceiptBytes)
                throw new InvalidOperationException("El comprobante supera el tamaño máximo permitido (5 MB).");

            var extension = Path.GetExtension(fileName).ToLowerInvariant();
            if (!_allowedExtensions.Contains(extension))
                throw new InvalidOperationException("Tipo de archivo no permitido. Use: PDF, JPG o PNG.");

            var uploadsPath = Path.Combine(_basePath, ReceiptsFolder);
            if (!Directory.Exists(uploadsPath))
                Directory.CreateDirectory(uploadsPath);

            var nuevoNombre = $"{Guid.NewGuid()}{extension}";
            var fullPath = Path.Combine(uploadsPath, nuevoNombre);

            using var stream = new FileStream(fullPath, FileMode.Create);
            await content.CopyToAsync(stream);

            return $"/{ReceiptsFolder}/{nuevoNombre}";
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
            return Path.Combine(_basePath, relativePath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
        }
    }
}
