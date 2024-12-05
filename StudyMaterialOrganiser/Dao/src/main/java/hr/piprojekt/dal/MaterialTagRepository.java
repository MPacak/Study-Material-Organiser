/*
 * Click nbfs://nbhost/SystemFileSystem/Templates/Licenses/license-default.txt to change this license
 * Click nbfs://nbhost/SystemFileSystem/Templates/Classes/Interface.java to edit this template
 */
package hr.piprojekt.dal;

import hr.piprojekt.dal.model.MaterialTag;
import java.util.Set;

/**
 *
 * @author majap
 */
public interface MaterialTagRepository extends Repository {
    int createMaterialTag(MaterialTag materialtag) throws Exception;

    void createMaterialTags(Set<MaterialTag> materialtags) throws Exception;
    
    void updateMaterialTag(int id,MaterialTag materialtag) throws Exception;
   
    Set<MaterialTag> selectMaterialByTag() throws Exception;
    Set<MaterialTag>  selectTagByMaterial() throws Exception;
}
