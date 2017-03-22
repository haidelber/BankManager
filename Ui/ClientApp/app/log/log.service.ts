import { Injectable } from "@angular/core";
import { Http, Response, RequestOptions, URLSearchParams } from "@angular/http";
import { Observable } from "rxjs/Observable";
import "rxjs/add/operator/map";

@Injectable()
export class LogService {
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

    getLogFiles(): Observable<string[]> {
        return this.http.get("/api/Log").map(this.extractData);
    }

    getLogFileContent(path: string): Observable<string> {
        let param = new URLSearchParams();
        param.append("path", path);
        return this.http.get("/api/Log/GetContent", { search: param }).map(this.extractData);
    }
}
