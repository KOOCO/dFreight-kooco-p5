var invDataTable;
var currencies;
var glcodes;
var exchangeRate;
var df = true;
var cf = true;

var paymentId = $('#Accounting_InvoiceList_PaymentId').val();

$(function () {
    currencies = [];
    dolphin.freight.common.ajaxDropdown.getSysCodeDtosByType({ queryType: 'Currency' }).done(function (result) {
        $.each(result, function (key, value) {
            currencies.push(value.codeValue)
        });  
    });

    glcodes = []
    dolphin.freight.accountingSettings.glCodes.glCode.getGlCodes({}).done(function (result) {
        $.each(result, function (key, value) {
            var code = value.code;
            var id = value.id;
            glcodes[id] = code;
        });

        load();
    });



    $(document).on('show.bs.modal', "[id^='altEditor-modal-']", function () {
        $("#invoiceDate").on("change", function (event) {
            if (df) {
                df = false;
            } else {
                full_cal();
            }
        });
        $("#currency").on("change", function (event) {
            if (cf) {
                cf = false;
            } else {
                full_cal();
            }
        });
    });

    $(document).on('hidden.bs.modal', "[id^='altEditor-modal-']", function () {
        df = true;
        cf = true;
    })

    $(document).on('click', "[id^='InvTable'] tbody ", 'tr', function () {
        var selectedData = invDataTable.rows({ selected: true }).data()[0];
        if (selectedData) {
            getExchangeRate(selectedData.invoiceDate, selectedData.currency, 'TWD').then(function (result) {
                exchangeRate = result;
            });
        }
    });

    $(document).on("keyup", "#paymentAmount", function (event) {
        cal();
    });

});

var load = function () {
    dolphin.freight.accounting.inv.inv.getList(paymentId)
        .done(function (result) {
            console.log(paymentId);
            console.log(result);
            var l = abp.localization.getResource('Freight');
            invDataTable = $('#InvTable').DataTable(
                abp.libs.datatables.normalizeConfiguration({
                    paging: false,
                    info: false,
                    searching: false,
                    scrollX: true,
                    select: 'single',
                    responsive: true,
                    data: result,
                    columnDefs: [
                        {
                            data: "id",
                            visible: false,
                            searchable: false,
                            type: "hidden"
                        },
                        {
                            title: l('InvoiceList:PostDate'),
                            data: "postDate",
                            type: "readonly"
                        },
                        {
                            title: l('InvoiceList:InvoiceDate'),
                            data: "invoiceDate",
                            datetimepicker: { timepicker: false, format: "Y-m-d" }
                        },
                        {
                            title: l('InvoiceList:DueDate'),
                            data: "dueDate",
                            type: "readonly"
                        },
                        {
                            title: l('InvoiceList:Type'),
                            data: "type",
                            type: "readonly"
                        },
                        {
                            title: l('InvoiceList:OfficeId'),
                            data: "officeId",
                            type: "readonly"
                        },
                        {
                            title: l('InvoiceList:CustomerId'),
                            data: "customerId",
                            type: "readonly"
                        },
                        {
                            title: l('InvoiceList:InvoiceNo'),
                            data: "invoiceNo"
                        },
                        {
                            title: l('InvoiceList:GlCodeId'),
                            data: "glCodeId",
                            render: function (data) {
                                if (!data)
                                    return "";
                                else {
                                    if (glcodes[data] !== undefined)
                                        return glcodes[data];
                                    else
                                        return "";
                                }
                            },
                            type: "select",
                            "options": glcodes
                        },
                        {
                            title: l('InvoiceList:Currency'),
                            data: "currency",
                            type: "select",
                            "options": currencies
                        },
                        {
                            title: l('InvoiceList:InvoiceAmount'),
                            data: "invoiceAmount",
                            type: "readonly"
                        },
                        {
                            title: l('InvoiceList:BalanceAmount'),
                            data: "balanceAmount",
                            type: "readonly"
                        },
                        {
                            title: l('InvoiceList:PaymentAmount'),
                            data: "paymentAmount",
                            type: "number"
                        },
                        {
                            title: l('InvoiceList:PaymentAmountTwd'),
                            data: "paymentAmountTwd",
                            type: "readonly"
                        },
                        {
                            title: l('InvoiceList:InvoiceDescription'),
                            data: "invoiceDescription"
                        },
                        {
                            title: l('InvoiceList:DocNo'),
                            data: "docNo",
                            type: "readonly"
                        },
                        {
                            title: l('InvoiceList:BlNo'),
                            data: "blNo",
                            type: "readonly"
                        },
                        {
                            title: l('InvoiceList:PoNo'),
                            data: "poNo",
                            type: "readonly"
                        },
                        {
                            title: l('InvoiceList:CsCode'),
                            data: "csCode",
                            type: "readonly"
                        },
                        {
                            title: l('InvoiceList:SalesCode'),
                            data: "salesCode",
                            type: "readonly"
                        }
                    ],
                    dom: 'Bfrtip',        // Needs button container
                    altEditor: true,     // Enable altEditor
                    buttons: [
                        {
                            text: l('Create'),
                            name: 'add'        // do not change name
                        },
                        {
                            extend: 'selected', // Bind to Selected row
                            text: l('Edit'),
                            name: 'edit'        // do not change name
                        },
                        {
                            extend: 'selected', // Bind to Selected row
                            text: l('Delete'),
                            name: 'delete'      // do not change name
                        }
                    ]
                })
            );

            $("InvTable_filter").find('[type=search]').on('keyup', function () {
                invDataTable.search(this.value).draw();
            });
        }
    );
}

const getExchangeRate = async (exchageDate, ccy1, ccy2) => {
    var result = ''
    if (ccy1 === ccy2)
        result = '1';
    else if (exchageDate && ccy1, ccy2) {
        result = await dolphin.freight.accounting.inv.inv.getExchangeRate(exchageDate, ccy1, ccy2)
    }
    else {
        result = '';
    }
    return result;
}

var full_cal = function () {
    var invoiceDate = $('#invoiceDate').val();
    var currency = $('#currency').val();
    getExchangeRate(invoiceDate, currency, 'TWD').then(function (result) {
        exchangeRate = result;
        console.log(exchangeRate);
        cal();
    });
}

var cal = function () {
    var paymentAmount = $('#paymentAmount').val();
    if (exchangeRate && paymentAmount) {
        var twd = (Math.round(parseFloat(paymentAmount) * parseFloat(exchangeRate))).toString() + '.00';
        $('#paymentAmountTwd').val(twd);
    }
    else
        $('#paymentAmountTwd').val('');
}

var updateInvoiceList = function () {
    dolphin.freight.accounting.inv.inv.updateList(paymentId, invDataTable.data().toArray())
}