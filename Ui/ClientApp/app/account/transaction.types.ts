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

export class CumulativeTransactionModel extends BankTransactionModel {
    cumulative: number;
}

export class CumulativePositionModel extends PortfolioPositionModel {
    amountChange: number;
    valuePerItemChange: number;
    cumulative: number;
}

export class AggregatedTransactionModel {
    year: number;
    month: number;
    average: number;
    stdDev: number;
}