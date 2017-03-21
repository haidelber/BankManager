"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
var core_1 = require("@angular/core");
var http_1 = require("@angular/http");
var SettingsComponent = (function () {
    function SettingsComponent(http) {
        var _this = this;
        http.get("/api/Configuration").subscribe(function (result) {
            _this.configuration = result.json();
            _this.configurationString = JSON.stringify(_this.configuration);
        }, function (error) { return _this.errorMessage = error; });
        http.get("/api/Configuration/ConfigurationFilePath").subscribe(function (result) {
            return _this.configurationFilePath = result.text();
        }, function (error) { return _this.errorMessage = error; });
    }
    return SettingsComponent;
}());
SettingsComponent = __decorate([
    core_1.Component({
        selector: "settings",
        template: require("./settings.component.html")
    }),
    __metadata("design:paramtypes", [http_1.Http])
], SettingsComponent);
exports.SettingsComponent = SettingsComponent;
var ApplicationConfiguration = (function () {
    function ApplicationConfiguration() {
    }
    return ApplicationConfiguration;
}());
var KeePassConfiguration = (function () {
    function KeePassConfiguration() {
    }
    return KeePassConfiguration;
}());
var DatabaseConfiguration = (function () {
    function DatabaseConfiguration() {
    }
    return DatabaseConfiguration;
}());
var UiConfiguration = (function () {
    function UiConfiguration() {
    }
    return UiConfiguration;
}());
//# sourceMappingURL=settings.component.js.map