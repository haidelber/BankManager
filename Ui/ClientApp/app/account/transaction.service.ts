import { Injectable } from "@angular/core";
import { Http, Response, RequestOptions, URLSearchParams } from "@angular/http";
import { Observable } from "rxjs/Observable";
import "rxjs/add/operator/map";

import * as Model from "./transaction.types";

@Injectable()
export class TransactionService {
    constructor(private http: Http) {

    }

    private extractObj(res: Response) {
        const body = res.json();
        return body || {};
    }

    private extractArr(res: Response) {
        const body = res.json();
        return body || [];
    }

    getBankTransaction(id: number): Observable<Model.BankTransactionModel[]> {
        const param = new URLSearchParams();
        param.append("accountId", id.toString());
        return this.http.get(`/api/Transaction/BankAccount`, { search: param }).map(this.extractArr);
    }

    getCreditCardTransaction(id: number): Observable<Model.BankTransactionForeignCurrencyModel[]> {
        const param = new URLSearchParams();
        param.append("accountId", id.toString());
        return this.http.get(`/api/Transaction/CreditCard`, { search: param }).map(this.extractArr);
    }

    getPortfolioPosition(id: number): Observable<Model.PortfolioPositionModel[]> {
        const param = new URLSearchParams();
        param.append("portfolioId", id.toString());
        return this.http.get(`/api/Transaction/Portfolio`, { search: param }).map(this.extractArr);
    }

    deleteBankTransaction(id: number): Observable<Model.BankTransactionModel> {
        return this.http.delete(`/api/Transaction/BankAccount/${id}`).map(this.extractObj);
    }

    deleteCreditCardTransaction(id: number): Observable<Model.BankTransactionForeignCurrencyModel> {
        return this.http.delete(`/api/Transaction/CreditCard/${id}`).map(this.extractObj);
    }

    deletePortfolioPosition(id: number): Observable<Model.PortfolioPositionModel> {
        return this.http.delete(`/api/Transaction/Portfolio/${id}`).map(this.extractObj);
    }

    getCumulativeAccountTransactions(): Observable<Model.CumulativeTransactionModel[]> {
        return this.http.get(`/api/Transaction/Cumulative/Account`).map(this.extractArr);
    }

    getMonthlyAggregatedAccountCapital(): Observable<Model.AggregatedTransactionModel[]> {
        return this.http.get(`/api/Transaction/Aggregated/Account`).map(this.extractArr);
    }

    getCumulativePortfolioPosition(): Observable<Model.CumulativePositionModel[]> {
        return this.http.get(`/api/Transaction/Cumulative/Portfolio`).map(this.extractArr);
    }

    getMontlyAggregatedPortfolioCapital(): Observable<Model.AggregatedTransactionModel[]> {
        return this.http.get(`/api/Transaction/Aggregated/Portfolio`).map(this.extractArr);
    }
}
