import { Component, Input } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { Form } from "@angular/forms"

import { AccountService } from "./account.service";
import { EditAccountModel, AccountModel } from "./account.model";

@Component({
    selector: "edit-account",
    templateUrl: "./edit-account.component.html"
})
export class EditAccountComponent {
    id: number;
    accountTypes = [{ key: "bankaccount", value: "Bank account" }, { key: "creditcard", value: "Credit card" }, { key: "portfolio", value: "Portfolio" }];
    accountType: any;
    model: EditAccountModel;

    constructor(private route: ActivatedRoute, private router: Router, private accountService: AccountService) {
    }

    ngOnInit() {
        const route = this.route.snapshot;
        this.accountType = route.url[0].path;
        if (route.params["id"]) {
            this.id = Number(route.params["id"]);
            switch (route.url[0].path) {
                case this.accountTypes[0].key:
                    this.accountService.getBankAccount(this.id).subscribe(a => this.model = this.toEditAccount(a));
                    break;
                case this.accountTypes[1].key:
                    this.accountService.getCreditCardAccount(this.id).subscribe(a => this.model = this.toEditAccount(a));
                    break;
                case this.accountTypes[2].key:
                    this.accountService.getPortfolioAccount(this.id).subscribe(a => this.model = this.toEditAccount(a));
                    break;
                default:
            }
        } else {
            this.model = new EditAccountModel();
        }
    }

    toEditAccount(model: any): EditAccountModel {
        return Object.assign(new EditAccountModel(), model);
    }

    onSubmit() {
        switch (this.accountType) {
            case this.accountTypes[0].key:
                this.accountService.editOrCreateBankAccount(this.model.toBankAccount()).subscribe();
                break;
            case this.accountTypes[1].key:
                this.accountService.editOrCreateCreditCardAccount(this.model.toCreditCardAccount()).subscribe();
                break;
            case this.accountTypes[2].key:
                this.accountService.editOrCreatePortfolioAccount(this.model.toPortfolio()).subscribe();
                break;
            default:
        }
        this.router.navigateByUrl("/");
    }

    onCancel() {
        this.router.navigateByUrl("/");
    }
}
