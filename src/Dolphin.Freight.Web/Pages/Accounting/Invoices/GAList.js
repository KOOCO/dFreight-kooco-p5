var cardIndex = 0;
$(document).ready(function () {

    var l = abp.localization.getResource('Freight');
    var queryAction = function () {
        return {
            QueryInvoiceType: 2
        };
    };
    var dataTable = $('#ListTable').DataTable(
        abp.libs.datatables.normalizeConfiguration({
            serverSide: true,
            paging: true,
            order: [[2, "asc"]],
            searching: true,
            scrollX: true,
            responsive: {
                details: {
                    type: 'column'
                }
            },
            autoWidth: false,
            ajax: abp.libs.datatables.createAjax(dolphin.freight.accounting.invoices.invoice.queryList, queryAction),
            columnDefs: [
                {
                    className: 'dtr-control',
                    orderable: false,
                    "defaultContent": ""
                },
                {
                    title: l('Actions'), 
                    rowAction: {
                        items:
                            [
                                {
                                    text: l('Edit'),
                                    visible: abp.auth.isGranted('Settings.ItNoRanges.Edit'), //CHECK for the PERMISSION
                                    action: function (data) {
                                        debugger;
                                        if (data.record.isLocked) {

                                        }
                                        location.href = 'EditModal?Id=' + data.record.id;

                                    }
                                },

                                {
                                    text: l('Delete'),
                                    visible: function (data) {

                                        return !data.isLocked && abp.auth.isGranted('Settings.ItNoRanges.Delete')
                                    },

                                    action: function (data) {
                                        if (!data.record.isLocked) {
                                            abp.message.confirm(l('DeletionConfirmationMessage'))
                                                .then(function (confirmed) {
                                                    if (confirmed) {
                                                        dolphin.freight.importExport.oceanExports.oceanExportMbl
                                                            .delete(data.record.id)
                                                            .then(function () {
                                                                abp.message.success(l('SuccessfullyDeleted'));
                                                                dataTable.ajax.reload();
                                                            });
                                                    }
                                                });

                                        } else {
                                            abp.message.warn("鎖定不能刪除")
                                        }

                                        /*
                                        if (!data.record.isLocked) {
                                            dolphin.freight.importExport.oceanExports.oceanExportMbl
                                                .delete(data.record.id)
                                                .then(function () {
                                                    abp.message.success(l('SuccessfullyDeleted'));
                                                    dataTable.ajax.reload();
                                                });
                                        } else {
                                            abp.message.warn("鎖定不能刪除")
                                        }*/

                                    }
                                }
                            ]
                    }
                },
                {
                    //文件號碼
                    title: l('FilingNo'),
                    data: "filingNo"
                },
                {
                    title: l('Party'),
                    data: "shipToName"
                },
                {
                    data: null, title: l('TheReferenceNo'),
                    render: function (data, type, row) {
                        var hid = row.hblId;
                        if (hid == null || hid == "null") hid = "";
                        return '<a href="/Accounting/Invoice?MethodType=2&InvoiceType=' + row.invoiceType + '&Mid=' + row.mblId + '&Hid=' + hid + '&InvoiceId=' + row.id + '" >' + row.invoiceNo + '</a> ';
                    }
                },
                {
                    title: l('AmountBeforeTax'),
                    data: "",
                    defaultContent: ""
                },
                {
                    title: l('TaxAmount'),
                    data: "",
                    defaultContent: ""
                },
                {
                    title: l('Amount'),
                    data: "",
                    defaultContent: ""
                },
                {
                    title: l('AidAmount'),
                    data: "",
                    defaultContent: ""
                },
                {
                    title: l('OverDue'),
                    data: "",
                    defaultContent: ""
                },
                {
                    //類別
                    data: null, title: l('Type'),
                    render: function (data, type, row) {
                        switch (row.invoiceType) {
                            default:
                                return "";
                            case 0:
                                return "應收";
                            case 1:
                                return "應收/應付";
                            case 2:
                                return "應付";

                        }
                    }
                },
                {
                    //發布日期
                    title: l('PostDate'),
                    data: "postDate"
                },
                {
                    //發票日期
                    title: l('InvoiceDate'),
                    data: "invoiceDate"
                },
                {
                    //到期日期
                    title: l('DueDate'),
                    data: "dueDate"
                },
                {
                    //結餘金額
                    title: l('Balance'),
                    data: "balanceAmount"
                },
                {
                    //最後付款日期
                    title: l('LastDate'),
                    data: "lastDate"
                },
                {
                    //分站
                    title: l('OfficeId'),
                    data: "officeName"
                },
                {
                    title: l('IssuedBy'),
                    data: "issuedBy"
                }
            ]
        })
    );

});