import { Component } from "@angular/core";
import { LogService } from "./log.service";

@Component({
    selector: "log",
    templateUrl: "./log.component.html"
})
export class LogComponent {
    logFiles: LogModel[];
    logFileContent: string;

    constructor(private logService: LogService) {
        logService.getLogFiles().subscribe(res => {
            const paths = res.sort().reverse();
            this.logFiles = new Array<LogModel>();
            for (let i = 0; i < paths.length; i++) {
                this.logFiles.push({ index: i, selected: false, path: paths[i] });
            }
        });
    }

    fileClicked(index: number) {
        this.logService.getLogFileContent(this.logFiles[index].path).subscribe(cont => {
            this.logFileContent = cont;
            for (var logFile of this.logFiles) {
                logFile.selected = false;
            }
            this.logFiles[index].selected = true;
        });
    }
}

class LogModel {
    index: number;
    selected: boolean;
    path: string;
}
