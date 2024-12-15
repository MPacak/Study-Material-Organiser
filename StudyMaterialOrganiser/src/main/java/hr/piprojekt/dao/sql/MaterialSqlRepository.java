package hr.piprojekt.dao.sql;

import hr.piprojekt.dao.dal.MaterialRepository;
import hr.piprojekt.dao.model.FileType;
import hr.piprojekt.dao.model.Material;

import javax.sql.DataSource;
import java.sql.*;
import java.util.ArrayList;
import java.util.List;
import java.util.Optional;

public class MaterialSqlRepository implements MaterialRepository {
    private static final String ID_MATERIAL = "IDMaterial";
    private static final String NAME = "Name";
    private static final String LINK = "Link";
    private static final String FILE_PATH = "FilePath";
    private static final String DESCRIPTION = "Description";
    private static final String FOLDERTYPE = "FolderTypeID";

    private static final String ID_TAG = "IDTag";
    private static final String TAG_NAME = "TagName";

    private static final String ID_MATERIALTAG = "IDMaterialTag";

    private static final String CREATE_MATERIAL = "{ CALL createMaterial (?,?,?,?,?,?) }";
    private static final String UPDATE_MATERIAL = "{ CALL updateMaterial (?,?,?,?,?,?) }";
    private static final String DELETE_MATERIAL  = "{ CALL deleteMaterial (?) }";
    private static final String SELECT_MATERIAL  = "{ CALL selectMaterial (?) }";
    private static final String SELECT_MATERIALS = "{ CALL selectMaterials }";
    public MaterialSqlRepository() {
        // Constructor logic
    }
    @Override
    public int createMaterial(Material material) throws Exception {
        DataSource dataSource = DataSourceSingleton.getInstance();
        try
            (Connection con = dataSource.getConnection();
            CallableStatement stmt = con.prepareCall(CREATE_MATERIAL)){
            stmt.registerOutParameter(ID_MATERIAL, java.sql.Types.INTEGER);
                stmt.setString(NAME, material.getName());
                stmt.setString(DESCRIPTION, material.getDescription());
                stmt.setString(LINK, material.getLink());
                stmt.setString(FILE_PATH, material.getFilepath());
                stmt.setInt(FOLDERTYPE, material.getFiletype().getId());

                stmt.executeUpdate();
                return stmt.getInt(ID_MATERIAL);
            }
        catch (SQLException e) {

            if (e.getErrorCode() == 51000) {
                throw new IllegalArgumentException("Material with the same name already exists.", e);
            }
            throw e;
        }
    }

    @Override
    public void updateMaterial(int id, Material data) throws Exception {
        DataSource dataSource = DataSourceSingleton.getInstance();
        try (Connection con = dataSource.getConnection();
             CallableStatement stmt = con.prepareCall(UPDATE_MATERIAL)) {
            stmt.setInt(ID_MATERIAL, id);
            stmt.setString(NAME, data.getName());
            stmt.setString(LINK, data.getLink());
            stmt.setString(DESCRIPTION, data.getDescription());
            stmt.setString(FILE_PATH, data.getFilepath());
            stmt.setInt(FOLDERTYPE, data.getFiletype().getId());

            stmt.executeUpdate();
        }
    }

    @Override
    public void deleteMaterial(int id) throws Exception {
        DataSource dataSource = DataSourceSingleton.getInstance();
        try (Connection con = dataSource.getConnection();
             CallableStatement stmt = con.prepareCall(DELETE_MATERIAL)) {
            stmt.setInt(ID_MATERIAL, id);

            stmt.executeUpdate();
        }
    }

    @Override
    public Optional<Material> selectMaterial(int id) throws Exception {
        DataSource dataSource = DataSourceSingleton.getInstance();
        try (Connection con = dataSource.getConnection();
             PreparedStatement stmt = con.prepareCall(SELECT_MATERIAL)) {

            stmt.setInt(1, id);
            try (ResultSet rs = stmt.executeQuery()) {

                if (rs.next()) {
                    return Optional.of(new Material (
                            rs.getInt(ID_MATERIAL),
                            rs.getString(NAME),
                            rs.getString(DESCRIPTION),
                            rs.getString(LINK),
                            rs.getString(FILE_PATH),
                            FileType.from(rs.getInt(FOLDERTYPE))));
                }
            }
        }
        return Optional.empty();
    }

    @Override
    public List<Material> selectMaterials() throws Exception {
        List<Material> materials = new ArrayList<>();
        DataSource dataSource = DataSourceSingleton.getInstance();
        try (Connection con = dataSource.getConnection();
             PreparedStatement stmt = con.prepareCall(SELECT_MATERIALS)) {

            try (ResultSet rs = stmt.executeQuery()) {

                while (rs.next()) {
                    materials.add(new Material(
                            rs.getInt(ID_MATERIAL),
                            rs.getString(NAME),
                            rs.getString(DESCRIPTION),
                            rs.getString(LINK),
                            rs.getString(FILE_PATH),
                            FileType.from(rs.getInt(FOLDERTYPE))));
                }
            }
        }
        return materials;
    }
}
