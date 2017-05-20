import { Component } from "@angular/core";
import { PortfolioGroupsService } from "./portfoliogroups.service";
import { PortfolioGroupModel } from "./portfoliogroups.types";

@Component({
    selector: "portfolio-detail",
    templateUrl: "./portfoliogroups-detail.component.html"
})
export class PortfolioGroupsDetailComponent {
    public groups: PortfolioGroupModel[];

    constructor(private portfolioService: PortfolioGroupsService) {
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
