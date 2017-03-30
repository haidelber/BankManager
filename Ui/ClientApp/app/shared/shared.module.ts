import { NgModule } from "@angular/core";
import { RouterModule } from "@angular/router"
import { UniversalModule } from "angular2-universal";

import { LoadingComponent } from "./loading.component";
import { PaginationComponent } from "./pagination.component";
import { SplitPipe } from "./split.pipe";
import { KeysPipe } from "./key.pipe";

@NgModule({
    declarations: [SplitPipe, KeysPipe, LoadingComponent, PaginationComponent],
    imports: [UniversalModule, RouterModule],
    exports: [SplitPipe, KeysPipe, LoadingComponent, PaginationComponent],
    providers: []
})
export class SharedModule { }
