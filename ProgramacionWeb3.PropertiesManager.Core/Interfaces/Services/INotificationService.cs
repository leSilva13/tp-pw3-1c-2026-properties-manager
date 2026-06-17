using ProgramacionWeb3.PropertiesManager.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProgramacionWeb3.PropertiesManager.Core.Interfaces.Services
{
    public interface INotificacionService
    {
        // Obtiene todas las notificaciones de un consorcio específico
        Task<IEnumerable<Notificacion>> ObtenerPorConsorcioAsync(int idBuilding);

        // Obtiene una notificación por su ID principal
        Task<Notificacion?> ObtenerPorIdAsync(int id);

        // Crea una nueva notificación en el sistema
        Task CrearAsync(Notificacion notification);

        // Actualiza una notificación existente. Retorna true si fue exitoso (ej. si no estaba enviada)
        Task<bool> ActualizarAsync(Notificacion modNotification);

        // Elimina una notificación por su ID. Retorna true si fue exitoso
        Task<bool> EliminarAsync(int id);

        // Dispara la lógica de envío de correos y marca la notificación como enviada
        Task<bool> EnviarNotificacionAsync(int idNotification);
    }
}