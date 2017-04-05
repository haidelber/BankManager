import { Component } from "@angular/core";
import { ConfigurationService } from "./configuration.service";
import { ApplicationConfiguration } from "./configuration.model";

@Component({
    selector: "configuration",
    templateUrl: "./configuration.component.html"
})
export class ConfigurationComponent {
    public configuration: any;
    public configurationFilePath: string;
    public errorMessage: string;

    constructor(private configurationService: ConfigurationService) {
        configurationService.getConfiguration().subscribe(conf => this.configuration = conf);
        configurationService.getConfigurationFilePath().subscribe(conf => this.configurationFilePath = conf);
    }

    generateConfigFile() {
        this.configurationService.generateConfigFile().subscribe(success => success);
    }
}
