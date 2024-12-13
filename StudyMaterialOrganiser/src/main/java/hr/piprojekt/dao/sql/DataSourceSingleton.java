package hr.piprojekt.dao.sql;

import com.microsoft.sqlserver.jdbc.SQLServerDataSource;

import javax.sql.DataSource;
import java.io.IOException;
import java.io.InputStream;
import java.util.Properties;
import java.util.logging.Level;
import java.util.logging.Logger;

public final class DataSourceSingleton {
    private static final String PATH = "/config/db.properties";

    private static final String SERVER_NAME = "SERVER_NAME";
    private static final String PORT = "PORT";
    private static final String DATABASE_NAME = "DATABASE_NAME";
    private static final String USER = "USER";
    private static final String PASSWORD = "PASSWORD";
    private static final String ENCRYPT = "ENCRYPT"; // Optional property for encryption

    private static final Properties properties = new Properties();

    static {
        try (InputStream is = DataSourceSingleton.class.getResourceAsStream(PATH)) {
            if (is == null) {
                throw new RuntimeException("Failed to load configuration file: " + PATH);
            }
            properties.load(is);
        } catch (IOException ex) {
            Logger.getLogger(DataSourceSingleton.class.getName()).log(Level.SEVERE, "Error loading configuration file", ex);
            throw new RuntimeException("Failed to load database configuration", ex);
        }
    }

    private DataSourceSingleton() {
    }

    private static DataSource instance;

    public static DataSource getInstance() {
        if (instance == null) {
            instance = createInstance();
        }
        return instance;
    }

    private static DataSource createInstance() {
        SQLServerDataSource dataSource = new SQLServerDataSource();

        dataSource.setServerName(properties.getProperty(SERVER_NAME));
        if (properties.containsKey(PORT)) {
            dataSource.setPortNumber(Integer.parseInt(properties.getProperty(PORT)));
        }
        dataSource.setDatabaseName(properties.getProperty(DATABASE_NAME));
        dataSource.setUser(properties.getProperty(USER));
        dataSource.setPassword(properties.getProperty(PASSWORD));

        // Disable encryption based on property or by default
        String encrypt = properties.getProperty(ENCRYPT, "false"); // Default to "false" if not specified
        dataSource.setEncrypt("true".equalsIgnoreCase(encrypt) ? "true" : "false");

        return dataSource;
    }
}
