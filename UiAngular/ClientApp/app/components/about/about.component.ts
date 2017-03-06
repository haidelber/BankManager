import { Component } from '@angular/core';

@Component({
    selector: 'about',
    template: require('./about.component.html')
})
export class AboutComponent {
    public currentCount = 0;

    public incrementCounter() {
        this.currentCount++;
    }
}
