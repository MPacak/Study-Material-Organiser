﻿using BL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.IServices
{
    public interface IMaterialTagService
    {
        void Create(MaterialTagDto materialTagDto);
        void DeleteByMaterialId(int materialId);
        IEnumerable<MaterialTagDto> GetByMaterialId(int materialId);
        void Update(int materialId, List<int> TagIds);
    }
}