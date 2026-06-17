using ProgramacionWeb3.PropertiesManager.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProgramacionWeb3.PropertiesManager.Core.Interfaces.Repositories
{
    public interface INotificacionRepository
    {
        Task<IEnumerable<Notificacion>> GetByConsorcioIdAsync(int idConsorcio);      
        Task<Notificacion?> GetByIdAsync(int id);
        Task AddAsync(Notificacion notificacion);
        void Update(Notificacion notificacion);
        void Delete(Notificacion notificacion);
        Task SaveAsync();
    }
}