package hr.piprojekt.studymaterialorganiser;

import javafx.application.Application;
import javafx.fxml.FXMLLoader;
import javafx.scene.Scene;
import javafx.stage.Stage;

import java.io.IOException;

public class StudyMaterialOrganiser extends Application {

    @Override
    public void start(Stage stage) throws IOException {
        FXMLLoader fxmlLoader = new FXMLLoader(StudyMaterialOrganiser.class.getResource("HomePage.fxml"));
        Scene scene = new Scene(fxmlLoader.load(), 500, 400);
        stage.setTitle("Study Material Organiser");
        stage.setScene(scene);
        stage.show();
    }

    public static void main(String[] args) {
        launch();
    }
}
