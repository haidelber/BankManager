import { Component } from "@angular/core";
import { ConfigurationService } from "./configuration.service";
import { } from "./configuration.types";

@Component({
    selector: "configuration-overview",
    templateUrl: "./configuration-overview.component.html"
})
export class ConfigurationOverviewComponent {
    model: any;
    constructor(private configurationService: ConfigurationService) {

    }

    ngOnInit() {
        this.configurationService.getApplicationConfiguration().subscribe(model => this.model = JSON.parse(model));
    }
}
