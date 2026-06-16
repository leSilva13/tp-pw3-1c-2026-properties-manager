using ProgramacionWeb3.PropertiesManager.Core.DTOs;
using ProgramacionWeb3.PropertiesManager.Core.Entities;
using ProgramacionWeb3.PropertiesManager.Core.Interfaces;
using ProgramacionWeb3.PropertiesManager.Core.Interfaces.Repositories;
using ProgramacionWeb3.PropertiesManager.Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;


namespace ProgramacionWeb3.PropertiesManager.Core.Services
{
    public class NotificacionService : INotificacionService
    {
        private readonly INotificacionRepository _notificacionRepository;
        private readonly IUnitRepository _unitRepository;
        private readonly IMasiveEmailService _emailService;

        // Inyección directa de los repositorios necesarios
        public NotificacionService(INotificacionRepository notificacionRepository, IUnitRepository unidadRepository, IMasiveEmailService emailService)
        {
            _notificacionRepository = notificacionRepository;
            _unitRepository = unidadRepository;
            _emailService = emailService;
        }

        public async Task<IEnumerable<Notificacion>> ObtenerPorConsorcioAsync(int idConsorcio)
        {
            // Asume que el repositorio tiene un método para filtrar por consorcio
            return await _notificacionRepository.GetByConsorcioIdAsync(idConsorcio);
        }

        public async Task<Notificacion?> ObtenerPorIdAsync(int id)
        {
            return await _notificacionRepository.GetByIdAsync(id);
        }

        public async Task CrearAsync(Notificacion notificacion)
        {
            notificacion.CreationDate = DateTime.Now;

            await _notificacionRepository.AddAsync(notificacion);
            await _notificacionRepository.SaveAsync();
        }

        public async Task<bool> ActualizarAsync(Notificacion notificacionModificada)
        {
            var notificacionDb = await _notificacionRepository.GetByIdAsync(notificacionModificada.IdNotificacion);

            if (notificacionDb != null && notificacionDb.SendDate == null)
            {
                notificacionDb.Title = notificacionModificada.Title;
                notificacionDb.Description = notificacionModificada.Description;

                _notificacionRepository.Update(notificacionDb);
                await _notificacionRepository.SaveAsync();

                return true;
            }
            return false;
        }

        public async Task<bool> EliminarAsync(int id)
        {
            var notificacionDb = await _notificacionRepository.GetByIdAsync(id);

            if (notificacionDb != null && notificacionDb.SendDate == null)
            {
                _notificacionRepository.Delete(notificacionDb);
                await _notificacionRepository.SaveAsync();

                return true;
            }
            return false;
        }

        public async Task<bool> EnviarNotificacionAsync(int idNotificacion)
        {
            var notificacionDb = await _notificacionRepository.GetByIdAsync(idNotificacion);

            // Regla de negocio: En caso de haberse enviado, no se puede volver a enviar
            if (notificacionDb != null && notificacionDb.SendDate == null)
            {
                // 1. Obtener todas las unidades (Units) de este consorcio (Building)
                // IMPORTANTE: Tu repositorio debe incluir (Include) la entidad Owner al hacer la consulta 
                // para que la propiedad de navegación no venga nula.
                var units = await _unitRepository.GetByBuildingIdAsync(notificacionDb.IdBuilding);

                // 2. Extraemos los mails a una lista navegando hacia el Owner
                var emails = units
                    .Where(u => u.Owner != null && !string.IsNullOrWhiteSpace(u.Owner.Email)) // Filtramos unidades sin dueño o sin mail
                    .Select(u => u.Owner.Email)
                    .Distinct() // Usamos Distinct() por si un mismo Owner tiene varias unidades en el consorcio, para no mandarle el mail 2 veces
                    .ToList();

                // 3. Llamada a tu servicio de Mailing
                // await _emailService.EnviarNotificacionMasivaAsync(emails, notificacionDb.Titulo, notificacionDb.Descripcion);

                // 4. Marcar la notificación como enviada con la fecha actual
                await _emailService.EnviarNotificacionMasivaAsync(emails, notificacionDb.Title, notificacionDb.Description);
                notificacionDb.SendDate = DateTime.Now;

                _notificacionRepository.Update(notificacionDb);
                await _notificacionRepository.SaveAsync();

                return true;
            }
            return false;
        }
    }
}