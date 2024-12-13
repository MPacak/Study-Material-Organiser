package hr.piprojekt.dao.dal;

import java.util.HashMap;
import java.util.Map;
import java.util.logging.Level;
import java.util.logging.Logger;

public class RepositoryFactory {
    private static final Map<Class<? extends Repository>, Repository> repositories = new HashMap<>();

    @SuppressWarnings("unchecked")
    public static <T extends Repository> T getRepository(Class<T> repositoryType) {
        return (T) repositories.computeIfAbsent(repositoryType, key -> {
            try {

                String implClassName = "hr.piprojekt.dao.sql." + repositoryType.getSimpleName().replace("Repository", "SqlRepository");
                return (T) Class.forName(implClassName).getDeclaredConstructor().newInstance();
            } catch (Exception ex) {
                Logger.getLogger(RepositoryFactory.class.getName()).log(Level.SEVERE, null, ex);
                throw new RuntimeException("Failed to create repository instance for: " + repositoryType.getSimpleName(), ex);
            }
        });
    }
}
