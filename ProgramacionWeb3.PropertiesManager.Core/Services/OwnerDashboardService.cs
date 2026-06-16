using ProgramacionWeb3.PropertiesManager.Core.DTOs;
using ProgramacionWeb3.PropertiesManager.Core.DTOs.Owner;
using ProgramacionWeb3.PropertiesManager.Core.Interfaces.Services;

namespace ProgramacionWeb3.PropertiesManager.Core.Services
{
    public class OwnerDashboardService : IOwnerDashboardService
    {
        private readonly IBuildingService _buildingService;
        private readonly IUnitService _unitService;
        private readonly IOwnerService _ownerService;

        public OwnerDashboardService(IBuildingService buildingService, IUnitService unitService, IOwnerService ownerService)
        {
            _buildingService = buildingService;
            _unitService = unitService;
            _ownerService = ownerService;
        }

        public async Task<OwnerDashboardDto> GetDashboardDataAsync(int userId)
        {
            var owner = await _ownerService.GetByUserId(userId);

            if (owner == null)
                return null;

            return new OwnerDashboardDto
            {
                BuildingDtos = owner.UnitDtos
                    .GroupBy(u => u.BuildingDto)
                    .Select(g => 
                        new BuildingDto
                        {
                            Id = g.Key.Id,
                            MultipurposeRoomDtos = g.Key.MultipurposeRoomDtos,
                            AddressDto = g.Key.AddressDto,
                            UnitDtos = g.Select(u => 
                                new  UnitDto
                                {
                                    ApartmentNumber = u.ApartmentNumber,
                                    Floor = u.Floor,
                                })
                        })
                    .ToList()
            };
        }
    }
}
