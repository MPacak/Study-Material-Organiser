/*
 * Click nbfs://nbhost/SystemFileSystem/Templates/Licenses/license-default.txt to change this license
 * Click nbfs://nbhost/SystemFileSystem/Templates/Classes/Class.java to edit this template
 */
package hr.piprojekt.dal;

import java.io.InputStream;
import java.util.HashMap;
import java.util.Map;
import java.util.Properties;
import java.util.logging.Level;
import java.util.logging.Logger;

/**
 *
 * @author majap
 */
public class RepositoryFactory {
     private static final Properties properties = new Properties();
    private static final String PATH = "/config/repository.properties";
    private static final Map<Class<? extends Repository>, Repository> repositories = new HashMap<>();

    static {
        try (InputStream is = RepositoryFactory.class.getResourceAsStream(PATH)) {
            properties.load(is);
        } catch (Exception ex) {
            Logger.getLogger(RepositoryFactory.class.getName()).log(Level.SEVERE, null, ex);
        }
    }

    @SuppressWarnings("unchecked")
    public static <T extends Repository> T getRepository(Class<T> repositoryType) {
        return (T) repositories.computeIfAbsent(repositoryType, key -> {
            String className = properties.getProperty(repositoryType.getSimpleName());
            try {
                return (T) Class.forName(className).getDeclaredConstructor().newInstance();
            } catch (Exception ex) {
                Logger.getLogger(RepositoryFactory.class.getName()).log(Level.SEVERE, null, ex);
                throw new RuntimeException("Failed to create repository instance for: " + className, ex);
            }
        });
    }
    
}
