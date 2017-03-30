import { Component } from "@angular/core";
import { ActivatedRoute } from "@angular/router";

import { TransactionService } from "./transaction.service";
import { CumulativePositionModel } from "./transaction.types";

@Component({
    selector: "cumulated-position",
    templateUrl: "./cumulated-position.component.html"
})
export class CumulatedPositionComponent {
    page: number;
    pageSize = +25;

    transactions: CumulativePositionModel[];
    currency: string;

    constructor(private transactionService: TransactionService, private route: ActivatedRoute) {
    }
    ngOnInit() {
        this.route.params.subscribe(params => {
            this.page = Number(this.route.snapshot.params["page"]);
        });

        this.transactionService.getCumulativePortfolioPosition().subscribe(model => {
            this.transactions = model;
            this.currency = model[model.length - 1].currentValueCurrencyIso;
        });
    }
}
