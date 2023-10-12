var l = abp.localization.getResource('Freight');
class GAListJS {
    static selectAllCheckbox(element) {
        var isChecked = $(element).prop('checked');
        $('#ListTable tbody input.selectCheckbox[type="checkbox"]').prop('checked', isChecked);

        if (isChecked) {
            $('#copyBtn').prop('disabled', !isChecked);
            $('#deleteBtn').prop('disabled', !isChecked);
        } else {
            $('#copyBtn').prop('disabled', !isChecked);
            $('#deleteBtn').prop('disabled', !isChecked);
        }
    }

    static selectCheckbox(checkbox) {
        var checkedCheckboxes = $('.selectCheckbox:checked');
        if (checkbox.checked) {
            checkedCheckboxes.each(function (index, checkbox1) {
                $('#deleteBtn').prop('disabled', false);

                if (checkedCheckboxes.length == 1) {
                    $('#copyBtn').prop('disabled', false);
                } else {
                    $('#copyBtn').prop('disabled', true);
                }
            });
        } else {
            var checkedCheckboxes = $('.selectCheckbox:checked');

            if (checkedCheckboxes && checkedCheckboxes.length == 0) {
                $('#deleteBtn').prop('disabled', true);
                $('#copyBtn').prop('disabled', true);
            } else if (checkedCheckboxes.length == 1) {
                $('#copyBtn').prop('disabled', false);
            }
        }
        if (!$(checkbox).prop('checked')) {
            $('#selectAllCheckbox').prop('checked', false);
        } else {
            var allChecked = true;
            $('#ListTable tbody input.selectCheckbox[type="checkbox"]').each(function () {
                if (!$(this).prop('checked')) {
                    allChecked = false;
                    return false;
                }
            });
            $('#selectAllCheckbox').prop('checked', allChecked);
        }
    }

    static deleteInvoices() {
        var ids = [];
        var selectedCheckboxes = $('#ListTable tbody input.selectCheckbox[type="checkbox"]:checked');
        selectedCheckboxes.each(function (i, checkbox) {
            var id = $(checkbox).attr('data-id');
            ids.push(id);
        });

        abp.message.confirm(l('DeleteConfirmationMessage')).then(function (confirmed) {
            if (confirmed) {
                dolphin.freight.accounting.invoices.invoice.deleteGAInvoicesById(ids).done(function () {
                    abp.message.success(l('Message:SuccessDelete'));
                    dataTable.ajax.reload();
                });
            }
        });
    }

    static copyInvoice() {
        var CheckedCheckBoxes = $('.selectCheckbox:checked');
        var InvoiceId = CheckedCheckBoxes.attr('data-id');

        dolphin.freight.accounting.invoices.invoice.copyGAInvoice(InvoiceId).done(function (res) {
            location.href = 'GACreate?InvoiceId=' + res.invoiceId + '&InvoiceType=' + res.invoiceType;
        });
    }
}

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
            searching: false,
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
                    title: '<input type="checkbox" id="selectAllCheckbox" disable="true" onclick="GAListJS.selectAllCheckbox(this)" style=" cursor: pointer;">',
                    data: null,
                    orderable: false,
                    render: function (data, type, row) {
                        var id = row.id;
                        $('#selectAllCheckbox').prop('checked', false);
                        return '<input type="checkbox" class="selectCheckbox" data-id="' + id + '" onclick="GAListJS.selectCheckbox(this)" style=" cursor: pointer;">';
                    }
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
                                        if (data.record.isLocked) {

                                        }
                                        location.href = 'GACreate?InvoiceId=' + data.record.id + '&InvoiceType=' + data.record.invoiceType;

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
                    data: "docNo"
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
                    data: "totalBeforeTax",
                    defaultContent: ""
                },
                {
                    title: l('TaxAmount'),
                    data: "totalTax",
                    defaultContent: ""
                },
                {
                    title: l('Amount'),
                    data: "totalAmount",
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