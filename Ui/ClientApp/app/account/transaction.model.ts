export class BankTransactionModel {
    id: number;
    availabilityDate: Date;
    postingDate: Date;
    text: string;
    amount: number;
    currencyIso: string;
    accountId: number;
}

export class BankTransactionForeignCurrencyModel extends BankTransactionModel {
    amountForeignCurrency: number;
    foreignCurrencyIso: string;
    exchangeRate: number;
}

export class PortfolioPositionModel {
    id: number;
    isin: string;
    name: string;
    amount: number;
    dateTime: Date;
    currentValue: number;
    currentValueCurrencyIso: string;
    originalValue: number;
    originalValueCurrencyIso: string;
    portfolioId: number;
}