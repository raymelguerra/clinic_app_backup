export class PagedResponse<T>{

    pageNumber: number;
    pageSize: number;
    firstPage: string;
    lastPage: string;
    totalPages: string;
    totalRecords: string;
    nextPage: string;
    previousPage: string;
    data: T;
    message: string;
    succeeded: boolean;
    errors: string;

    constructor(data: T, pageNumber: number, pageSize: number) {
        this.pageNumber = pageNumber;
        this.pageSize = pageSize;
        this.data = data;
        // this.data = data;
        // this.message = null;
        // this.succeeded = true;
        // this.errors = null;
    }
}
