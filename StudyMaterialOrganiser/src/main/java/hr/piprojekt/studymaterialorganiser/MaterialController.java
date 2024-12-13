package hr.piprojekt.studymaterialorganiser;

import hr.piprojekt.dao.model.Material;
import hr.piprojekt.dao.model.FileType;
import hr.piprojekt.dao.service.MaterialService;
import javafx.collections.FXCollections;
import javafx.event.ActionEvent;
import javafx.fxml.FXML;
import javafx.scene.control.*;
import javafx.scene.Node;
import javafx.scene.Scene;
import javafx.stage.Stage;
import javafx.fxml.FXMLLoader;
import javafx.scene.Parent;

import java.io.IOException;
import java.util.logging.Level;
import java.util.logging.Logger;

public class MaterialController {

    @FXML
    private TextField nameField;

    @FXML
    private TextArea descriptionField;

    @FXML
    private TextField linkField;

    @FXML
    private TextField filePathField;

    @FXML
    private ComboBox<FileType> fileTypeComboBox;

    @FXML
    private Label nameErrorLabel;

    private final MaterialService materialService = new MaterialService();

    public void initialize() {

        fileTypeComboBox.setItems(FXCollections.observableArrayList(FileType.values()));
        nameErrorLabel.setText("");
    }

    @FXML
    private void handleSaveMaterial(ActionEvent event) {
        try {
            nameErrorLabel.setText("");

            Material material = new Material(
                    nameField.getText(),
                    descriptionField.getText(),
                    linkField.getText(),
                    filePathField.getText(),
                    fileTypeComboBox.getValue()
            );

            if (material.getName().isEmpty() || material.getFiletype() == null) {
                showAlert(Alert.AlertType.ERROR, "Validation Error", "Please fill in all required fields.");
                return;
            }

            materialService.createMaterial(material);

            showAlert(Alert.AlertType.INFORMATION, "Success", "Material updated successfully!");
            clearForm();
        } catch (IllegalArgumentException ex) {
            if (ex.getMessage().contains("already exists")) {
                nameErrorLabel.setText("This name already exists. Please choose another.");
                nameField.requestFocus();
            } else {
                Logger.getLogger(MaterialController.class.getName()).log(Level.SEVERE, null, ex);
                showAlert(Alert.AlertType.ERROR, "Error", "Unable to update material.");
            }
        } catch (Exception ex) {
            Logger.getLogger(MaterialController.class.getName()).log(Level.SEVERE, null, ex);
            showAlert(Alert.AlertType.ERROR, "Error", "Unable to update material.");
        }
    }

    @FXML
    private void handleHomePage(ActionEvent event) {
        try {
            FXMLLoader loader = new FXMLLoader(getClass().getResource("/hr/piprojekt/studymaterialorganiser/HomePage.fxml"));
            Parent homePageRoot = loader.load();

            Stage stage = (Stage) ((Node) event.getSource()).getScene().getWindow();
            Scene scene = new Scene(homePageRoot);
            stage.setScene(scene);
            stage.show();
        } catch (IOException e) {
            Logger.getLogger(MaterialController.class.getName()).log(Level.SEVERE, null, e);
            showAlert(Alert.AlertType.ERROR, "Navigation Error", "Unable to navigate to the home page.");
        }
    }

    private void clearForm() {
        nameField.clear();
        descriptionField.clear();
        linkField.clear();
        filePathField.clear();
        fileTypeComboBox.getSelectionModel().clearSelection();
        nameErrorLabel.setText("");
    }

    private void showAlert(Alert.AlertType alertType, String title, String message) {
        Alert alert = new Alert(alertType);
        alert.setTitle(title);
        alert.setHeaderText(null);
        alert.setContentText(message);
        alert.showAndWait();
    }
}
