import { NgModule } from "@angular/core";
import { RouterModule } from "@angular/router";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { CommonModule } from "@angular/common";
import { SharedModule } from "../shared/shared.module";
import { AccountModule } from "../account/account.module";

import { PortfolioGroupsService } from "./portfoliogroups.service";

import { PortfolioGroupsDetailComponent } from "./portfoliogroups-detail.component";
import { PortfolioGroupsEditComponent } from "./portfoliogroups-edit.component";


@NgModule({
    declarations: [PortfolioGroupsDetailComponent, PortfolioGroupsEditComponent],
    imports: [CommonModule, SharedModule, AccountModule, FormsModule, ReactiveFormsModule,
        RouterModule.forChild([
            { path: "portfoliogroup", component: PortfolioGroupsDetailComponent },
            { path: "portfoliogroup/:id/edit", component: PortfolioGroupsEditComponent },
            { path: "portfoliogroup/create", component: PortfolioGroupsEditComponent }
        ])],
    exports: [RouterModule],
    providers: [PortfolioGroupsService]
})
export class PortfolioGroupsModule { }
