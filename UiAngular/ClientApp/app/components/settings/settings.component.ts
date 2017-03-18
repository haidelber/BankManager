import { Component } from "@angular/core";
import { Http } from "@angular/http";

@Component({
    selector: "settings",
    template: require("./settings.component.html")
})
export class SettingsComponent {
    public configuration: ApplicationConfiguration;
    public configurationString: string;
    public configurationFilePath: string;
    public errorMessage: string;

    constructor(http: Http) {
        http.get("/api/Configuration").subscribe(result => {
            this.configuration = result.json();
            this.configurationString = JSON.stringify(this.configuration);
        },
            error => this.errorMessage = error);
        http.get("/api/Configuration/ConfigurationFilePath").subscribe(result =>
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
