export class AccountModel {
    id: number;
    bankName: string;
    accountName: string;
}

export class BankAccountModel extends AccountModel {
    iban: string;
    accountNumber: string;
}

export class CreditCardAccountModel extends AccountModel {
    creditCardNumber: string;
    accountNumber: string;
}

export class PortfolioModel extends AccountModel {
    portfolioNumber: string;
}

export class EditAccountModel extends AccountModel {
    iban: string;
    accountNumber: string;
    creditCardNumber: string;
    portfolioNumber: string;

    toBankAccount(): BankAccountModel {
        return Object.assign(new BankAccountModel(), this);
    }
    toCreditCardAccount(): CreditCardAccountModel {
        return Object.assign(new CreditCardAccountModel(), this);
    }
    toPortfolio(): PortfolioModel {
        return Object.assign(new PortfolioModel(), this);
    }
}