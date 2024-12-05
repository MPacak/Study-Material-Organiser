/*
 * Click nbfs://nbhost/SystemFileSystem/Templates/Licenses/license-default.txt to change this license
 * Click nbfs://nbhost/SystemFileSystem/Templates/Classes/Class.java to edit this template
 */
package hr.piprojekt.dal.sql;

import hr.piprojekt.dal.Repository;
import hr.piprojekt.dal.model.Material;
import hr.piprojekt.dal.model.MaterialTag;
import java.sql.CallableStatement;
import java.sql.Connection;
import java.util.List;
import java.util.Optional;
import java.util.Set;
import javax.sql.DataSource;

/**
 *
 * @author majap
 */
public class SqlRepository implements Repository{
    private static final String ID_MATERIAL = "IDMaterial";
    private static final String NAME = "Name";
     private static final String LINK = "Link";
     private static final String FILE_PATH = "IDMission";
    private static final String DESCRIPTION = "Description";
    
    private static final String FOLDERTYPE = "FolderTypeID";
    
    private static final String ID_TAG = "IDTag";
    private static final String TAG_NAME = "TagName";
    
    private static final String ID_MATERIALTAG = "IDMaterialTag";
    
     private static final String CREATE_MATERIAL = "{ CALL createMaterial (?,?,?,?,?,?,?,?,?,?,?) }";
    private static final String UPDATE_MATERIAL = "{ CALL updateMaterial (?,?,?,?,?,?,?,?,?,?,?) }";
    private static final String DELETE_MATERIAL  = "{ CALL deleteMaterial (?) }";
    private static final String SELECT_MATERIAL  = "{ CALL selectMaterial (?) }";
    private static final String SELECT_MATERIALS = "{ CALL selectMaterials }";

    @Override
    public int createMaterial(Material material) throws Exception {
         DataSource dataSource = DataSourceSingleton.getInstance();
    try (Connection con = dataSource.getConnection();
    CallableStatement stmt = con.prepareCall(CREATE_MATERIAL)) {
    
    stmt.setString(NAME, material.getName());
    stmt.setString(LINK, material.getLink());
    stmt.setString(DESCRIPTION, material.getDescription());
    stmt.setString(FILE_PATH, material.getFilepath());
    stmt.setInt(FOLDERTYPE, material.getFiletype().getId());
    
    stmt.executeUpdate();
    return stmt.getInt(ID_MATERIAL);
    }
    }

    @Override
    public void updateMaterial(int id, Material data) throws Exception {
        throw new UnsupportedOperationException("Not supported yet."); // Generated from nbfs://nbhost/SystemFileSystem/Templates/Classes/Code/GeneratedMethodBody
    }

    @Override
    public void deleteMaterial(int id) throws Exception {
        throw new UnsupportedOperationException("Not supported yet."); // Generated from nbfs://nbhost/SystemFileSystem/Templates/Classes/Code/GeneratedMethodBody
    }

    @Override
    public Optional<Material> selectMaterial(int id) throws Exception {
        throw new UnsupportedOperationException("Not supported yet."); // Generated from nbfs://nbhost/SystemFileSystem/Templates/Classes/Code/GeneratedMethodBody
    }

    @Override
    public List<Material> selectMaterials() throws Exception {
        throw new UnsupportedOperationException("Not supported yet."); // Generated from nbfs://nbhost/SystemFileSystem/Templates/Classes/Code/GeneratedMethodBody
    }

    @Override
    public int createMaterialTag(MaterialTag materialtag) throws Exception {
        throw new UnsupportedOperationException("Not supported yet."); // Generated from nbfs://nbhost/SystemFileSystem/Templates/Classes/Code/GeneratedMethodBody
    }

    @Override
    public void createMaterialTags(Set<MaterialTag> materialtags) throws Exception {
        throw new UnsupportedOperationException("Not supported yet."); // Generated from nbfs://nbhost/SystemFileSystem/Templates/Classes/Code/GeneratedMethodBody
    }

    @Override
    public void updateMaterialTag(int id, MaterialTag materialtag) throws Exception {
        throw new UnsupportedOperationException("Not supported yet."); // Generated from nbfs://nbhost/SystemFileSystem/Templates/Classes/Code/GeneratedMethodBody
    }

    @Override
    public Set<MaterialTag> selectMaterialByTag() throws Exception {
        throw new UnsupportedOperationException("Not supported yet."); // Generated from nbfs://nbhost/SystemFileSystem/Templates/Classes/Code/GeneratedMethodBody
    }

    @Override
    public Set<MaterialTag> selectTagByMaterial() throws Exception {
        throw new UnsupportedOperationException("Not supported yet."); // Generated from nbfs://nbhost/SystemFileSystem/Templates/Classes/Code/GeneratedMethodBody
    }
   
}
