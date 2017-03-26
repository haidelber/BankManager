import { Component, Input } from "@angular/core";
import { CreditCardAccountModel } from "./account.model";
import { BankTransactionForeignCurrencyModel } from "./transaction.model";
import { TransactionService } from "./transaction.service";

@Component({
    selector: "creditcard-detail",
    templateUrl: "./creditcard-detail.component.html"
})
export class CreditCardDetailComponent {
    @Input()
    account: CreditCardAccountModel;
    transactions: BankTransactionForeignCurrencyModel[];
    transactionSum: number;
    currency: string;
    lastTransaction: Date;

    constructor(private transactionService: TransactionService) {

    }

    ngOnInit() {
        this.transactionService.getCreditCardTransaction(this.account.id)
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
