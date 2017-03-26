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
        accountService.getBankAccounts().subscribe(model => this.bankAccounts = model);
        accountService.getCreditCardAccounts().subscribe(model => this.creditCardAccounts = model);
        accountService.getPortfolioAccounts().subscribe(model => this.portfolioAccounts = model);
    }
}
