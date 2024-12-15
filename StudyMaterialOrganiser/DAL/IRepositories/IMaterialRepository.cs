using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.IRepositories
{
    public interface IMaterialRepository
    {
        Material? GetMaterialById(int materialId);
        Material? GetMaterialByName(string materialName);
        void Add(Material material);
        void Update(int id, Material data);
        void Delete(Material material);
        IEnumerable<Material> GetAll();
        bool MaterialNameExists(string name);
    }
}
