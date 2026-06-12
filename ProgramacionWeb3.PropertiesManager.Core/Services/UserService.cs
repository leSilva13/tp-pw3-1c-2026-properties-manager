using ProgramacionWeb3.PropertiesManager.Core.Common.Enums;
using ProgramacionWeb3.PropertiesManager.Core.DTOs;
using ProgramacionWeb3.PropertiesManager.Core.Entities;
using ProgramacionWeb3.PropertiesManager.Core.Interfaces;
using ProgramacionWeb3.PropertiesManager.Core.Interfaces.Repositories;
using ProgramacionWeb3.PropertiesManager.Core.Interfaces.Services;

namespace ProgramacionWeb3.PropertiesManager.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        private readonly IOwnerRepository _ownerRepository;

        public UserService(IUnitOfWork unitOfWork, IUserRepository userRepository, IOwnerRepository ownerRepository)
        {
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _ownerRepository = ownerRepository;
        }

        public async Task<bool> CreateUserAsync(UserDto userDto)
        {
            try
            {
                var user = new User
                {
                    Email = userDto.Email,
                    RoleId = userDto.RoleId,
                    Password = BCrypt.Net.BCrypt.EnhancedHashPassword(Guid.NewGuid().ToString()),
                };

                await _userRepository.AddAsync(user);

                if (userDto.RoleId == (int)UserRoleEnum.Owner)
                {
                    var owner = new Owner
                    {
                        Email = userDto.Email,
                        FullName = userDto.OwnerDto.FullName,
                        User = user
                    };

                    await _ownerRepository.AddAsync(owner);
                }

                await _unitOfWork.SaveChangesAsync();

                return true;
            } catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return users.Select(u =>
                new UserDto
                {
                    Id = u.Id,
                    Email = u.Email,
                    IsActive = u.IsActive,
                    CreatedDate = u.CreationDate,
                    LastLoginDate = u.LastLoginDate,
                    RoleId = u.RoleId
                }).ToList();
        }
    }
}
