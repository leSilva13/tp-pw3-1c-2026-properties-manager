using System.IO;

namespace ProgramacionWeb3.PropertiesManager.Core.Interfaces.Services
{
    public interface IFileService
    {
        Task<string> SaveReceiptAsync(Stream content, string fileName);
        void DeleteReceipt(string? relativePath);
        string? GetReceiptPhysicalPath(string? relativePath);
    }
}
