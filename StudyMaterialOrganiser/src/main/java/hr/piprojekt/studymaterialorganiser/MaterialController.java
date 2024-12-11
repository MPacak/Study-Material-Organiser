package hr.piprojekt.studymaterialorganiser;

import javafx.event.ActionEvent;
import javafx.fxml.FXML;
import javafx.fxml.FXMLLoader;
import javafx.scene.Node;
import javafx.scene.Scene;
import javafx.scene.control.ListView;
import javafx.scene.control.TextField;
import javafx.stage.Stage;

public class MaterialController {
    @FXML
    private TextField materialInput;

    @FXML
    private ListView<String> materialsList;

    @FXML
    private void handleAddMaterial() {
        String material = materialInput.getText();
        if (material != null && !material.isEmpty()) {
            materialsList.getItems().add(material);
            materialInput.clear();
        }
    }

    @FXML
    private void handleHomePage(ActionEvent event) {
        try {
            FXMLLoader loader = new FXMLLoader(getClass().getResource("HomePage.fxml"));
            Scene scene = new Scene(loader.load());
            Stage stage = (Stage) ((Node) event.getSource()).getScene().getWindow();
            stage.setScene(scene);
            stage.setTitle("Home Page");
        } catch (Exception e) {
            e.printStackTrace();
        }
    }
}
