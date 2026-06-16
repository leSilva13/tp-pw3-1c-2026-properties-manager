using ProgramacionWeb3.PropertiesManager.Core.DTOs.Owner;

namespace ProgramacionWeb3.PropertiesManager.Core.Interfaces.Services
{
    public interface IOwnerDashboardService
    {
        Task<OwnerDashboardDto> GetDashboardDataAsync(int userId);
    }
}
