using Microsoft.AspNetCore.Http;

namespace ProgramacionWeb3.PropertiesManager.Web.Services
{
    public interface IFileService
    {
        Task<string> SaveReceiptAsync(IFormFile file);
        void DeleteReceipt(string? relativePath);
    }
}
