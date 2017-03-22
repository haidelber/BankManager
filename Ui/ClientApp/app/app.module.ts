import { NgModule } from "@angular/core";
import { RouterModule } from "@angular/router";
import { UniversalModule } from "angular2-universal";

import { AboutModule } from "./about/about.module";
import { DownloadModule } from "./download/download.module";
import { ConfigurationModule } from "./configuration/configuration.module";
import { LogModule } from "./log/log.module";
import { SharedModule } from "./shared/shared.module";

import { AppComponent } from "./app.component";
import { HeaderComponent } from "./nav-menu.component";


@NgModule({
    bootstrap: [AppComponent],
    declarations: [AppComponent, HeaderComponent],
    imports: [
        UniversalModule, // Must be first import. This automatically imports BrowserModule, HttpModule, and JsonpModule too.
        AboutModule,
        DownloadModule,
        ConfigurationModule,
        LogModule,
        SharedModule,
        RouterModule.forRoot([
            { path: "", redirectTo: "overview", pathMatch: "full" },
            { path: "**", redirectTo: "home" }
        ])
    ]
})
export class AppModule {
}
