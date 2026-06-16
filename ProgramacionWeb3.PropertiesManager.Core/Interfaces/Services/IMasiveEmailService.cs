using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProgramacionWeb3.PropertiesManager.Core.Interfaces.Services
{
    public interface IMasiveEmailService
    {
        Task EnviarNotificacionMasivaAsync(List<string> destinatarios, string asunto, string cuerpo);
    }
}