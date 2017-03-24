export class AccountModel {
    id: string;
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