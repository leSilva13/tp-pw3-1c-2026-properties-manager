using ProgramacionWeb3.PropertiesManager.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProgramacionWeb3.PropertiesManager.Core.Interfaces.Repositories
{
    public interface IOwnerRepository : IBaseRepository<Owner>
    {
        Task<Owner?> GetByEmailAsync(string email);
    }
}
