using Microsoft.EntityFrameworkCore;
using ProgramacionWeb3.PropertiesManager.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProgramacionWeb3.PropertiesManager.Core.Interfaces.Repositories
{
    internal class BuildingRepository : IBuildingRepository
    {
        private readonly AppDbcontext _context;

        public BuildingRepository(MyDbContext context)
        {
            _context = context;
        }

        public List<Building> GetAllWithRelationships(int userId)
        {
            // Tarea 2.2: Se incluye de forma anidada la Dirección, la Localidad y la Provincia
            // Requisito funcional pág 10: "Por defecto debe estar ordenado por nombre ascendentemente"
            return _context.Buildings
                .Include(b => b.Address)
                    .ThenInclude(a => a.Locality)
                        .ThenInclude(l => l.Province)
                .Where(b => b.UserId == userId)
                .OrderBy(b => b.Name)
                .ToList();
        }

        public Building GetByIdWithRelationships(int id)
        {
            return _context.Buildings
                .Include(b => b.Address)
                    .ThenInclude(a => a.Locality)
                        .ThenInclude(l => l.Province)
                .FirstOrDefault(b => b.Id == id);
        }

        public void Insert(Building building)
        {
            _context.Buildings.Add(building);
        }

        public void Update(Building building)
        {
            _context.Buildings.Update(building);
        }

        public void Delete(int id)
        {
            var building = _context.Buildings.FirstOrDefault(b => b.Id == id);
            if (building != null)
            {
                _context.Buildings.Remove(building);
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
