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
        private readonly IEmailService _emailService;
        private readonly IUserRepository _userRepository;
        private readonly IOwnerRepository _ownerRepository;

        public UserService(IUnitOfWork unitOfWork, IEmailService emailService, IUserRepository userRepository, IOwnerRepository ownerRepository)
        {
            _unitOfWork = unitOfWork;
            _emailService = emailService;
            _userRepository = userRepository;
            _ownerRepository = ownerRepository;
        }

        public async Task<BaseResponseDto> CreateUserAsync(UserDto userDto)
        {
            try
            {
                if (await _userRepository.GetByEmailAsync(userDto.Email) != null)
                    return new BaseResponseDto() { IsSuccess = false, Message = "Ya existe un usuario con el email ingresado." };

                string password = Guid.NewGuid().ToString();

                var user = new User
                {
                    Email = userDto.Email,
                    RoleId = userDto.RoleId,
                    Password = BCrypt.Net.BCrypt.EnhancedHashPassword(password),
                };

                await _userRepository.AddAsync(user);

                if (userDto.RoleId == (int)UserRoleEnum.Owner)
                {
                    var owner = await _ownerRepository.GetByEmailAsync(userDto.Email);
                    if (owner != null)
                    {
                        owner.User = user;
                        _ownerRepository.Update(owner);
                    }
                    else
                    {
                        owner = new Owner
                        {
                            Email = userDto.Email,
                            FullName = userDto.OwnerDto?.FullName ?? string.Empty,
                            User = user
                        };
                        await _ownerRepository.AddAsync(owner);
                    }
                }

                await _unitOfWork.SaveChangesAsync();

                var emailBody = $"Welcome! Your temporary password is: {password}. Please change it upon logging in.";
                await _emailService.SendEmailAsync(user.Email, "Welcome to PropertiesManager", emailBody);

                return new BaseResponseDto() { IsSuccess = true, Message = "Usuario creado." };
            }
            catch (Exception ex)
            {
                return new BaseResponseDto() { IsSuccess = false, Message = "Ocurrió un error. Vuelva a intentarlo." };
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
