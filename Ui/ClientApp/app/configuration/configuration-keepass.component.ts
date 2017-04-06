import { Component } from "@angular/core";
import { ConfigurationService } from "./configuration.service";
import { KeePassConfiguration } from "./configuration.types";

@Component({
    selector: "configuration-keepass",
    templateUrl: "./configuration-keepass.component.html"
})
export class ConfigurationKeepassComponent {
    model: KeePassConfiguration;
    edit: boolean;

    constructor(private configurationService: ConfigurationService) {

    }

    ngOnInit() {
        this.configurationService.getKeePassConfiguration().subscribe(model => this.model = model);
    }

    toggleEdit() {
        this.edit = true;
    }

    onDiscard() {
        this.ngOnInit();
        this.edit = false;
    }

    onSubmit() {
        this.configurationService.editKeePassConfiguration(this.model).subscribe(model => this.model = model);
        this.edit = false;
    }
}
