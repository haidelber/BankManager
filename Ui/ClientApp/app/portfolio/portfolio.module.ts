import { NgModule } from "@angular/core";
import { RouterModule } from "@angular/router";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { CommonModule } from "@angular/common";
import { SharedModule } from "../shared/shared.module";

import { PortfolioService } from "./portfolio.service";

import { PortfolioDetailComponent } from "./portfolio-detail.component"


@NgModule({
    declarations: [PortfolioDetailComponent],
    imports: [CommonModule, SharedModule, FormsModule, ReactiveFormsModule,
        RouterModule.forChild([{ path: "portfolio", component: PortfolioDetailComponent }])],
    exports: [RouterModule],
    providers: [PortfolioService]
})
export class PortfolioModule { }
