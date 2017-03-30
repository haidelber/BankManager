import { Component, Input } from "@angular/core";
import { BankAccountModel, CreditCardAccountModel, PortfolioModel } from "./account.types";

@Component({
    selector: "account-title",
    templateUrl: "./account-title.component.html"
})
export class AccountTitleComponent {
    @Input()
    bankAccount: BankAccountModel;
    @Input()
    creditCardAccount: CreditCardAccountModel;
    @Input()
    portfolio: PortfolioModel;

    bank: string;
    accountName: string;
    accountNumber: string;

    ngOnInit() {
        if (this.bankAccount) {
            this.bank = this.bankAccount.bankName;
            this.accountName = this.bankAccount.accountName;
            this.accountNumber = this.bankAccount.iban ? this.bankAccount.iban : this.bankAccount.accountNumber;
        } else if (this.creditCardAccount) {
            this.bank = this.creditCardAccount.bankName;
            this.accountName = this.creditCardAccount.accountName;
            this.accountNumber = this.creditCardAccount.creditCardNumber ? this.creditCardAccount.creditCardNumber : this.creditCardAccount.accountNumber;
        } else if (this.portfolio) {
            this.bank = this.portfolio.bankName;
            this.accountName = this.portfolio.accountName;
            this.accountNumber = this.portfolio.portfolioNumber;
        }
    }
}
