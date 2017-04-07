import { Component } from "@angular/core";
import { FormControl, FormGroup, Validators } from "@angular/forms";
import { ConfigurationService } from "./configuration.service";
import { DownloadHandlerConfiguration } from "./configuration.types";

@Component({
    selector: "configuration-download",
    templateUrl: "./configuration-download.component.html"
})
export class ConfigurationDownloadComponent {
    model: DownloadHandlerConfiguration[];
    selected: DownloadHandlerConfiguration;
    additionalForm: FormGroup;
    additionalArr: number[];

    constructor(private configurationService: ConfigurationService) {
        this.selected = undefined;
    }

    ngOnInit() {
        this.configurationService.getDownloadHandlerConfigurations().subscribe(model => {
            this.model = model;
            this.model = [...this.model];
        });
    }

    edit(model: DownloadHandlerConfiguration) {
        this.selected = model;
        this.additionalForm = new FormGroup({});
        this.additionalArr = new Array();
        for (const key of Object.keys(this.selected.additionalKeePassFields)) {
            this.addAdditionalKeePass(key, this.selected.additionalKeePassFields[key]);
        }
    }

    onDiscard() {
        this.selected = undefined;
        this.ngOnInit();
    }

    onSubmit() {
        this.selected.additionalKeePassFields = {};
        for (let i of this.additionalArr) {
            let keyControl = this.additionalForm.get("key" + i);
            let valueControl = this.additionalForm.get("value" + i);
            if (keyControl.valid && valueControl.valid) {
                this.selected.additionalKeePassFields[keyControl.value] = valueControl.value;
            }
        }
        console.log(this.selected);
        this.configurationService.editDownloadHandlerConfiguration(this.selected).subscribe();
        this.selected = undefined;
        this.ngOnInit();
    }

    addAdditionalKeePass(key: string = "", value: string = "") {
        let max = Math.max(...this.additionalArr);
        if (max == undefined || max < 0) {
            max = 0;
        }
        max++;
        this.additionalForm.addControl("key" + max, new FormControl(key, Validators.required));
        this.additionalForm.addControl("value" + max, new FormControl(value, Validators.required));
        this.additionalArr.push(max);
    }

    deleteAdditionalKeePass(index: number) {
        this.additionalForm.removeControl("key" + index);
        this.additionalForm.removeControl("value" + index);
        this.additionalArr.splice(this.additionalArr.indexOf(index), 1);
        console.log(this.additionalArr);
    }
}
