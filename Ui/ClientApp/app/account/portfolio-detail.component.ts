import { Component, Input } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { PortfolioModel } from "./account.types";
import { PortfolioPositionModel } from "./transaction.types";
import { TransactionService } from "./transaction.service";
import { AccountService } from "./account.service";

@Component({
    selector: "portfolio-detail",
    templateUrl: "./portfolio-detail.component.html"
})
export class PortfolioDetailComponent {
    id:number;
    account: PortfolioModel;
    positions: PortfolioPositionModel[];
    portfolioSumCurrent: number;
    portfolioSumOriginal: number;
    currency: string;
    dateTime: Date;

    constructor(private transactionService: TransactionService, private accountService: AccountService, private route: ActivatedRoute) {

    }

    ngOnInit() {
        this.id = this.route.snapshot.params["id"];
        this.accountService.getPortfolioAccount(this.id).subscribe(model => {
            this.account = model;
        });
        this.transactionService.getPortfolioPosition(this.id)
            .subscribe(model => {
                //The model is retrieved inverse sorted from REST
                this.positions = model;
                this.portfolioSumCurrent = model.map(t => t.amount * t.currentValue)
                    .reduce((sum, current) => sum + current);
                this.portfolioSumOriginal = model.map(t => t.amount * t.originalValue)
                    .reduce((sum, current) => sum + current);
                this.dateTime = model[0].dateTime;
                this.currency = model[0].currentValueCurrencyIso;
            });
    }
}
