import { Component, Input } from '@angular/core';

@Component({
    selector: 'pagination',
    templateUrl: './pagination.component.html'
})
export class PaginationComponent {
    @Input()
    itemCount: number;
    @Input()
    link: string;
    @Input()
    pageSize: number;
    @Input()
    currentPage: number;

    previousPage: number;
    nextPage: number;

    firstPage = 1;
    lastPage: number;

    pages: number[];

    ngOnChanges() {
        if (!this.pageSize) {
            this.pageSize = 100;
        }
        this.pages = new Array<number>();
        this.currentPage = Number(this.currentPage);
        this.lastPage = Math.ceil(this.itemCount / this.pageSize);
        this.previousPage = this.currentPage >= 2 ? this.currentPage - 1 : this.firstPage;
        this.nextPage = this.currentPage < this.lastPage ? this.currentPage + 1 : this.lastPage;
        const pageOffsetBegin = Math.max(-this.currentPage + 1, -2) + this.currentPage;
        const pageOffsetEnd = Math.min(this.lastPage - this.currentPage, 2) + this.currentPage;
        for (var i = pageOffsetBegin; i <= pageOffsetEnd; i++) {
            this.pages.push(i);
        }
    }

    ngOnInit() {

    }
}
