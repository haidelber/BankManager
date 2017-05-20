import { Injectable } from "@angular/core";
import { Http, Response, RequestOptions, URLSearchParams } from "@angular/http";
import { Observable } from "rxjs/Observable";
import "rxjs/add/operator/map";

import { PortfolioGroupModel } from "./portfoliogroups.types";
import { PortfolioPositionModel } from "../account/transaction.types"

@Injectable()
export class PortfolioGroupsService {
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

    getPortfolioGroups(): Observable<PortfolioGroupModel[]> {
        return this.http.get("/api/Portfolio/Group").map(this.extractArr);
    }

    getPortfolioGroupPositions(id: number): Observable<PortfolioPositionModel[]> {
        return this.http.get(`/api/Portfolio/Group/${id}/Position`).map(this.extractArr);
    }

    getPortfolioGroup(id: number): Observable<PortfolioGroupModel> {
        return this.http.get(`/api/Portfolio/Group/${id}`).map(this.extractObj);
    }

    editOrCreatePortfolioGroup(model: PortfolioGroupModel) {
        return this.http.post(`/api/Portfolio/Group`, model).map(this.extractObj);
    }
}
