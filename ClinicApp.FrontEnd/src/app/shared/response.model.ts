export class Response<T> {
    data: T;
    message: string;
    succeeded: boolean;
    errors: string;

    constructor(data: T) {
        this.data = data;
        this.message = null;
        this.succeeded = true;
        this.errors = null;
    }
}
