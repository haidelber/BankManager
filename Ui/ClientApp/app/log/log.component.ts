import { Component } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { LogService } from "./log.service";

@Component({
    selector: "log",
    templateUrl: "./log.component.html"
})
export class LogComponent {
    index: number;
    logFiles: LogModel[];
    logFileContent: string;

    constructor(private logService: LogService, private route: ActivatedRoute) {

    }

    ngOnInit() {
        this.route.params.subscribe(params => {
            this.index = Number(this.route.snapshot.params["index"]);
            this.loadFile();
        });

        this.logService.getLogFiles().subscribe(res => {
            const paths = res.sort().reverse();
            this.logFiles = new Array<LogModel>();
            for (let i = 0; i < paths.length; i++) {
                this.logFiles.push({ index: i, selected: false, path: paths[i] });
            }
            this.loadFile();
        });
    }

    loadFile() {
        if (this.logFiles != undefined && this.index != undefined) {
            this.logService.getLogFileContent(this.logFiles[this.index].path).subscribe(cont => {
                this.logFileContent = cont;
                for (const logFile of this.logFiles) {
                    logFile.selected = false;
                }
                this.logFiles[this.index].selected = true;
            });
        }
    }
}

class LogModel {
    index: number;
    selected: boolean;
    path: string;
}
