/*
 * Click nbfs://nbhost/SystemFileSystem/Templates/Licenses/license-default.txt to change this license
 * Click nbfs://nbhost/SystemFileSystem/Templates/Classes/Interface.java to edit this template
 */
package hr.piprojekt.dal;

import hr.piprojekt.dal.model.Material;
import java.util.List;
import java.util.Optional;

/**
 *
 * @author majap
 */
public interface MaterialRepository extends Repository{
    int createMaterial (Material material) throws Exception;
    
    void updateMaterial(int id, Material data) throws Exception;
    
    void deleteMaterial(int id) throws Exception;
    
    Optional<Material> selectMaterial(int id) throws Exception;
    
    List<Material> selectMaterials() throws Exception;
    
}
