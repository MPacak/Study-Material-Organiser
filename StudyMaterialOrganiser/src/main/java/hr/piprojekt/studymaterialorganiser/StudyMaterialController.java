package hr.piprojekt.studymaterialorganiser;

import javafx.fxml.FXML;
import javafx.scene.control.Label;

public class StudyMaterialController {
    @FXML
    private Label welcomeText;

    @FXML
    protected void onHelloButtonClick() {
        welcomeText.setText("Welcome to JavaFX Application!");
    }
}