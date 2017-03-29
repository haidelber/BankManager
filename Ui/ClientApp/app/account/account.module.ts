import { NgModule } from "@angular/core";
import { RouterModule } from "@angular/router"
import { UniversalModule } from "angular2-universal";
import { NgxChartsModule } from "@swimlane/ngx-charts";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";

import { SharedModule } from "../shared/shared.module";

import { AccountService } from "./account.service";
import { TransactionService } from "./transaction.service";

import { AccountListComponent } from "./account-list.component";
import { AccountTitleComponent } from "./account-title.component";
import { BankAccountDetailComponent } from "./bankaccount-detail.component";
import { CreditCardDetailComponent } from "./creditcard-detail.component"
import { PortfolioDetailComponent } from "./portfolio-detail.component";
import { HistoryDiagramComponent } from "./history-diagram.component";
import { CumulatedTransactionComponent } from "./cumulated-transaction.component";
import { CumulatedPositionComponent } from "./cumulated-position.component";
import { EditAccountComponent } from "./edit-account.component";


@NgModule({
    declarations: [AccountListComponent, AccountTitleComponent, BankAccountDetailComponent, CreditCardDetailComponent, PortfolioDetailComponent, HistoryDiagramComponent, CumulatedTransactionComponent, CumulatedPositionComponent, EditAccountComponent],
    imports: [UniversalModule, SharedModule, NgxChartsModule, FormsModule, ReactiveFormsModule,
        RouterModule.forChild([
            { path: "accounts", component: AccountListComponent },
            { path: "bankaccount/:id/show", component: BankAccountDetailComponent },
            { path: "creditcard/:id/show", component: CreditCardDetailComponent },
            { path: "portfolio/:id/show", component: PortfolioDetailComponent },
            { path: "bankaccount/:id/edit", component: EditAccountComponent },
            { path: "creditcard/:id/edit", component: EditAccountComponent },
            { path: "portfolio/:id/edit", component: EditAccountComponent },
            { path: "bankaccount/create", component: EditAccountComponent },
            { path: "creditcard/create", component: EditAccountComponent },
            { path: "portfolio/create", component: EditAccountComponent },
            { path: "transactions", redirectTo: "transactions/page/1" },
            { path: "transactions/page/:page", component: CumulatedTransactionComponent },
            { path: "positions", redirectTo: "positions/page/1" },
            { path: "positions/page/:page", component: CumulatedPositionComponent },
            { path: "capitaldevelopment", component: HistoryDiagramComponent }
        ])],
    exports: [RouterModule],
    providers: [AccountService, TransactionService]
})
export class AccountModule { }
