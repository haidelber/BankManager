import { NgModule } from "@angular/core";
import { RouterModule } from "@angular/router";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { CommonModule } from "@angular/common";

import { DownloadComponent } from "./download.component";
import { DownloadService } from "./download.service";

@NgModule({
    declarations: [DownloadComponent],
    imports: [CommonModule, FormsModule, ReactiveFormsModule, RouterModule.forChild([{ path: "download", component: DownloadComponent }])],
    exports: [RouterModule],
    providers: [DownloadService]
})
export class DownloadModule { }
