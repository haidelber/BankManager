import { Component } from "@angular/core";
import { PortfolioService } from "./portfolio.service";
import {PortfolioGroupModel} from "./portfolio.types";

@Component({
    selector: "portfolio-detail",
    templateUrl: "./portfolio-detail.component.html"
})
export class PortfolioDetailComponent {
    public groups:PortfolioGroupModel[];

    constructor(private portfolioService: PortfolioService) {
    }

    ngOnInit() {
        this.portfolioService.getPortfolioGroups().subscribe(model => {
            this.groups = model;
            for (let group of model) {
                this.portfolioService.getPortfolioGroupPositions(group.id).subscribe(pos => group.positions = pos);
            }
        });
    }
}
