class GACreate {
    static getCurrencySelect(id, currency, value) {
        var selectHtml = "<select name='InvoiceBillDtos[" + index + "].Currency' id='Currency_" + id + "' onchange='GACreateScripts.onCurrencyChange(event)'><option value='0'>Select Currency</option>";
        currency = currency || value;
        const set = new Set(CurrencyItems.map(item => JSON.stringify(item)));
        const test = [...set].map(item => JSON.parse(item));
        for (var i = 0; i < test.length; i++) {
            selectHtml = selectHtml + "<option value='" + test[i] + "'";
            if (test[i] == currency) selectHtml = selectHtml + " selected ";
            selectHtml = selectHtml + " > " + test[i] + "</option>"
        }
        selectHtml = selectHtml + "</select>";
        return selectHtml;
    }

    static getBillCodeSelect(id, billCode, value) {
        var selectHtml = "<select name='InvoiceBillDtos[" + index + "].Code' id='Code_" + id + "'>";
        billCode = billCode || value;
        for (var i = 0; i < BillCodeItems.length; i++) {
            var ovalue = BillCodeItems[i].code + ":" + BillCodeItems[i].billingName;
            selectHtml = selectHtml + "<option value='" + ovalue + "'";
            if (ovalue == billCode) selectHtml = selectHtml + " selected ";
            selectHtml = selectHtml + " > " + ovalue + "</option>"
        }
        selectHtml = selectHtml + "</select>";
        return selectHtml;
    }

    static getUnitSelect(id, unit, value) {
        var selectHtml = "<select name='InvoiceBillDtos[" + index + "].Unit' id='Unit_" + id + "'>";
        unit = unit || value;
        for (var i = 0; i < UnitItems.length; i++) {
            selectHtml = selectHtml + "<option value='" + UnitItems[i].codeValue + "'";
            if (UnitItems[i].codeValue == unit) selectHtml = selectHtml + " selected ";
            selectHtml = selectHtml + " > " + UnitItems[i].showName + "</option>"
        }
        selectHtml = selectHtml + "</select>";
        return selectHtml;
    }

    static getBillTypeSelect(id, billType) {
        var selectHtml = "<select name='InvoiceBillDtos[" + index + "].BillType' id='BillType_" + id + "'>";
        for (var i = 0; i < BillTypeItems.length; i++) {
            selectHtml = selectHtml + "<option value='" + BillTypeItems[i].codeValue + "'";
            if (BillTypeItems[i].codeValue == billType) selectHtml = selectHtml + " selected ";
            selectHtml = selectHtml + " > " + BillTypeItems[i].showName + "</option>"
        }
        selectHtml = selectHtml + "</select>";
        return selectHtml;
    }

    static getPPOrCCSelect(id, pPOrCC) {
        var selectHtml = "<select name='InvoiceBillDtos[" + index + "].PPOrCC' id='PPOrCC_" + id + "'>";
        for (var i = 0; i < PPOrCCItems.length; i++) {
            selectHtml = selectHtml + "<option value='" + PPOrCCItems[i].codeValue + "'";
            if (PPOrCCItems[i].codeValue == pPOrCC) selectHtml = selectHtml + " selected ";
            selectHtml = selectHtml + " > " + PPOrCCItems[i].showName + "</option>"
        }
        selectHtml = selectHtml + "</select>";
        return selectHtml;
    }
}

function getTotalAmount() {
    var table = $('#trtbody');
    var total = 0;

    for (var i = 0; i < table[0].rows.length; i++) {
        var amountCell = document.getElementById("td_Amount_" + i);

        var amount = parseFloat(amountCell.innerHTML);

        total += amount;
    }
    var totalAmountElement = document.getElementById("totalAmount");
    totalAmountElement.innerHTML = parseFloat(total).toFixed(2);
}

function getTotalSubAmount() {
    var table = $('#trtbody');
    var total = 0;
    for (var i = 0; i < table[0].rows.length; i++) {
        var amountCell = document.getElementById("td_Amount_" + i);

        var amount = parseFloat(amountCell.innerHTML);

        total += amount;
    }
    var totalAmountElement = document.getElementById("convertedTWD");
    totalAmountElement.innerHTML = total.toFixed(2);
    let conversion = JSON.parse(currencyConverter);
    currencies.forEach(function (currency) {
        let key = 'TWD-' + currency + '';
        let convertedRate = conversion[key] || 1;

        document.getElementById("converted" + currency).innerHTML = (convertedRate * total).toFixed(2);
    })
}

var GACreateScripts = {
    onCurrencyChange(e) {
        let target = e.currentTarget || e.srcElement || e.target;
        let index = target.id.split('_')[1];
        let val = currencyCodeToName[$(target).val()];
        let conversion = JSON.parse(currencyConverter);
        let rate = conversion["" + val + "-TWD"] || 1;
        if (val === undefined) {
            $('#Rate_' + index).val(0);
            $('#Quantity_' + index).val(0);
        } else {
            $('#Rate_' + index).val(parseFloat(rate).toFixed(2));
        }

        //this.setCurrencyRate1($(target).val(), event);

        var quantity = $('#Quantity_' + index).val();

        $('#td_Amount_' + index).text(parseFloat(quantity * rate).toFixed(2));
        var amount = quantity * rate;
        if (document.getElementById('Amount_' + index + '').innerHTML == 'NaN') {
            document.getElementById('Amount_' + index + '').innerHTML = 0;
        }
        getTotalSubAmount();
        getTotalAmount();
    },

    onQuantityChange(e) {
        let target = e.currentTarget || e.srcElement || e.target;
        var index = target.id.split('_')[1];
        let quantity = parseFloat($(target).val() || 0);
        let rate = parseFloat($(target).parent().next().children().val() || 0);
        var amount = quantity * rate;
        $(target).parent().next().next().text(parseFloat(quantity * rate).toFixed(2));
        getTotalSubAmount();
        getTotalAmount();
    },

    setCurrencyRate1(value, e) {
        let target = e.currentTarget || e.srcElement || e.target;
        var index = target.id.split('_')[1];
        let val = currencyCodeToName[value];
        let conversion = JSON.parse(currencyConverter);
        let rate = conversion["" + val + "-TWD"];
        
        let quantity = $('#Quantity_' + index + '').val();
        if ($('#Currency_0').val() == 0) {
            amount = 0;
        }
        currencies.forEach(function (currency) {
            let key = '' + val + '-' + currency + '';
            let convertedRate = conversion[key] || 1;
        
            document.getElementById("converted" + currency).innerHTML = (convertedRate * quantity).toFixed(2);
        })
    }
}
