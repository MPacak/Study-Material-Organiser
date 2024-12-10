package hr.piprojekt.dao.service;

import hr.piprojekt.dao.dal.MaterialRepository;
import hr.piprojekt.dao.dal.RepositoryFactory;
import hr.piprojekt.dao.model.Material;

import java.util.List;
import java.util.Optional;

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
