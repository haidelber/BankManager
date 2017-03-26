import { Component, Input } from "@angular/core";
import { PortfolioModel } from "./account.model";
import { PortfolioPositionModel } from "./transaction.model";
import { TransactionService } from "./transaction.service";

@Component({
    selector: "portfolio-detail",
    templateUrl: "./portfolio-detail.component.html"
})
export class PortfolioDetailComponent {
    @Input()
    account: PortfolioModel;
    positions: PortfolioPositionModel[];
    portfolioSum: number;
    currency: string;
    dateTime: Date;

    constructor(private transactionService: TransactionService) {

    }

    ngOnInit() {
        this.transactionService.getPortfolioPosition(this.account.id)
            .subscribe(model => {
                //The model is retrieved sorted from REST
                this.positions = model;
                this.portfolioSum = model.map(t => t.amount)
                    .reduce((sum, current) => sum + current);
                this.dateTime = model[model.length - 1].dateTime;
                this.currency = model[model.length - 1].currentValueCurrencyIso;
            });
    }
}
