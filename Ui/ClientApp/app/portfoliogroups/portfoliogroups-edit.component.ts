import { Component, Input } from "@angular/core";
import { FormControl, FormGroup, Validators } from "@angular/forms";
import { ActivatedRoute, Router } from "@angular/router";

import { PortfolioGroupsService } from "./portfoliogroups.service";
import { PortfolioGroupModel } from "./portfoliogroups.types";

@Component({
    selector: "portfoliogroups-edit",
    templateUrl: "./portfoliogroups-edit.component.html"
})
export class PortfolioGroupsEditComponent {
    id: number;
    model: PortfolioGroupModel;

    constructor(private route: ActivatedRoute, private router: Router, private portfolioGroupService: PortfolioGroupsService) {
    }

    ngOnInit() {
        const route = this.route.snapshot;
        if (route.params["id"]) {
            this.id = Number(route.params["id"]);
            this.portfolioGroupService.getPortfolioGroup(this.id).subscribe(a => {
                this.model = a;
            });
        } else {
            this.model = new PortfolioGroupModel();
        }
    }

    addIsin() {
        this.model.assignedIsins.push("");
    }

    removeIsin(index:number) {
        this.model.assignedIsins.splice(index,1);
    }

    trackByIdx(index: any, item: any) {
        return index;
    }

    onSubmit() {
        this.portfolioGroupService.editOrCreatePortfolioGroup(this.model).subscribe();
        this.router.navigateByUrl("/portfoliogroup");
    }

    onCancel() {
        this.router.navigateByUrl("/portfoliogroup");
    }
}
