import { NgModule } from "@angular/core";
import { RouterModule } from "@angular/router"
import { UniversalModule } from "angular2-universal";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { FileUploadModule } from "ng2-file-upload";
import { AccountModule } from "../account/account.module";
import { SharedModule } from "../shared/shared.module";

import { ImportComponent } from "./import.component";
import { ImportService } from "./import.service";

@NgModule({
    declarations: [ImportComponent],
    imports: [UniversalModule, FormsModule, ReactiveFormsModule, SharedModule, AccountModule, FileUploadModule, RouterModule.forChild([{ path: "import", component: ImportComponent }])],
    exports: [RouterModule],
    providers: [ImportService]
})
export class ImportModule { }
