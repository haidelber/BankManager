import { Injectable } from "@angular/core";
import { Http, Response, RequestOptions, URLSearchParams } from "@angular/http";
import { Observable } from "rxjs/Observable";
import * as Model from "./import.types";

@Injectable()
export class ImportService {
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

    getFileParserConfiguration(): Observable<string[]> {
        return this.http.get("/api/Import/FileParserConfiguration").map(this.extractData);
    }

    startImport(postModel: Model.ImportServiceRunModel): Observable<string> {
        return this.http.post("/api/Import/Run", postModel).map(this.extractData);
    }
}
