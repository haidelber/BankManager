import { Component, Input } from "@angular/core";
import { PortfolioPositionModel } from "./transaction.types";
import { TransactionService } from "./transaction.service";

@Component({
    selector: "portfolioposition-list",
    templateUrl: "./portfolioposition-list.component.html"
})
export class PortfolioPositionListComponent {
    @Input()
    positions: PortfolioPositionModel[];
    @Input()
    canDelete: boolean = false;
    @Input()
    hasPortfolioId: boolean = false;

    portfolioSumCurrent: number;
    portfolioSumOriginal: number;
    currency: string;
    dateTime: Date;

    constructor(private transactionService: TransactionService) {

    }

    ngOnInit() {
        this.portfolioSumCurrent = this.positions.map(t => t.amount * t.currentValue)
            .reduce((sum, current) => sum + current);
        this.portfolioSumOriginal = this.positions.map(t => t.amount * t.originalValue)
            .reduce((sum, current) => sum + current);
        this.dateTime = this.positions[0].dateTime;
        this.currency = this.positions[0].currentValueCurrencyIso;
    }

    deleteTransaction(trans: PortfolioPositionModel) {
        let result = window.confirm(`Are you sure you want to delete the position ${trans.dateTime} ${trans.name}`);
        if (result) {
            this.transactionService.deletePortfolioPosition(trans.id).subscribe(t => {
                var index = this.positions.indexOf(trans);
                if (index > -1) {
                    this.positions.splice(index, 1);
                }
            });
        }
    }
}
