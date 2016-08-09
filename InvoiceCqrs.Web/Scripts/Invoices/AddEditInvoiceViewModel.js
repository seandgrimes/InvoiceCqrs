class LineItemViewModel {
    constructor(id, amount, description) {
        this.id = id;
        this.amount = ko.observable(amount);
        this.description = ko.observable(description);
    }
}

class AddEditInvoiceViewModel {
    constructor(invoice) {
        this.companies = ko.observableArray(invoice.companies);
        this.companyId = ko.observable(invoice.companyId);
        this.id = ko.observable(invoice.id);
        this.invoiceNumber = ko.observable(invoice.invoiceNumber);
        this.lineItems = ko.observableArray([]);

        invoice.lineItems.forEach(lineItem =>
            this.lineItems.push(new LineItemViewModel(lineItem.id, lineItem.amount, lineItem.description)));
    }

    addLineItem() {
        const lineItem = new LineItemViewModel(null, null, null);
        this.lineItems.push(lineItem);
    }

    removeLineItem(lineItem) {
        this.lineItems.remove(lineItem);
    }

    submit() {
        const data = {
            id: this.id(),
            companyId: this.companyId(),
            invoiceNumber: this.invoiceNumber(),
            lineItems: ko.toJS(this.lineItems())
        };

        const request = new XMLHttpRequest();

        request.onreadystatechange = function () {
            // There any other status codes that can be interpreted as success?
            if (request.readyState === XMLHttpRequest.DONE && request.status === 200) {
                const invoice = JSON.parse(request.responseText);
                window.location.href = `/Invoices/View/${invoice.Id}`;
            } else {
                // Need to show some type of error message here
            }
        }

        request.open("post", "/Invoices/Add", true);
        request.setRequestHeader("Content-Type", "application/json");
        request.send(JSON.stringify(data));
    }
}