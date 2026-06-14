using ProgramacionWeb3.PropertiesManager.Core.Entities;
using ProgramacionWeb3.PropertiesManager.Core.Interfaces;
using ProgramacionWeb3.PropertiesManager.Core.Interfaces.Repositories;
using ProgramacionWeb3.PropertiesManager.Core.Interfaces.Services;

namespace ProgramacionWeb3.PropertiesManager.Core.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _uow;
        private readonly IUserRepository _userRepository;
        private readonly IOwnerRepository _ownerRepository;

        public AccountService(IUnitOfWork uow, IUserRepository userRepository, IOwnerRepository ownerRepository)
        {
            _uow = uow;
            _userRepository = userRepository;
            _ownerRepository = ownerRepository;
        }

        public async Task<(bool Success, string? Error)> RegisterAsync(string fullName, string email, string password)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                if (addr.Address != email)
                    return (false, "Invalid email format.");
            }
            catch
            {
                return (false, "Invalid email format.");
            }

            var existingOwner = await _ownerRepository.GetByEmailAsync(email);
            if (existingOwner != null)
                return (false, "An owner with that email already exists.");
            
            var existingUser = await _userRepository.GetByEmailAsync(email);
            if (existingUser != null)
                return (false, "A user with that email already exists.");

            var user = new User
            {
                Email = email,
                Password = BCrypt.Net.BCrypt.EnhancedHashPassword(password),
                IsActive = true,
                CreationDate = DateTime.Now,
                LastLoginDate = null,
                RoleId = 2
            };
            
            var owner = new Owner
            {
                FullName = fullName,
                Email = email,
                CreationDate = DateTime.Now,
                User = user
            };

            await _userRepository.AddAsync(user);
            await _ownerRepository.AddAsync(owner);

            await _uow.SaveChangesAsync();

            return (true, null);
        }

        public async Task<User?> LogicAsync(string email, string password)
        {
            var user = await _userRepository.GetActiveUserByEmailAsync(email);
            if (user == null)
                return null;

            if (!BCrypt.Net.BCrypt.EnhancedVerify(password, user.Password))
                return null;

            user.LastLoginDate = DateTime.Now;
            await _uow.SaveChangesAsync();

            return user;
        }
    }
}
