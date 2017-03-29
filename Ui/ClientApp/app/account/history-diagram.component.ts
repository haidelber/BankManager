import { Component } from "@angular/core";

import { TransactionService } from "./transaction.service";
import { AggregatedTransactionModel } from "./transaction.model";

@Component({
    selector: "history-diagram",
    templateUrl: "./history-diagram.component.html"
})
export class HistoryDiagramComponent {
    data: any[];

    colorScheme = {
        domain: ["#5AA454", "#A10A28", "#C7B42C", "#AAAAAA"]
    };
    view = undefined;
    showXAxis = true;
    showYAxis = true;
    gradient = false;
    showLegend = true;
    showXAxisLabel = true;
    xAxisLabel = "Time";
    showYAxisLabel = true;
    yAxisLabel = "Capital";
    timeline = true;

    onSelect(event) {
        console.log(event);
    }

    accountCapital: AggregatedTransactionModel[];
    portfolioCapital: AggregatedTransactionModel[];
    avgAccount = { name: "Avg Account Capital", series: [] };
    stdDevAccount = { name: "StdDev Account Capital", series: [] };
    avgPortfolio = { name: "Avg Portfolio Capital", series: [] };
    stdDevPortfolio = { name: "StdDev Portfolio Capital", series: [] };

    //fromYear = 0;
    //toYear = 0;

    constructor(private transactionService: TransactionService) {
        this.data = new Array();
    }
    ngOnInit() {
        this.transactionService.getMonthlyAggregatedAccountCapital().subscribe(model => {
            this.accountCapital = model;

            for (var point of model) {
                let date = new Date(point.year + "-" + point.month);
                this.avgAccount.series.push({ name: date, value: point.average });
                this.stdDevAccount.series.push({ name: date, value: point.stdDev });
            }
            this.data.push(this.avgAccount);
            this.data = [...this.data];
            //this.fromYear = model[model.length - 1].year;
            //this.toYear = model[0].year;
        });
        this.transactionService.getMontlyAggregatedPortfolioCapital().subscribe(model => {
            this.portfolioCapital = model;

            for (var point of model) {
                let date = new Date(point.year + "-" + point.month);
                this.avgPortfolio.series.push({ name: date, value: point.average });
                this.stdDevPortfolio.series.push({ name: date, value: point.stdDev });
            }
            this.data.push(this.avgPortfolio);
            //This triggers change detection
            this.data = [...this.data];
        });
    }

    applyFilter() {

    }
}