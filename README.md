# README Bank Manager

This is the next evolution step of my former [Bank Data Downloader](https://github.com/haidelber/BankDataDownloader). This project offers more features and is designed for extensibility.

### Upcoming features

* As there is no feedback from the Backend yet i plan to include something like SignalR to push notifications to the UI.
	* Ideally this is done in a completely decoupled way (NLog 2 SignalR Appender)
* Excel Export
* (Setup + making the application portable)

### Used Technology

* Google Chrome + Selenium Webdriver for dowloading data from the different banks
* KeePass for providing passwords
* Angular 2+ via Webpack for providing a nice UI
* .Net Core for Hosting the application
	* As the KeePass lib is not supported in Core the project is still compiled to .Net Framework instead of .Net Standard

### Included Banks ###

* [DKB](https://www.dkb.de)
* [Flatex](https://www.flatex.at)
* [Number26](https://n26.com)
* [PayPal](https://www.paypal.com)
* [Raiffeisen Österreich](http://www.raiffeisen.at)
* [Renault Bank direkt](https://www.renault-bank-direkt.at)
* [Santander Consumer Bank](https://www.santanderconsumer.at) needs adaption

## Projects

* Common
    * provides configuration types, configuration export converters, some helpers and extensions
* Data
	* contains everything regarding Entity Framework + Migration, Entities, Repositories
* Core
    * contains the core Services of this application, the download handlers, file parsers, and default configurations
* Test
    * contains test, they are aweful, mostly integration tests
* Ui
    * a .Net Core Console application firing up Kestrel to host a MVC application providing a Web API, including Swagger (UI), and the Angular SPA

## Selenium

From time to time you have to update the Chrome driver as older versions are not supported any more by Google Chrome.
Do this by updating the NuGet dependency version. 

## Testing

1. Use the app.changeme.config in the Test project.
1. Rename it to app.config and fill the settings with the according values.
	* This file is ignored by git because it contains sensitive data as we can't operate on mocks of the banks websites.
1. Generate the testdata necessary and adapt the app.config file.
1. Please make sure to not commit this file afterwards as it also contains a KeePass password.
    * Best practice is to use a KeePass file not containing anything apart from the entries necessary.
    * Use a generated master password for this KeePass database.

## Developing

I set up [Autofac](https://autofac.org/) + Entity Framework with some conventions allowing registration of components without needing to register them explicitly. These conventions include: 

* Classes ending with "Service" are registered as their implemented interface in default scope.
* Classes ending with "Provider" behave in the same way.

* All subtypes of PortfolioEntity, PositionEntity, AccountEntity, TransactionEntity are automatically added to Entity Framework DbContext.
* There is a generic IRepository for every Entity based on EntityBase.