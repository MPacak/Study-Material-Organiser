module hr.piprojekt.studymaterialorganiser {
    requires javafx.controls;
    requires javafx.fxml;

    requires com.dlsc.formsfx;
    requires org.kordamp.bootstrapfx.core;
    requires java.sql;
    requires com.microsoft.sqlserver.jdbc;
    requires java.naming;

    opens hr.piprojekt.studymaterialorganiser to javafx.fxml;
    exports hr.piprojekt.studymaterialorganiser;
}