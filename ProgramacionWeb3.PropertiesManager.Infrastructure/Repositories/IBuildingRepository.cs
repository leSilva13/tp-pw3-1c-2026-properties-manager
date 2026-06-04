using ProgramacionWeb3.PropertiesManager.Core.Entities;

namespace ProgramacionWeb3.PropertiesManager.Core.Interfaces.Repositories
{
    internal interface IBuildingRepository : IBaseRepository<Building>
    {
        List<Building> GetAllWithRelationships(int userId); // Tarea 2.2 (Trae con Localidad y Provincia)
        Building GetByIdWithRelationships(int id);
        void Insert(Building building);
        void Update(Building building);
        void Delete(int id);
        void Save();
    }
}