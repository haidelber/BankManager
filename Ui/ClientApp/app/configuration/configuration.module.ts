import { NgModule } from "@angular/core";
import { RouterModule } from "@angular/router"
import { UniversalModule } from "angular2-universal";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { SharedModule } from "../shared/shared.module";

import { ConfigurationNavComponent } from "./configuration-nav.component";
import { ConfigurationDatabaseComponent } from "./configuration-database.component";
import { ConfigurationDownloadComponent } from "./configuration-download.component";
import { ConfigurationKeepassComponent } from "./configuration-keepass.component";
import { ConfigurationOverviewComponent } from "./configuration-overview.component";
import { ConfigurationService } from "./configuration.service";

@NgModule({
    declarations: [ConfigurationNavComponent, ConfigurationDatabaseComponent, ConfigurationDownloadComponent, ConfigurationKeepassComponent, ConfigurationOverviewComponent],
    imports: [UniversalModule, FormsModule, ReactiveFormsModule, SharedModule, RouterModule.forChild([
        { path: "configuration", redirectTo: "configuration/overview" },
        { path: "configuration/overview", component: ConfigurationOverviewComponent },
        { path: "configuration/database", component: ConfigurationDatabaseComponent },
        { path: "configuration/download", component: ConfigurationDownloadComponent },
        { path: "configuration/keepass", component: ConfigurationKeepassComponent }
    ])],
    exports: [RouterModule],
    providers: [ConfigurationService]
})
export class ConfigurationModule { }
