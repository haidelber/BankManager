import { NgModule } from "@angular/core";

import { LoadingComponent } from "./loading.component";

import { SplitPipe } from "./split.pipe";

@NgModule({
    declarations: [SplitPipe, LoadingComponent],
    imports: [],
    exports: [SplitPipe, LoadingComponent],
    providers: []
})
export class SharedModule { }
