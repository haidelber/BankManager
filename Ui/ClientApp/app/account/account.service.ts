import { Injectable } from "@angular/core";
import { Http, Response, RequestOptions, URLSearchParams } from "@angular/http";
import { Observable } from "rxjs/Observable";
import "rxjs/add/operator/map";

import * as Model from "./account.model";

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
}
