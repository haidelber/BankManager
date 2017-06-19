import { Component } from "@angular/core";
import { PortfolioGroupsService } from "./portfoliogroups.service";
import { PortfolioGroupModel } from "./portfoliogroups.types";
import { PortfolioPositionModel } from "../account/transaction.types";

@Component({
    selector: "portfolio-detail",
    templateUrl: "./portfoliogroups-detail.component.html"
})
export class PortfolioGroupsDetailComponent {
    groups: PortfolioGroupModel[];
    groupPositionVisibility: boolean[];
    positions: PortfolioPositionModel[];
    thresholdPercentage = 15;
    amount = 1000;
    currency: string;
    rebalanceMonths = 3;

    constructor(private portfolioService: PortfolioGroupsService) {
    }

    ngOnInit() {
        this.portfolioService.getPortfolioGroups().subscribe(model => {
            this.groups = model;
            this.groupPositionVisibility = new Array<boolean>();
            for (let group of model) {
                this.groupPositionVisibility.push(false);
                this.portfolioService.getPortfolioGroupPositions(group.id).subscribe(pos => group.positions = pos);
            }
        });
        this.portfolioService.getAllPortfolioPositions().subscribe(model => { this.positions = model; this.currency = model[0].currentValueCurrencyIso });
    }

    currentPositionSum(positions: PortfolioPositionModel[]): number {
        if (positions) {
            return positions.map(pos => pos.currentValue * pos.amount).reduce((prev, curr) => prev + curr);
        }
        return 0;
    }

    allGroupsPositionsSum(): number {
        return this.groups.filter(grp => grp.includeInCalculations)
            .map(grp => grp.positions).reduce((prev, curr) => prev.concat(curr))
            .map(pos => pos.currentValue * pos.amount).reduce((prev, curr) => prev + curr);
    }

    threshold(): number {
        return this.thresholdPercentage / 100;
    }

    lowerThresholdPercent(targetPercentage: number): number {
        return targetPercentage * (1 - this.threshold());
    }

    upperThresholdPercent(targetPercentage: number): number {
        return targetPercentage * (1 + this.threshold());
    }

    lowerThreshold(targetPercentage: number): number {
        return this.lowerThresholdPercent(targetPercentage) * this.allGroupsPositionsSum() / 100;
    }

    upperThreshold(targetPercentage: number): number {
        return this.upperThresholdPercent(targetPercentage) * this.allGroupsPositionsSum() / 100;
    }

    target(targetPercentage: number): number {
        return targetPercentage * this.allGroupsPositionsSum() / 100;
    }

    difference(targetPercentage: number, positions: PortfolioPositionModel[]): number {
        return this.currentPositionSum(positions) - this.target(targetPercentage);
    }

    monthlyInvestment(targetPercentage: number): number {
        return this.amount * targetPercentage / 100;
    }

    monthlyRebalance(targetPercentage: number, positions: PortfolioPositionModel[]): number {
        let diff = this.difference(targetPercentage, positions);
        let monthly = this.monthlyInvestment(targetPercentage);

        if (diff > this.rebalanceMonths * monthly) {
            //Can't rebalance because group value too high
        }
        if (Math.abs(diff) > this.rebalanceMonths * this.amount) {
            //Can't rebalance because group value too little
        }
        return (this.rebalanceMonths * monthly - diff) / this.rebalanceMonths;
    }
}
