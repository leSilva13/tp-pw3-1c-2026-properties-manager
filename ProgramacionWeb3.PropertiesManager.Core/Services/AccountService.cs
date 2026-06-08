using ProgramacionWeb3.PropertiesManager.Core.Entities;
using ProgramacionWeb3.PropertiesManager.Core.Interfaces;
using ProgramacionWeb3.PropertiesManager.Core.Interfaces.Services;

namespace ProgramacionWeb3.PropertiesManager.Core.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _uow;

        public AccountService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _uow.Users.GetAllAsync();
        }

        public async Task<(bool Success, string? Error)> RegisterOwnerAsync(string fullName, string email, string password)
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

            var existingOwner = await _uow.Owners.GetByEmailAsync(email);
            if (existingOwner != null)
                return (false, "An owner with that email already exists.");
            
            var existingUser = await _uow.Users.GetByEmailAsync(email);
            if (existingUser != null)
                return (false, "A user with that email already exists.");

            var hashed = BCryptPasswordHasher.HashPassword(password);

            var user = new User
            {
                Email = email,
                Password = hashed,
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

            await _uow.Users.AddAsync(user);
            await _uow.Owners.AddAsync(owner);

            await _uow.SaveAsync();

            return (true, null);
        }

        public async Task<User?> LogicAsync(string email, string password)
        {
            var user = await _uow.Users.GetByEmailAsync(email);
            if (user == null)
                return null;

            var valid = BCryptPasswordHasher.Verify(password, user.Password);
            if (!valid)
                return null;

            user.LastLoginDate = DateTime.Now;
            await _uow.SaveAsync();

            return user;
        }
    }
}
