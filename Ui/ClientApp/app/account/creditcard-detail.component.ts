import { Component, Input } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { CreditCardAccountModel } from "./account.types";
import { BankTransactionForeignCurrencyModel } from "./transaction.types";
import { TransactionService } from "./transaction.service";
import { AccountService } from "./account.service";

@Component({
    selector: "creditcard-detail",
    templateUrl: "./creditcard-detail.component.html"
})
export class CreditCardDetailComponent {
    id: number;
    account: CreditCardAccountModel;
    transactions: BankTransactionForeignCurrencyModel[];
    transactionSum: number;
    currency: string;
    lastTransaction: Date;

    constructor(private transactionService: TransactionService, private accountService: AccountService, private route: ActivatedRoute) {

    }

    ngOnInit() {
        this.id = this.route.snapshot.params["id"];
        this.accountService.getCreditCardAccount(this.id).subscribe(model => {
            this.account = model;
        });
        this.transactionService.getCreditCardTransaction(this.id)
            .subscribe(model => {
                //The model is retrieved inverse sorted from REST
                this.transactions = model;
                this.transactionSum = model.map(t => t.amount)
                    .reduce((sum, current) => sum + current);
                this.lastTransaction = model[0].availabilityDate;
                this.currency = model[0].currencyIso;
            });
    }

    deleteTransaction(trans: BankTransactionForeignCurrencyModel) {
        let result = window.confirm(`Are you sure you want to delete the transaction ${trans.amount} ${trans.text}`);
        if (result) {
            this.transactionService.deleteCreditCardTransaction(trans.id).subscribe(t => {
                var index = this.transactions.indexOf(trans);
                if (index > -1) {
                    this.transactions.splice(index, 1);
                }
            });
        }
    }
}
