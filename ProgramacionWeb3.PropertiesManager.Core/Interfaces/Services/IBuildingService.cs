using ProgramacionWeb3.PropertiesManager.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProgramacionWeb3.PropertiesManager.Core.Interfaces.Services
{
    public interface IBuildingService
    {
        Task<IEnumerable<Building>> ObtenerEdificiosPorUsuarioAsync(int userId);
        Task<Building?> ObtenerEdificioPorIdAsync(int id);
        Task CrearEdificioAsync(Building building);
        Task ActualizarEdificioAsync(Building building);
        Task<bool> BajaDeEdificioAsync(int id);

       


    }
}
