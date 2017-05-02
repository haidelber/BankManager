import { Injectable } from "@angular/core";
import { Http, Response, RequestOptions, URLSearchParams } from "@angular/http";
import { Observable } from "rxjs/Observable";
import * as Model from "./download.types";

@Injectable()
export class DownloadService {
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

    getHandler(): Observable<Model.DownloadHandler[]> {
        return this.http.get("/api/Download").map(this.extractArr);
    }

    startDownload(selectedKeys: string[], password: string, downloadStatements: boolean): Observable<string> {
        const postModel = {
            "KeePassPassword": password,
            "DownloadHandlerKeys": selectedKeys,
            "DownloadStatements": downloadStatements
        };
        return this.http.post("/api/Download/Run", postModel).map(this.extractObj);
    }
}
