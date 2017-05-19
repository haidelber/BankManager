import { NgModule } from "@angular/core";
import { RouterModule } from "@angular/router";
import { CommonModule } from "@angular/common";

import { SharedModule } from "../shared/shared.module";

import { ExportComponent } from "./export.component";

@NgModule({
    declarations: [ExportComponent],
    imports: [CommonModule, SharedModule, RouterModule.forChild([{ path: "export", component: ExportComponent }])],
    exports: [RouterModule],
    providers: []
})
export class ExportModule { }
