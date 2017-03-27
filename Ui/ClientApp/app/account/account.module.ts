import { NgModule } from "@angular/core";
import { RouterModule } from "@angular/router"
import { UniversalModule } from "angular2-universal";

import { SharedModule } from "../shared/shared.module";

import { AccountService } from "./account.service";
import { TransactionService } from "./transaction.service";

import { AccountListComponent } from "./account-list.component";
import { AccountTitleComponent } from "./account-title.component";
import { BankAccountDetailComponent } from "./bankaccount-detail.component";
import { CreditCardDetailComponent } from "./creditcard-detail.component"
import { PortfolioDetailComponent } from "./portfolio-detail.component";
import { HistoryDiagramComponent } from "./history-diagram.component";

@NgModule({
    declarations: [AccountListComponent, AccountTitleComponent, BankAccountDetailComponent, CreditCardDetailComponent, PortfolioDetailComponent, HistoryDiagramComponent],
    imports: [UniversalModule, SharedModule,
        RouterModule.forChild([
            { path: "accounts", component: AccountListComponent },
            { path: "bankaccount/:id", component: BankAccountDetailComponent },
            { path: "creditcard/:id", component: CreditCardDetailComponent },
            { path: "portfolio/:id", component: PortfolioDetailComponent }])],
    exports: [RouterModule],
    providers: [AccountService, TransactionService]
})
export class AccountModule { }
