import { Component } from "@angular/core";
import { Http } from "@angular/http";

@Component({
    selector: "configuration",
    templateUrl: "./configuration.component.html"
})
export class ConfigurationComponent {
    public configuration: ApplicationConfiguration;
    public configurationFilePath: string;
    public errorMessage: string;

    constructor(private http: Http) {
        http.get("/api/Configuration").subscribe(result =>
            this.configuration = result.json(),
            error => this.errorMessage = error);
        http.get("/api/Configuration/ConfigurationFilePath").subscribe(result =>
            this.configurationFilePath = result.text(),
            error => this.errorMessage = error);
    }
    generateConfigFile() {
        this.http.post("/api/Configuration/GenerateConfigFile", {}).subscribe(result =>
            this.configurationFilePath = result.text(),
            error => this.errorMessage = error);
    }
}

class ApplicationConfiguration {
    keePassConfiguration: KeePassConfiguration;
    databaseConfiguration: DatabaseConfiguration;
    uiConfiguration: UiConfiguration;
}
class KeePassConfiguration {
    path: string;
}
class DatabaseConfiguration {
    databasePath: string;
}
class UiConfiguration {
    language: string;
}
