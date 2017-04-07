import { NgModule } from "@angular/core";
import { RouterModule } from "@angular/router"
import { UniversalModule } from "angular2-universal";

import { SharedModule } from "../shared/shared.module";

import { LogService } from "./log.service";
import { LogComponent } from "./log.component";


@NgModule({
    declarations: [LogComponent],
    imports: [UniversalModule, SharedModule, RouterModule.forChild([
        { path: "log", redirectTo: "log/0" },
        { path: "log/:index", component: LogComponent }])],
    exports: [RouterModule],
    providers: [LogService]
})
export class LogModule { }
