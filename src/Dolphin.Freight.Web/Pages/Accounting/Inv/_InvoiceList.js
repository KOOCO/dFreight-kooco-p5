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



    //$(document).on('show.bs.modal', "[id^='altEditor-modal-']", function () {
    //    $("#invoiceDate").on("change", function (event) {
    //        if (df) {
    //            df = false;
    //        } else {
    //            full_cal();
    //        }
    //    });
    //    $("#currency").on("change", function (event) {
    //        if (cf) {
    //            cf = false;
    //        } else {
    //            full_cal();
    //        }
    //    });
    //});

    //$(document).on('hidden.bs.modal', "[id^='altEditor-modal-']", function () {
    //    df = true;
    //    cf = true;
    //})

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
                    altEditor: false,     // Enable altEditor
                    buttons: [
                        {
                            text: '<i class="fa fa-plus"></i>',
                            className: 'btn btn-outline-success',
                            name: 'add',// do not change name
                            action: function (e, dt, node, config) {
                                var blankData = {
                                    id: "",
                                    postDate: "",
                                    invoiceDate: "",
                                    dueDate: "",
                                    type: "",
                                    officeId: "",
                                    customerId: "",
                                    invoiceNo: "",
                                    glCodeId: "",
                                    currency: "",
                                    invoiceAmount: "",
                                    balanceAmount: "",
                                    paymentAmount: "",
                                    paymentAmountTwd: "",
                                    invoiceDescription: "",
                                    docNo: "",
                                    blNo: "",
                                    poNo: "",
                                    csCode: "",
                                    salesCode: ""
                                };

                                dt.row.add(blankData).draw();
                                dt.row(':last', { order: 'current' }).select();
                            }
                        },
                        {
                            extend: 'selected', // Bind to Selected row
                            text: '<i class="fa fa-pencil-alt"></i>',
                            className: 'btn btn-outline-secondary',
                            name: 'edit'        // do not change name
                        },
                        {
                            extend: 'selected', // Bind to Selected row
                            text: '<i class="fa fa-trash"></i>',
                            className: 'btn btn-outline-danger',
                            name: 'delete'      // do not change name
                        }
                    ],
                    createdRow: function (row, data, dataIndex) {
                        if (!data.id) {
                            $('td:eq(0)', row).html("<input type='hidden' id='id' name='id'><input class='form-control' type='text' pattern='.*' name='postDate' readonly='' id='postDate' title='' placeholder='' data-special='' data-unique='false' style='overflow: hidden; '>");
                            $('td:eq(1)', row).html("<input class='form-control' type='text'' pattern='.*' name='invoiceDate' id='invoiceDate' title='' placeholder='' data-special='' data-unique='false' style='overflow: hidden;'>");
                            $('td:eq(2)', row).html("<input class='form-control' type='readonly' pattern='.*' name='dueDate' readonly='' id='dueDate' title='' placeholder='' data-special='' data-unique='false' style='overflow: hidden; '>");
                            $('td:eq(3)', row).html("<input class='form-control' type='readonly' pattern='.*' name='type' readonly='' id='type' title='' placeholder='' data-special='' data-unique='false' style='overflow: hidden; '>");
                            $('td:eq(4)', row).html("<input class='form-control' type='readonly' pattern='.*' name='officeId' readonly='' id='officeId' title='' placeholder='' data-special='' data-unique='false' style='overflow: hidden; '>");
                            $('td:eq(5)', row).html("<input class='form-control' type='readonly' pattern='.*' name='customerId' readonly='' id='customerId' title='' placeholder='' data-special='' data-unique='false' style='overflow: hidden; '>");
                            $('td:eq(6)', row).html("<input class='form-control' type='text' pattern='.*' name='invoiceNo' id='invoiceNo' title='' placeholder='' data-special='' data-unique='false' style='overflow: hidden; '>");
                            $('td:eq(7)', row).html("<select class='form-control' name='glCodeId' id='glCodeI' placeholder='' data-special='' data-unique='false'><option value='1bc83dd9-dc0a-89e2-8512-3a0c7629f213'>10100</option><option value='3c912a9d-83d0-5a86-ade0-3a0c762abb3a'>10101</option><option value='65ad1f59-28a6-cc76-552b-3a0cac5bab51'>10120</option><option value='afa44b26-8089-a9eb-3a13-3a0cac5c28db'>40100</option><option value='129bf766-1ce6-3776-3301-3a0cac5d0b7f'>212305</option><option value='698fda7c-6449-2839-af58-3a0cbd54a010'>40101</option></select>");
                            $('td:eq(8)', row).html("<select class='form-control' name='currency' id='currency' placeholder='' data-special='' data-unique='false'><option value='RMB'>RMB</option><option value='USD'>USD</option><option value='19'>19</option><option value='18'>18</option><option value='TWD'>TWD</option><option value='HKD'>HKD</option></select>");
                            $('td:eq(9)', row).html("<input class='form-control' type='readonly' pattern='.*' name='invoiceAmount' readonly='' id='invoiceAmount' title='' placeholder='' data-special='' data-unique='false' style='overflow: hidden; '>");
                            $('td:eq(10)', row).html("<input class='form-control' type='readonly' pattern='.*' name='balanceAmount' readonly='' id='balanceAmount' title='' placeholder='' data-special='' data-unique='false' style='overflow: hidden; '>");
                            $('td:eq(11)', row).html("<input class='form-control' type='number' pattern='.*' name='paymentAmount' title='' id='paymentAmount' placeholder='' data-special='' data-unique='false' style='overflow: hidden; '>");
                            $('td:eq(12)', row).html("<input class='form-control' type='readonly' pattern='.*' name='paymentAmountTwd' readonly='' id='paymentAmountTwd' placeholder='' data-special='' data-unique='false' style='overflow: hidden; '>");
                            $('td:eq(13)', row).html("<input class='form-control' type='text' pattern='.*' name='invoiceDescription' title='' id='invoiceDescription' placeholder='' data-special='' data-unique='false' style='overflow: hidden; '>");
                            $('td:eq(14)', row).html("<input class='form-control' type='readonly' pattern='.*' name='docNo' readonly='' id='docNo' placeholder='' data-special='' data-unique='false' style='overflow: hidden; '>");
                            $('td:eq(15)', row).html("<input class='form-control' type='readonly' pattern='.*' name='blNo' readonly='' id='blNo' placeholder='' data-special='' data-unique='false' style='overflow: hidden; '>");
                            $('td:eq(16)', row).html("<input class='form-control' type='readonly' pattern='.*' name='poNo' readonly='' id='poNo' placeholder='' data-special='' data-unique='false' style='overflow: hidden; '>");
                            $('td:eq(17)', row).html("<input class='form-control' type='readonly' pattern='.*' name='csCode' readonly='' id='csCode' placeholder='' data-special='' data-unique='false' style='overflow: hidden; '>");
                            $('td:eq(18)', row).html("<input class='form-control' type='readonly' pattern='.*' name='salesCode' readonly='' id='salesCode' placeholder='' data-special='' data-unique='false' style='overflow: hidden; '>");
                        } else if (data.id != null) {
                            $('td:eq(0)', row).html("<input type='hidden' id='id' name='id' value='" + data.id + "'><input class='form-control' type='readonly' pattern='.*' name='postDate' value='" + (data.postDate ?? "") + "' readonly='' id='postDate' title='' placeholder='' data-special='' data-unique='false' style='overflow: hidden; '>");
                            $('td:eq(1)', row).html("<input class='form-control' type='readonly'' pattern='.*' name='invoiceDate' value='" + (data.invoiceDate ?? "") + "' id='invoiceDate' readonly='' placeholder='' data-special='' data-unique='false' style='overflow: hidden;'>");
                            $('td:eq(2)', row).html("<input class='form-control' type='readonly' pattern='.*' name='dueDate' value='" + (data.dueDate ?? "") + "' readonly='' id='dueDate' readonly='' placeholder='' data-special='' data-unique='false' style='overflow: hidden; '>");
                            $('td:eq(3)', row).html("<input class='form-control' type='readonly' pattern='.*' name='type' readonly='' value='" + (data.type ?? "") + "' id='type' title='' placeholder='' data-special='' data-unique='false' style='overflow: hidden; '>");
                            $('td:eq(4)', row).html("<input class='form-control' type='readonly' pattern='.*' name='officeId' value='" + (data.officeId ?? "") + "' readonly='' id='officeId' readonly='' placeholder='' data-special='' data-unique='false' style='overflow: hidden; '>");
                            $('td:eq(5)', row).html("<input class='form-control' type='readonly' pattern='.*' name='customerId' readonly='' value='" + (data.customerId ?? "") + "' id='customerId' readonly='' placeholder='' data-special='' data-unique='false' style='overflow: hidden; '>");
                            $('td:eq(6)', row).html("<input class='form-control' type='readonly' pattern='.*' name='invoiceNo' id='invoiceNo' value='" + data.invoiceNo + "' readonly='' placeholder='' data-special='' data-unique='false' style='overflow: hidden; '>");
                            $('td:eq(7)', row).html("<select class='form-control' type='readonly' name='glCodeId' value='" + (data.glCodeId ?? "") + "' id='glCodeI' readonly='' placeholder='' data-special='' data-unique='false'><option value='1bc83dd9-dc0a-89e2-8512-3a0c7629f213'>10100</option><option value='3c912a9d-83d0-5a86-ade0-3a0c762abb3a'>10101</option><option value='65ad1f59-28a6-cc76-552b-3a0cac5bab51'>10120</option><option value='afa44b26-8089-a9eb-3a13-3a0cac5c28db'>40100</option><option value='129bf766-1ce6-3776-3301-3a0cac5d0b7f'>212305</option><option value='698fda7c-6449-2839-af58-3a0cbd54a010'>40101</option></select>");
                            $('td:eq(8)', row).html("<select class='form-control' type='readonly' name='currency' id='currency' value='" + data.currency + "' placeholder='' readonly='' data-special='' data-unique='false'><option value='RMB'>RMB</option><option value='USD'>USD</option><option value='19'>19</option><option value='18'>18</option><option value='TWD'>TWD</option><option value='HKD'>HKD</option></select>");
                            $('td:eq(9)', row).html("<input class='form-control' type='readonly' pattern='.*' name='invoiceAmount' value='" + (data.invoiceAmount ?? "") + "' readonly='' id='invoiceAmount' title='' placeholder='' data-special='' data-unique='false' style='overflow: hidden; '>");
                            $('td:eq(10)', row).html("<input class='form-control' type='readonly' pattern='.*' name='balanceAmount' readonly='' value='" + (data.balanceAmount ?? "") + "' id='balanceAmount' title='' placeholder='' data-special='' data-unique='false' style='overflow: hidden; '>");
                            $('td:eq(11)', row).html("<input class='form-control' type='number' pattern='.*' name='paymentAmount' title='' id='paymentAmount' value='" + (data.paymentAmount ?? "") + "' placeholder='' data-special='' data-unique='false' style='overflow: hidden; '>");
                            $('td:eq(12)', row).html("<input class='form-control' type='readonly' pattern='.*' name='paymentAmountTwd' value='" + (data.paymentAmountTwd ?? "") + "' readonly='' id='paymentAmountTwd' placeholder='' data-special='' data-unique='false' style='overflow: hidden; '>");
                            $('td:eq(13)', row).html("<input class='form-control' type='text' pattern='.*' name='invoiceDescription' value='" + (data.invoiceDescription ?? "") + "' title='' id='invoiceDescription' placeholder='' data-special='' data-unique='false' style='overflow: hidden; '>");
                            $('td:eq(14)', row).html("<input class='form-control' type='readonly' pattern='.*' name='docNo' value='" + (data.docNo ?? "") + "' readonly='' id='docNo' placeholder='' data-special='' data-unique='false' style='overflow: hidden; '>");
                            $('td:eq(15)', row).html("<input class='form-control' type='readonly' pattern='.*' name='blNo' readonly='' value='" + (data.blNo ?? "") + "' id='blNo' placeholder='' data-special='' data-unique='false' style='overflow: hidden; '>");
                            $('td:eq(16)', row).html("<input class='form-control' type='readonly' pattern='.*' name='poNo' readonly='' value='" + (data.poNo ?? "") + "' id='poNo' placeholder='' data-special='' data-unique='false' style='overflow: hidden; '>");
                            $('td:eq(17)', row).html("<input class='form-control' type='readonly' pattern='.*' name='csCode' readonly='' value='" + (data.csCode ?? "") + "' id='csCode' placeholder='' data-special='' data-unique='false' style='overflow: hidden; '>");
                            $('td:eq(18)', row).html("<input class='form-control' type='readonly' pattern='.*' name='salesCode' readonly='' value='" + (data.salesCode ?? "") + "' id='salesCode' placeholder='' data-special='' data-unique='false' style='overflow: hidden; '>");
                        }
                    }
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