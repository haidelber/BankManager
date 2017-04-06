import { Component } from "@angular/core";
import { ConfigurationService } from "./configuration.service";
import { DownloadHandlerConfiguration } from "./configuration.types";

@Component({
    selector: "configuration-download",
    templateUrl: "./configuration-download.component.html"
})
export class ConfigurationDownloadComponent {
    model: DownloadHandlerConfiguration[];
    selected: DownloadHandlerConfiguration;

    constructor(private configurationService: ConfigurationService) {
        this.selected = undefined;
    }

    ngOnInit() {
        this.configurationService.getDownloadHandlerConfigurations().subscribe(model => {
            this.model = model;
            this.model = [...this.model];
        });
    }

    edit(model: DownloadHandlerConfiguration) {
        this.selected = model;
    }

    onDiscard() {
        this.selected = undefined;
        this.ngOnInit();
    }

    onSubmit() {
        this.configurationService.editDownloadHandlerConfiguration(this.selected).subscribe();
        this.selected = undefined;
        this.ngOnInit();
    }
}
