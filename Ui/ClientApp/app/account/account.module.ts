import { NgModule } from "@angular/core";
import { RouterModule } from "@angular/router"
import { UniversalModule } from "angular2-universal";
import { NgxChartsModule } from '@swimlane/ngx-charts';
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { DatePickerModule } from 'ng2-datepicker';

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


@NgModule({
    declarations: [AccountListComponent, AccountTitleComponent, BankAccountDetailComponent, CreditCardDetailComponent, PortfolioDetailComponent, HistoryDiagramComponent, CumulatedTransactionComponent, CumulatedPositionComponent],
    imports: [UniversalModule, SharedModule, NgxChartsModule, FormsModule, ReactiveFormsModule, DatePickerModule
        RouterModule.forChild([
            { path: "accounts", component: AccountListComponent },
            { path: "bankaccount/:id", component: BankAccountDetailComponent },
            { path: "creditcard/:id", component: CreditCardDetailComponent },
            { path: "portfolio/:id", component: PortfolioDetailComponent },
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
