import { Component } from "@angular/core";
import { Http } from "@angular/http";
import { List } from "linqts";
import { FormGroup, FormBuilder, Validators, ReactiveFormsModule } from '@angular/forms'

@Component({
    selector: "downloadhandler",
    template: require("./downloadhandler.component.html")
})
export class DownloadHandlerComponent {
    private http: Http;
    public handlers: DownloadHandler[];
    public password: string;
    public errorMessage: string;

    constructor(http: Http) {
        this.http = http;
        this.http.get("/api/DownloadHandler").subscribe(result =>
            this.handlers = result.json(),
            error => this.errorMessage = error);
        this.password = "";
    }

    checkAllOnChange(event) {
        for (let handler of this.handlers) {
            handler.selected = event.target.checked;
        }
    }

    startDownload() {
        const selectedKey: string[] = [];
        for (let handler of this.handlers) {
            if (handler.selected) {
                selectedKey.push(handler.key);
            }
        }
        const postModel = {
            "KeePassPassword": this.password,
            DownloadHandlerKeys: selectedKey
        };
        this.http.post("/api/DownloadHandler/RunDownloadHandler", postModel
        ).subscribe(result => { console.log(result) },
            error => this.errorMessage = error);
    }
}

interface DownloadHandler {
    key: string;
    url: string;
    path: string;
    selected: boolean;
    displayName: string;
}