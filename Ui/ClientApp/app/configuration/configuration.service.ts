import { Injectable } from "@angular/core";
import { Http, Response, RequestOptions, URLSearchParams } from "@angular/http";
import { Observable } from "rxjs/Observable";
import "rxjs/add/operator/map";

import { ApplicationConfiguration } from "./configuration.model"

@Injectable()
export class ConfigurationService {
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

    getConfiguration(): Observable<any> {
        return this.http.get("/api/Configuration").map(resp => resp.json());
    }

    getConfigurationFilePath(): Observable<string> {
        return this.http.get("/api/Configuration/Path").map(this.extractObj);
    }

    generateConfigFile(): Observable<boolean> {
        return this.http.post("/api/Configuration/Generate", {}).map(this.extractObj);
    }
}
