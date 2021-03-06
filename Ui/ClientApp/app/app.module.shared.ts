import { NgModule } from "@angular/core";
import { RouterModule } from "@angular/router";
import { CommonModule } from "@angular/common";

import { AboutModule } from "./about/about.module";
import { AccountModule } from "./account/account.module";
import { ConfigurationModule } from "./configuration/configuration.module";
import { DownloadModule } from "./download/download.module";
import { ImportModule } from "./import/import.module";
import { ExportModule } from "./export/export.module";
import { SharedModule } from "./shared/shared.module";
import { PortfolioGroupsModule } from "./portfoliogroups/portfoliogroups.module"

import { AppComponent } from "./app.component";
import { HeaderComponent } from "./nav-menu.component";

export const sharedConfig: NgModule = {
    bootstrap: [AppComponent],
    declarations: [AppComponent, HeaderComponent],
    imports: [CommonModule, AboutModule, AccountModule, ConfigurationModule, DownloadModule, SharedModule, ImportModule, ExportModule, PortfolioGroupsModule,
        RouterModule.forRoot([
            { path: "", redirectTo: "accounts", pathMatch: "full" },
            { path: "**", redirectTo: "home" }
        ])
    ]
};
