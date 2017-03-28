import { NgModule } from "@angular/core";
import { RouterModule } from "@angular/router"
import { UniversalModule } from "angular2-universal";

import { LoadingComponent } from "./loading.component";
import { PaginationComponent } from "./pagination.component";
import { SplitPipe } from "./split.pipe";

@NgModule({
    declarations: [SplitPipe, LoadingComponent, PaginationComponent],
    imports: [UniversalModule, RouterModule],
    exports: [SplitPipe, LoadingComponent, PaginationComponent],
    providers: []
})
export class SharedModule { }
