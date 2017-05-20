import { NgModule } from "@angular/core";
import { RouterModule } from "@angular/router";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { CommonModule } from "@angular/common";
import { SharedModule } from "../shared/shared.module";
import { AccountModule } from "../account/account.module";

import { PortfolioGroupsService } from "./portfoliogroups.service";

import { PortfolioGroupsDetailComponent } from "./portfoliogroups-detail.component";



@NgModule({
    declarations: [PortfolioGroupsDetailComponent],
    imports: [CommonModule, SharedModule, AccountModule, FormsModule, ReactiveFormsModule,
        RouterModule.forChild([{ path: "portfoliogroups", component: PortfolioGroupsDetailComponent }])],
    exports: [RouterModule],
    providers: [PortfolioGroupsService]
})
export class PortfolioGroupsModule { }
