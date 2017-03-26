import { Injectable } from "@angular/core";
import { Http, Response, RequestOptions, URLSearchParams } from "@angular/http";
import { Observable } from "rxjs/Observable";
import * as Model from "./download.model";

@Injectable()
export class DownloadService {
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

    getHandler(): Observable<Model.DownloadHandler[]> {
        return this.http.get("/api/Download").map(this.extractData);
    }

    startDownload(selectedKeys: string[], password: string): Observable<string> {
        const postModel = {
            "KeePassPassword": password,
            DownloadHandlerKeys: selectedKeys
        };
        return this.http.post("/api/Download/Run", postModel).map(this.extractData);
    }
}
