import { Injectable } from "@angular/core";
import { Http, Response, RequestOptions, URLSearchParams } from "@angular/http";
import { Observable } from "rxjs/Observable";
import "rxjs/add/operator/map";

import * as Model from "./account.types";

@Injectable()
export class AccountService {
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

    getBankAccounts(): Observable<Model.BankAccountModel[]> {
        return this.http.get("/api/Account/BankAccount").map(this.extractArr);
    }

    getCreditCardAccounts(): Observable<Model.CreditCardAccountModel[]> {
        return this.http.get("/api/Account/CreditCard").map(this.extractArr);
    }

    getPortfolioAccounts(): Observable<Model.PortfolioModel[]> {
        return this.http.get("/api/Account/Portfolio").map(this.extractArr);
    }

    getBankAccount(id: number): Observable<Model.BankAccountModel> {
        return this.http.get(`/api/Account/BankAccount/${id}`).map(this.extractObj);
    }

    getCreditCardAccount(id: number): Observable<Model.CreditCardAccountModel> {
        return this.http.get(`/api/Account/CreditCard/${id}`).map(this.extractObj);
    }

    getPortfolioAccount(id: number): Observable<Model.PortfolioModel> {
        return this.http.get(`/api/Account/Portfolio/${id}`).map(this.extractObj);
    }

    editOrCreateBankAccount(model: Model.BankAccountModel): Observable<Model.BankAccountModel> {
        return this.http.post(`/api/Account/BankAccount`, model).map(this.extractObj);
    }

    editOrCreateCreditCardAccount(model: Model.CreditCardAccountModel): Observable<Model.CreditCardAccountModel> {
        return this.http.post(`/api/Account/CreditCard`, model).map(this.extractObj);
    }

    editOrCreatePortfolioAccount(model: Model.PortfolioModel): Observable<Model.PortfolioModel> {
        return this.http.post(`/api/Account/Portfolio`, model).map(this.extractObj);
    }
}
