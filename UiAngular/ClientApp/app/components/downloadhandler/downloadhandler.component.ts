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

    constructor(http: Http) {
        this.http = http;
        this.http.get("/api/DownloadHandler/").subscribe(result => {
            this.handlers = result.json();
        });
    }

    checkAllOnChange(event) {
        for (let handler of this.handlers) {
            handler.selected = event.target.checked;
        }
    }

    startDownload(value: any) {
        console.log("start");
        let handlerList = new List<DownloadHandler>(this.handlers);
        this.http.post("/api/DownloadHandler/",
            {
                "KeePassPassword": value.password,
                DownloadHandlerKeys: handlerList.Where(h => h.selected).Select(h => h.key)
            });
    }
}

interface DownloadHandler {
    key: string;
    url: string;
    path: string;
    selected: boolean;
    displayName: string;
}