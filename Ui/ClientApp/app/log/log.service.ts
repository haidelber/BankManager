import { Injectable } from "@angular/core";
import { Http, Response, RequestOptions, URLSearchParams } from "@angular/http";
import { Observable } from "rxjs/Observable";
import "rxjs/add/operator/map";

@Injectable()
export class LogService {
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

    getLogFiles(): Observable<string[]> {
        return this.http.get("/api/Log").map(this.extractArr);
    }

    getLogFileContent(path: string): Observable<string> {
        let param = new URLSearchParams();
        param.append("path", path);
        return this.http.get("/api/Log/GetContent", { search: param }).map(this.extractObj);
    }
}
