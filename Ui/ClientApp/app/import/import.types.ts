import {AccountType} from "../account/account.types";
export class ImportServiceRunModel {
    base64File: string;
    fileParserConfigurationKey: string;
    accountType: AccountType;
    accountId: number;
}