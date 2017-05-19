import { NgModule } from "@angular/core";
import { RouterModule } from "@angular/router"
import { CommonModule } from "@angular/common";

import { AboutComponent } from "./about.component";

@NgModule({
    declarations: [AboutComponent],
    imports: [CommonModule, RouterModule.forChild([
        { path: "about", component: AboutComponent }
    ])],
    exports: [RouterModule]
})
export class AboutModule { }
