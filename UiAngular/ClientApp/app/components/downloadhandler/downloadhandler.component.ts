import { Component } from '@angular/core';
import { Http } from '@angular/http';

@Component({
    selector: 'downloadhandler',
    template: require('./downloadhandler.component.html')
})
export class DownloadHandlerComponent {
    public handlers: DownloadHandler[];

    constructor(http: Http) {
        http.get('/api/DownloadHandler/').subscribe(result => {
            this.handlers = result.json();
        });
    }
}

interface DownloadHandler {
    name: string;
    url: string;
    path: string;
    defaultSelected: boolean;
}
