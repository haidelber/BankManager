import { Injectable } from "@angular/core";
import { Http, Response, RequestOptions, URLSearchParams } from "@angular/http";
import { Observable } from "rxjs/Observable";
import "rxjs/add/operator/map";

import { ApplicationConfiguration } from "./configuration.model"

@Injectable()
export class ConfigurationService {
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

    getConfiguration(): Observable<ApplicationConfiguration> {
        return this.http.get("/api/Configuration").map(this.extractData);
    }

    getConfigurationFilePath(): Observable<string> {
        return this.http.get("/api/Configuration/ConfigurationFilePath").map(this.extractData);
    }

    generateConfigFile(): Observable<boolean> {
        return this.http.post("/api/Configuration/GenerateConfigFile", {}).map(this.extractData);
    }
}
