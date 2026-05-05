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
    }
}
