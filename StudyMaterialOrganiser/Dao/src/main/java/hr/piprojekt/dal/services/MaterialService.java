/*
 * Click nbfs://nbhost/SystemFileSystem/Templates/Licenses/license-default.txt to change this license
 * Click nbfs://nbhost/SystemFileSystem/Templates/Classes/Class.java to edit this template
 */
package hr.piprojekt.dal.services;

import hr.piprojekt.dal.MaterialRepository;
import hr.piprojekt.dal.RepositoryFactory;
import hr.piprojekt.dal.model.Material;
import java.util.List;
import java.util.Optional;

/**
 *
 * @author majap
 */
public class MaterialService {
    MaterialRepository materialRepository = RepositoryFactory.getRepository(MaterialRepository.class);

    public MaterialService() {
    }
    
    public int createMaterial (Material material) throws Exception { 
        int id = materialRepository.createMaterial(material);
        return id;
    }
       public void updateMaterial (int id, Material material) throws Exception { 
        materialRepository.updateMaterial(id, material);
    }
           public void deleteMaterial (int id) throws Exception { 
        materialRepository.deleteMaterial(id);
    }
          public Optional<Material> selectMaterial(int id) throws Exception {
             Optional<Material> m;
             m = materialRepository.selectMaterial(id);
             return m;
          }
    
    public List<Material> selectMaterials() throws Exception {
        List<Material> materials; 
        materials = materialRepository.selectMaterials();
        return materials;
    }
    }
    
