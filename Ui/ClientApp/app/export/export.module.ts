import { NgModule } from "@angular/core";
import { RouterModule } from "@angular/router"
import { UniversalModule } from "angular2-universal";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { FileUploadModule } from "ng2-file-upload";
import { AccountModule } from "../account/account.module";
import { SharedModule } from "../shared/shared.module";

import { ExportComponent } from "./export.component";

@NgModule({
    declarations: [ExportComponent],
    imports: [UniversalModule, SharedModule, RouterModule.forChild([{ path: "export", component: ExportComponent }])],
    exports: [RouterModule],
    providers: []
})
export class ExportModule { }
