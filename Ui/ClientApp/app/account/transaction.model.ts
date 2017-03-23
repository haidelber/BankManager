export class BankTransactionModel {
    id: string;
    availabilityDate: Date;
    postingDate: Date;
    text: string;
    amount: number;
    currencyIso: string;
    accountId: string;
}

export class BankTransactionForeignCurrencyModel extends BankTransactionModel {
    amountForeignCurrency: number;
    foreignCurrencyIso: string;
    exchangeRate: number;
}

export class PortfolioPositionModel {
    id: string;
    isin: string;
    name: string;
    amount: number;
    dateTime: Date;
    currentValue: number;
    currentValueCurrencyIso: string;
    originalValue: number;
    originalValueCurrencyIso: string;
    portfolioId: string;
}