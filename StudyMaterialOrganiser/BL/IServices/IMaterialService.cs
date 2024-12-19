using BL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.IServices
{
    public interface IMaterialService
    {
        MaterialDto? GetMaterialById(int materialId);
        MaterialDto? GetMaterialByName(string materialName);
        void Create(MaterialDto material);
        void Update(int id, MaterialDto data);
        void Delete(MaterialDto material);
        IEnumerable<MaterialDto> GetAll();
       
    }
}
