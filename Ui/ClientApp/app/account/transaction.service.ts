﻿import { Injectable } from "@angular/core";
import { Http, Response, RequestOptions, URLSearchParams } from "@angular/http";
import { Observable } from "rxjs/Observable";
import "rxjs/add/operator/map";

import * as Model from "./transaction.types";

@Injectable()
export class TransactionService {
    constructor(private http: Http) {

    }

    private extractData(res: Response) {
        const body = res.json();
        return body.data || {};
    }

    private extractMessages(res: Response): string[] {
        const body = res.json();
        return body.messages || {};
    }

    getBankTransaction(id: number): Observable<Model.BankTransactionModel[]> {
        const param = new URLSearchParams();
        param.append("id", id.toString());
        return this.http.get(`/api/Transaction/BankAccount/${id}`).map(this.extractData);
    }

    getCreditCardTransaction(id: number): Observable<Model.BankTransactionForeignCurrencyModel[]> {
        const param = new URLSearchParams();
        param.append("id", id.toString());
        return this.http.get(`/api/Transaction/CreditCard/${id}`).map(this.extractData);
    }

    getPortfolioPosition(id: number): Observable<Model.PortfolioPositionModel[]> {
        const param = new URLSearchParams();
        param.append("id", id.toString());
        return this.http.get(`/api/Transaction/Portfolio/${id}`).map(this.extractData);
    }

    getCumulativeAccountTransactions(): Observable<Model.CumulativeTransactionModel[]> {
        return this.http.get(`/api/Transaction/Cumulative/Account`).map(this.extractData);
    }

    getMonthlyAggregatedAccountCapital(): Observable<Model.AggregatedTransactionModel[]> {
        return this.http.get(`/api/Transaction/Aggregated/Account`).map(this.extractData);
    }

    getCumulativePortfolioPosition(): Observable<Model.CumulativePositionModel[]> {
        return this.http.get(`/api/Transaction/Cumulative/Portfolio`).map(this.extractData);
    }

    getMontlyAggregatedPortfolioCapital(): Observable<Model.AggregatedTransactionModel[]> {
        return this.http.get(`/api/Transaction/Aggregated/Portfolio`).map(this.extractData);
    }
}
