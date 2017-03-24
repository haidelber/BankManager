import { Component, Input } from '@angular/core';
import { AccountModel } from "./account.model";
import { BankTransactionModel, BankTransactionForeignCurrencyModel } from "./transaction.model";
import { TransactionService } from "./transaction.service";

@Component({
    selector: 'account-detail',
    templateUrl: "./account-detail.component.html"
})
export class AccountDetailComponent {
    @Input() account: AccountModel;
    bankTransactions: BankTransactionForeignCurrencyModel[];
    constructor(private transactionService: TransactionService) {

    }

    loadTransactions() {
        this.transactionService.getBankTransaction(this.account.id).subscribe(model => this.bankTransactions = model);
    }
}
