﻿import { Component } from "@angular/core";
import { Http } from "@angular/http";
import { List } from "linqts";
import { FormGroup, FormBuilder, Validators, ReactiveFormsModule } from "@angular/forms"

@Component({
    selector: "download",
    templateUrl: "./download.component.html"
})
export class DownloadComponent {
    private http: Http;
    public handlers: DownloadHandler[];
    public password: string;
    public errorMessage: string;

    constructor(http: Http) {
        this.http = http;
        this.http.get("/api/Download").subscribe(result =>
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
        this.http.post("/api/Download/Run", postModel
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