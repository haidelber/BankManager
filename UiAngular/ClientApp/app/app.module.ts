import { NgModule } from "@angular/core";
import { RouterModule } from "@angular/router";
import { UniversalModule } from "angular2-universal";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { ChartsModule } from "ng2-charts";

import { AppComponent } from "./components/_app/app.component"
import { NavMenuComponent } from "./components/navmenu/navmenu.component";
import { BalanceOverviewComponent } from "./components/balanceoverview/balanceoverview.component";
import { AboutComponent } from "./components/about/about.component";
import { DownloadHandlerComponent } from "./components/downloadhandler/downloadhandler.component";
import { LogsComponent } from "./components/logs/logs.component";
import { SettingsComponent } from "./components/settings/settings.component";

@NgModule({
    bootstrap: [AppComponent],
    declarations: [
        AppComponent,
        NavMenuComponent,
        BalanceOverviewComponent,
        AboutComponent,
        DownloadHandlerComponent,
        LogsComponent,
        SettingsComponent
    ],
    imports: [
        UniversalModule, // Must be first import. This automatically imports BrowserModule, HttpModule, and JsonpModule too.
        FormsModule,
        ChartsModule,
        ReactiveFormsModule,
        RouterModule.forRoot([
            { path: "", redirectTo: "home", pathMatch: "full" },
            { path: "home", component: BalanceOverviewComponent },
            { path: "download", component: DownloadHandlerComponent },
            { path: "configuration", component: SettingsComponent },
            { path: "logs", component: LogsComponent },
            { path: "about", component: AboutComponent },
            { path: "**", redirectTo: "home" }
        ])
    ]
})
export class AppModule {
}
