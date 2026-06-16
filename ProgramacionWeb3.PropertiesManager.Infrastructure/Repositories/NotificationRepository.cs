using Microsoft.EntityFrameworkCore;
using ProgramacionWeb3.PropertiesManager.Core.Entities;
using ProgramacionWeb3.PropertiesManager.Core.Interfaces.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProgramacionWeb3.PropertiesManager.Infrastructure.Repositories
{
    public class NotificacionRepository : INotificacionRepository
    {
        private readonly AppDbContext _context;

        public NotificacionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Notificacion>> GetByConsorcioIdAsync(int idConsorcio)
        {
        
            return await _context.Notificaciones
                                 .Where(n => n.IdBuilding == idConsorcio) 
                                 .OrderByDescending(n => n.CreationDate)
                                 .ToListAsync();
        }

        public async Task<Notificacion?> GetByIdAsync(int id)
        {
       
            return await _context.Notificaciones.FindAsync(id);
        }

        public async Task AddAsync(Notificacion notificacion)
        {
            await _context.Notificaciones.AddAsync(notificacion);
        }

        public void Update(Notificacion notificacion)
        {
         
            _context.Notificaciones.Update(notificacion);
        }

        public void Delete(Notificacion notificacion)
        {
            _context.Notificaciones.Remove(notificacion);
        }

        public async Task SaveAsync()
        {
     
            await _context.SaveChangesAsync();
        }
    }
}