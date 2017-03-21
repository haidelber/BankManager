import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router'
import { UniversalModule } from 'angular2-universal';
import { FormsModule, ReactiveFormsModule } from "@angular/forms";

import { DownloadComponent } from './download.component';

@NgModule({
    declarations: [DownloadComponent],
    imports: [UniversalModule, FormsModule, ReactiveFormsModule, RouterModule.forChild([{ path: 'download', component: DownloadComponent }])],
    exports: [RouterModule]
})
export class DownloadModule { }
