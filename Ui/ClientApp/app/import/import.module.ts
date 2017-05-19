import { NgModule } from "@angular/core";
import { RouterModule } from "@angular/router";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { AccountModule } from "../account/account.module";
import { CommonModule } from "@angular/common";

import { SharedModule } from "../shared/shared.module";

import { ImportComponent } from "./import.component";
import { ImportService } from "./import.service";

@NgModule({
    declarations: [ImportComponent],
    imports: [CommonModule, FormsModule, ReactiveFormsModule, SharedModule, AccountModule, RouterModule.forChild([{ path: "import", component: ImportComponent }])],
    exports: [RouterModule],
    providers: [ImportService]
})
export class ImportModule { }
