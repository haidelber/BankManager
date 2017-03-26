import { Component, Input } from "@angular/core";
import { BankAccountModel } from "./account.model";
import { BankTransactionModel } from "./transaction.model";
import { TransactionService } from "./transaction.service";

@Component({
    selector: "bankaccount-detail",
    templateUrl: "./bankaccount-detail.component.html"
})
export class BankAccountDetailComponent {
    @Input()
    account: BankAccountModel;
    transactions: BankTransactionModel[];
    transactionSum: number;
    currency: string;
    lastTransaction: Date;

    constructor(private transactionService: TransactionService) {

    }

    ngOnInit() {
        this.transactionService.getBankTransaction(this.account.id)
            .subscribe(model => {
                //The model is retrieved sorted from REST
                this.transactions = model;
                this.transactionSum = model.map(t => t.amount)
                    .reduce((sum, current) => sum + current);
                this.lastTransaction = model[model.length - 1].availabilityDate;
                this.currency = model[model.length - 1].currencyIso;
            });
    }
}
