import { Injectable } from "@angular/core";
import { Http, Response, RequestOptions, URLSearchParams } from "@angular/http";
import { Observable } from "rxjs/Observable";
import * as Model from "./import.types";

@Injectable()
export class ImportService {
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

    getFileParserConfiguration(): Observable<string[]> {
        return this.http.get("/api/Import/FileParserConfiguration").map(this.extractArr);
    }

    startImport(postModel: Model.ImportServiceRunModel): Observable<string> {
        return this.http.post("/api/Import/Run", postModel).map(this.extractObj);
    }
}
