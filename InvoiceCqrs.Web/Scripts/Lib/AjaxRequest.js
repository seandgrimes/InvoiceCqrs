class AjaxRequest {
    constructor(method, url, contentType) {
        this.method = method;
        this.url = url;
        this.contentType = contentType || "application/json";
    }

    submit(data) {
        return new Promise((resolve, reject) => {
            const request = new XMLHttpRequest();
            request.open(this.method, this.url);
            request.setRequestHeader("Content-Type", this.contentType);

            const failure = function() {
                
            }
        });
    }
}