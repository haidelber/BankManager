import { Injectable } from "@angular/core";
import { Http, Response, RequestOptions, URLSearchParams } from "@angular/http";
import { Observable } from "rxjs/Observable";
import "rxjs/add/operator/map";

import * as Model from "./account.types";

@Injectable()
export class AccountService {
    constructor(private http: Http) {

    }

    private extractData(res: Response) {
        const body = res.json();
        return body.data || [];
    }

    private extractMessages(res: Response): string[] {
        const body = res.json();
        return body.messages || {};
    }

    getBankAccounts(): Observable<Model.BankAccountModel[]> {
        return this.http.get("/api/Account/BankAccount").map(this.extractData);
    }

    getCreditCardAccounts(): Observable<Model.CreditCardAccountModel[]> {
        return this.http.get("/api/Account/CreditCard").map(this.extractData);
    }

    getPortfolioAccounts(): Observable<Model.PortfolioModel[]> {
        return this.http.get("/api/Account/Portfolio").map(this.extractData);
    }

    getBankAccount(id: number): Observable<Model.BankAccountModel> {
        return this.http.get(`/api/Account/BankAccount/${id}`).map(this.extractData);
    }

    getCreditCardAccount(id: number): Observable<Model.CreditCardAccountModel> {
        return this.http.get(`/api/Account/CreditCard/${id}`).map(this.extractData);
    }

    getPortfolioAccount(id: number): Observable<Model.PortfolioModel> {
        return this.http.get(`/api/Account/Portfolio/${id}`).map(this.extractData);
    }

    editOrCreateBankAccount(model: Model.BankAccountModel): Observable<Model.BankAccountModel> {
        return this.http.post(`/api/Account/BankAccount`, model).map(this.extractData);
    }

    editOrCreateCreditCardAccount(model: Model.CreditCardAccountModel): Observable<Model.CreditCardAccountModel> {
        return this.http.post(`/api/Account/CreditCard`, model).map(this.extractData);
    }

    editOrCreatePortfolioAccount(model: Model.PortfolioModel): Observable<Model.PortfolioModel> {
        return this.http.post(`/api/Account/Portfolio`, model).map(this.extractData);
    }
}
