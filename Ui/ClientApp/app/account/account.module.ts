import { NgModule } from '@angular/core';
import { RouterModule } from "@angular/router"
import { UniversalModule } from "angular2-universal";

import { SharedModule } from "../shared/shared.module";

import { AccountService } from "./account.service";

import { AccountListComponent } from "./account-list.component";
import { AccountDetailComponent } from "./account-detail.component";
import { PortfolioDetailComponent } from "./portfolio-detail.component";
import { HistoryDiagramComponent } from "./history-diagram.component";

@NgModule({
    declarations: [AccountListComponent, AccountDetailComponent, PortfolioDetailComponent, HistoryDiagramComponent],
    imports: [UniversalModule, SharedModule, RouterModule.forChild([{ path: "accounts", component: AccountListComponent }])],
    exports: [RouterModule],
    providers: [AccountService]
})
export class AccountModule { }
