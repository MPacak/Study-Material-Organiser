package hr.piprojekt.studymaterialorganiser.controllers;

import javafx.event.ActionEvent;
import javafx.fxml.FXML;
import javafx.fxml.FXMLLoader;
import javafx.scene.Node;
import javafx.scene.Scene;
import javafx.stage.Stage;

public class StudyMaterialController {
    @FXML
    private void handleMaterialsPage(ActionEvent event) {
        try {
          FXMLLoader loader = new FXMLLoader(getClass().getResource("/hr/piprojekt/studymaterialorganiser/MaterialsPage.fxml"));
            Scene scene = new Scene(loader.load());
            Stage stage = (Stage) ((Node) event.getSource()).getScene().getWindow();
            stage.setScene(scene);
            stage.setTitle("Materials Page");

        } catch (Exception e) {
            e.printStackTrace();
        }
    }
}