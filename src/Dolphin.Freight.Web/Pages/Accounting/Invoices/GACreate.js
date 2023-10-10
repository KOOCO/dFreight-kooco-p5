class GACreate {
    static getCurrencySelect(id, currency) {
        var selectHtml = "<select name='InvoiceBillDtos[" + index + "].Currency' id='Currency_" + id + "' onchange='GACreateScripts.onCurrencyChange(event)'><option value='0'>Select Currency</option>";
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

    static getBillCodeSelect(id, billCode) {
        var selectHtml = "<select name='InvoiceBillDtos[" + index + "].Code' id='Code_" + id + "'>";
        for (var i = 0; i < BillCodeItems.length; i++) {
            var ovalue = BillCodeItems[i].code + ":" + BillCodeItems[i].billingName;
            selectHtml = selectHtml + "<option value='" + ovalue + "'";
            if (ovalue == billCode) selectHtml = selectHtml + " selected ";
            selectHtml = selectHtml + " > " + ovalue + "</option>"
        }
        selectHtml = selectHtml + "</select>";
        return selectHtml;
    }

    static getUnitSelect(id, unit) {
        var selectHtml = "<select name='InvoiceBillDtos[" + index + "].Unit' id='Unit_" + id + "'>";
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

var GACreateScripts = {
    onCurrencyChange(e) {
        debugger;
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

        this.setCurrencyRate1($(target).val(), event);
    },

    setCurrencyRate1(value, e) {
        let target = e.currentTarget || e.srcElement || e.target;
        var index = target.id.split('_')[1];
        let val = currencyCodeToName[value];
        let conversion = JSON.parse(currencyConvertor);
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
