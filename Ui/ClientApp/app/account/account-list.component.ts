import { Component } from "@angular/core";
import { AccountService } from "./account.service";
import * as Model from "./account.model";

@Component({
    selector: "account-list",
    templateUrl: "./account-list.component.html"
})
export class AccountListComponent {
    public bankAccounts: Model.BankAccountModel[];
    public creditCardAccounts: Model.CreditCardAccountModel[];
    public portfolioAccounts: Model.PortfolioModel[];

    constructor(private accountService: AccountService) {
    }

    ngOnInit() {
        this.accountService.getBankAccounts().subscribe(model => this.bankAccounts = model);
        this.accountService.getCreditCardAccounts().subscribe(model => this.creditCardAccounts = model);
        this.accountService.getPortfolioAccounts().subscribe(model => this.portfolioAccounts = model);
    }
}
