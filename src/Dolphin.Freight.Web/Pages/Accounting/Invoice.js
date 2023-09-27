$(function () {
    $(document).ready(function () {
        var dateTimePicker = new CustomDateTimePicker();
        let Ids = ["InvoiceDto_PostDate", "InvoiceDto_InvoiceDate", "InvoiceDto_DueDate"];
        dateTimePicker.dateTimePicker(Ids, true);
    });
});

var TaxItems = [];
let globalInvoiceInstance;

dolphin.freight.settings.sysCodes.sysCode.getSysCodeDtosByType({ queryType: 'TaxId' }).done(function (res) {
    TaxItems = res.map(m => ({ id: m.id, value: m.codeValue + "-" + m.showName }));
})

class Invoice {
    constructor() {
        globalInvoiceInstance = this;
    }

    getPrice(id, price) {
        var html = "<input type='text' name='InvoiceBillDtos[" + id + "].Price' onchange='globalOnPriceChange(event)' value='" + price + "' id='Price_" + id + "' />"
        return html;
    }

    getTax(id, taxid) {
        var html = "<select name='InvoiceBillDtos[" + id + "].TaxId' onchange='globalOnTaxChange(event)' id='Tax_" + id + "'><option value='0'>Select Tax</option>";
        const set = new Set(TaxItems.map(item => JSON.stringify(item)));
        const uniqueValues = [...set];

        for (var i = 0; i < TaxItems.length; i++) {
            html += "<option value='" + TaxItems[i].id + "'"; 
            if (TaxItems[i].id == taxid) html += " selected";
            html += ">" + TaxItems[i].value + "</option>";

            const index = uniqueValues.indexOf(TaxItems[i].value);
            if (index > -1) {
                uniqueValues.splice(index, 1);
            }
        }
        return html;
    }

    onPriceChange(e) {
        let target = e.currentTarget || e.srcElement || e.target;
        var index = target.id.split('_')[1];
        let price = parseFloat($(target).val() || 0);
        let quantity = parseFloat($(target).parent().parent().children()[7].children[0].value || 0);
        let rate = parseFloat($(target).parent().parent().children()[8].children[0].value || 0);
        $(target).parent().next().next().children().text(parseFloat(quantity * rate * price).toFixed());
        var amount = quantity * price * rate;
        if ($('#Tax_0').find(':selected').text().split('%')[0] == '0') {
            var taxAmount = amount * 0;
        } else if ($('#Tax_0').find(':selected').text().split('%')[0] == '7') {
            var taxAmount = amount * 0.07;
        } else {
            var taxAmount = amount * 0;
        }
        setCurrencyRate1($('#Currency_' + index + '').val(), event);
        getTotalSubAmount();
        getTotalAmount(parseFloat(taxAmount).toFixed(2));
    }

    onTaxChange(e) {
        let target = e.currentTarget || e.srcElement || e.target;
        var index = target.id.split('_')[1];
        let price = $(target).parent().prev().children().val() || 0;
        let quantity = $(target).parent().prev().prev().prev().children().val() || 0;
        let rate = $(target).parent().prev().prev().children().val() || 0;
        var amount = quantity * price * rate;
        if ($(target).find(':selected').text().split('%')[0] == '7') {
            var taxAmount = 0.07 * amount;
        } else {
            var taxAmount = 0 * amount;
        }
        $(target).parent().next().children().text(parseFloat(quantity * rate * price).toFixed(2));
        setCurrencyRate1($('#Currency_' + index + '').val(), event);
        getTotalSubAmount();
        getTotalAmount(parseFloat(taxAmount).toFixed(2));
    }
}

function globalOnPriceChange(event) {
    if (globalInvoiceInstance) {
        globalInvoiceInstance.onPriceChange(event);
    }
}
function globalOnTaxChange(event) {
    if (globalInvoiceInstance) {
        globalInvoiceInstance.onTaxChange(event);
    }
}
