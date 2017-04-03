import { Component } from "@angular/core";

import { TransactionService } from "./transaction.service";
import { AggregatedTransactionModel } from "./transaction.types";

@Component({
    selector: "history-diagram",
    templateUrl: "./history-diagram.component.html"
})
export class HistoryDiagramComponent {
    dataCapital: any[];
    xAxisLabelCapital = "Time";
    yAxisLabelCapital = "Capital";

    dataStdDev: any[];
    xAxisLabelStdDev = "Time";
    yAxisLabelStdDev = "Capital";

    colorScheme = {
        domain: ["#5AA454", "#A10A28", "#C7B42C", "#AAAAAA"]
    };
    view = undefined;
    showXAxis = true;
    showYAxis = true;
    gradient = false;
    showLegend = true;
    showXAxisLabel = true;
    showYAxisLabel = true;
    
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

    constructor(private transactionService: TransactionService) {
        this.dataCapital = new Array();
        this.dataStdDev = new Array();
    }
    ngOnInit() {
        this.transactionService.getMonthlyAggregatedAccountCapital().subscribe(model => {
            this.accountCapital = model;

            for (var point of model) {
                let date = new Date(point.year + "-" + point.month);
                this.avgAccount.series.push({ name: date, value: point.average });
                this.stdDevAccount.series.push({ name: date, value: point.stdDev });
            }
            this.dataCapital.push(this.avgAccount);
            this.dataCapital = [...this.dataCapital];
            this.dataStdDev.push(this.stdDevAccount);
            this.dataStdDev = [...this.dataStdDev];
        });
        this.transactionService.getMontlyAggregatedPortfolioCapital().subscribe(model => {
            this.portfolioCapital = model;

            for (var point of model) {
                let date = new Date(point.year + "-" + point.month);
                this.avgPortfolio.series.push({ name: date, value: point.average });
                this.stdDevPortfolio.series.push({ name: date, value: point.stdDev });
            }
            this.dataCapital.push(this.avgPortfolio);
            //This triggers change detection
            this.dataCapital = [...this.dataCapital];
            this.dataStdDev.push(this.stdDevPortfolio);
            this.dataStdDev = [...this.dataStdDev];
        });
    }

    applyFilter() {

    }
}