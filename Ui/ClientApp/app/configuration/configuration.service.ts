import { Injectable } from "@angular/core";
import { Http, Response, RequestOptions, URLSearchParams } from "@angular/http";
import { Observable } from "rxjs/Observable";
import "rxjs/add/operator/map";

import { DatabaseConfiguration, DownloadHandlerConfiguration, KeePassConfiguration } from "./configuration.types"

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

    getApplicationConfiguration(): Observable<string> {
        return this.http.get("/api/Configuration").map(resp => resp.json());
    }

    getConfigurationFilePath(): Observable<string> {
        return this.http.get("/api/Configuration/Path").map(this.extractObj);
    }

    generateConfigFile(): Observable<boolean> {
        return this.http.post("/api/Configuration/Generate", {}).map(this.extractObj);
    }

    getKeePassConfiguration(): Observable<KeePassConfiguration> {
        return this.http.get("/api/Configuration/KeePass").map(this.extractObj);
    }

    editKeePassConfiguration(model: KeePassConfiguration): Observable<KeePassConfiguration> {
        return this.http.post("/api/Configuration/KeePass", model).map(this.extractObj);
    }

    getDatabaseConfiguration(): Observable<DatabaseConfiguration> {
        return this.http.get("/api/Configuration/Database").map(this.extractObj);
    }

    editDatabaseConfiguration(model: DatabaseConfiguration): Observable<DatabaseConfiguration> {
        return this.http.post("/api/Configuration/Database", model).map(this.extractObj);
    }

    getDownloadHandlerConfigurations(): Observable<DownloadHandlerConfiguration[]> {
        return this.http.get("/api/Configuration/DownloadHandler").map(this.extractObj);
    }

    editDownloadHandlerConfiguration(model: DownloadHandlerConfiguration): Observable<DownloadHandlerConfiguration> {
        return this.http.post("/api/Configuration/DownloadHandler", model).map(this.extractObj);
    }

    getDownloadHandlerConfiguration(key: string): Observable<DownloadHandlerConfiguration> {
        return this.http.get(`/api/Configuration/DownloadHandler/${key}`).map(this.extractObj);
    }
}
