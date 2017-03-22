export class ApplicationConfiguration {
    keePassConfiguration: KeePassConfiguration;
    databaseConfiguration: DatabaseConfiguration;
    uiConfiguration: UiConfiguration;
}
export class KeePassConfiguration {
    path: string;
}
export class DatabaseConfiguration {
    databasePath: string;
}
export class UiConfiguration {
    language: string;
}