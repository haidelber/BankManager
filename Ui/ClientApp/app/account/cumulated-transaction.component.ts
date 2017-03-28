import { Component } from "@angular/core";
import { ActivatedRoute } from "@angular/router";

import { TransactionService } from "./transaction.service";
import { CumulativeTransactionModel } from "./transaction.model";

@Component({
    selector: "cumulated-transaction",
    templateUrl: "./cumulated-transaction.component.html"
})
export class CumulatedTransactionComponent {
    page: number;
    pageSize = +25;

    transactions: CumulativeTransactionModel[];
    currency: string;

    constructor(private transactionService: TransactionService, private route: ActivatedRoute) {
    }
    ngOnInit() {
        this.route.params.subscribe(params => {
            this.page = Number(this.route.snapshot.params["page"]);
        });

        this.transactionService.getCumulativeAccountTransactions().subscribe(model => {
            this.transactions = model;
            this.currency = model[model.length - 1].currencyIso;
        });
    }
}
