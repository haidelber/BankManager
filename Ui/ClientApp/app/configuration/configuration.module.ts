import { NgModule } from "@angular/core";
import { RouterModule } from "@angular/router"
import { UniversalModule } from "angular2-universal";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";

import { ConfigurationComponent } from "./configuration.component";

@NgModule({
    declarations: [ConfigurationComponent],
    imports: [UniversalModule, FormsModule, ReactiveFormsModule, RouterModule.forChild([{ path: "configuration", component: ConfigurationComponent }])],
    exports: [RouterModule]
})
export class ConfigurationModule { }
