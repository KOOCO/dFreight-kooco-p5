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