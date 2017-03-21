import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router'
import { UniversalModule } from 'angular2-universal';

import { AboutComponent } from './about.component';

@NgModule({
    declarations: [AboutComponent],
    imports: [UniversalModule,RouterModule.forChild([
        { path: 'about', component: AboutComponent }
    ])],
    exports: [RouterModule]
})
export class AboutModule { }
