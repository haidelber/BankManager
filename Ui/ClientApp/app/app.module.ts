import { NgModule } from "@angular/core";
import { RouterModule } from "@angular/router";
import { UniversalModule } from "angular2-universal";

import { AboutModule } from "./about/about.module";
import { AccountModule } from "./account/account.module";
import { ConfigurationModule } from "./configuration/configuration.module";
import { DownloadModule } from "./download/download.module";
import { LogModule } from "./log/log.module";
import { SharedModule } from "./shared/shared.module";

import { AppComponent } from "./app.component";
import { HeaderComponent } from "./nav-menu.component";


@NgModule({
    bootstrap: [AppComponent],
    declarations: [AppComponent, HeaderComponent],
    imports: [
        UniversalModule, // Must be first import. This automatically imports BrowserModule, HttpModule, and JsonpModule too.
        AboutModule, AccountModule, ConfigurationModule, DownloadModule, LogModule, SharedModule,
        RouterModule.forRoot([
            { path: "", redirectTo: "accounts", pathMatch: "full" },
            { path: "**", redirectTo: "home" }
        ])
    ]
})
export class AppModule {
}
