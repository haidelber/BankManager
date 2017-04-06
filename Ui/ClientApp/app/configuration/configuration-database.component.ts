import { Component } from "@angular/core";
import { ConfigurationService } from "./configuration.service";
import { DatabaseConfiguration } from "./configuration.types";

@Component({
    selector: "configuration-database",
    templateUrl: "./configuration-database.component.html"
})
export class ConfigurationDatabaseComponent {
    model: DatabaseConfiguration;
    edit: boolean;
    constructor(private configurationService: ConfigurationService) {
        this.edit = false;
    }

    ngOnInit() {
        this.configurationService.getDatabaseConfiguration().subscribe(model => this.model = model);
    }

    toggleEdit() {
        this.edit = true;
    }

    onDiscard() {
        this.ngOnInit();
        this.edit = false;
    }

    onSubmit() {
        this.configurationService.editDatabaseConfiguration(this.model).subscribe(model => this.model = model);
        this.edit = false;
    }
}
