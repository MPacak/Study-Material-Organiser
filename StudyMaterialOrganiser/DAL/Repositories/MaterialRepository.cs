using DAL.IRepositories;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class MaterialRepository :IMaterialRepository
    {
        private readonly StudymaterialorganiserContext _context;

        public MaterialRepository(StudymaterialorganiserContext context)
        {
            _context = context;
        }

        public void Add(Material material)
        {
            _context.Add(material);
            _context.SaveChanges();
        }

        public void Delete(Material material)
        {
            _context.Remove(material);
        }

        public IEnumerable<Material> GetAll()
        {
            return _context.Materials;
        }

        public Material? GetMaterialById(int materialId)
        {
            return _context.Materials.FirstOrDefault(material => material.Idmaterial == materialId);
        }

        public Material? GetMaterialByName(string materialName)
        {
            return _context.Materials.FirstOrDefault(material => material.Name == materialName);
        }

        public void Update(int id, Material data)
        {
            var existingMaterial = _context.Materials.FirstOrDefault(material => material.Idmaterial == id);
            if (existingMaterial != null)
            {
                existingMaterial.Name = data.Name;
                existingMaterial.Description = data.Description;
                existingMaterial.Link = data.Link;
                existingMaterial.FilePath = data.FilePath;
                existingMaterial.FolderTypeId = data.FolderTypeId;

                _context.Update(existingMaterial);
            }
        }
        public bool MaterialNameExists(string name)
        {
            if(GetMaterialByName(name) != null)
            {
                return true;
            }
            return false;
        }

    }
}
