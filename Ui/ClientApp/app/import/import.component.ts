import { Component } from "@angular/core";
import { Router } from "@angular/router";
import { FileUploader } from "ng2-file-upload";

import { AccountService } from "../account/account.service";
import { AccountType } from "../account/account.types";
import { ImportService } from "./import.service";
import { ImportServiceRunModel } from "./import.types";
import { BankAccountModel, CreditCardAccountModel, PortfolioModel } from "../account/account.types";


@Component({
    selector: "download",
    templateUrl: "./import.component.html"
})
export class ImportComponent {
    model: ImportServiceRunModel;
    accountTypeKey = 0;
    file: File;
    accountTypes = AccountType;

    uploader = new FileUploader({});

    fileParserConfigurations: string[];
    bankAccounts: BankAccountModel[];
    creditCardAccounts: CreditCardAccountModel[];
    portfolioAccounts: PortfolioModel[];

    constructor(private importService: ImportService, private accountService: AccountService, private router: Router) {
        this.model = new ImportServiceRunModel();
    }

    ngOnInit() {
        this.importService.getFileParserConfiguration().subscribe(a => this.fileParserConfigurations = a);
        this.accountService.getBankAccounts().subscribe(accounts => {
            this.bankAccounts = accounts;
        });
        this.accountService.getCreditCardAccounts().subscribe(accounts => {
            this.creditCardAccounts = accounts;
        });
        this.accountService.getPortfolioAccounts().subscribe(accounts => {
            this.portfolioAccounts = accounts;
        });
    }

    onFileChange(e) {
        const fileList = e.srcElement.files as FileList;
        if (fileList.length > 0) {
            this.file = fileList[0];
        } else {
            this.file = undefined;
        }
    }

    onSubmit() {
        const reader = new FileReader();
        reader.readAsBinaryString(this.file);
        reader.onload = () => {
            const base64 = btoa(reader.result);
            this.model.accountType = this.accountTypeKey;
            this.model.base64File = base64;
            console.log(this.model);
            this.importService.startImport(this.model).subscribe(() => this.router.navigateByUrl("/"));
        };
    }

    onCancel() {
        this.router.navigateByUrl("/");
    }
}

