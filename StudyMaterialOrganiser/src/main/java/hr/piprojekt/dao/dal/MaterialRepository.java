package hr.piprojekt.dao.dal;

import hr.piprojekt.dao.model.Material;

import java.util.List;
import java.util.Optional;

public interface MaterialRepository extends Repository {
    int createMaterial (Material material) throws Exception;

    void updateMaterial(int id, Material data) throws Exception;

    void deleteMaterial(int id) throws Exception;

    Optional<Material> selectMaterial(int id) throws Exception;

  //  Optional<Material> selectMaterialbyName(String name) throws Exception;

    List<Material> selectMaterials() throws Exception;
}
