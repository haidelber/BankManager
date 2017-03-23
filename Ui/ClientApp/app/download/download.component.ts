import { Component } from "@angular/core";
import { FormGroup, FormBuilder, Validators, ReactiveFormsModule } from "@angular/forms"
import { DownloadService } from "./download.service";
import * as Model from "./download.model";

@Component({
    selector: "download",
    templateUrl: "./download.component.html"
})
export class DownloadComponent {
    public handlers: Model.DownloadHandler[];
    public password: string;
    public errorMessage: string;

    constructor(private downloadService: DownloadService) {
        this.password = "";
        this.downloadService.getHandler().subscribe(handlers => this.handlers = handlers);
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
        this.downloadService.startDownload(selectedKey, this.password).subscribe(response => console.log(response));
    }
}

