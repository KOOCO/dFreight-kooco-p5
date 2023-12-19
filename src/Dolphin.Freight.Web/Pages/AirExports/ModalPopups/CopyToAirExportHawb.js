$(document).ready(function () {
    var accountingCheckbox = $('#accountingCheckbox');
    var apCheckbox = $('#apCheckbox');
    var arCheckbox = $('#arCheckbox');
    var dcCheckbox = $('#dcCheckbox');
    
    apCheckbox.prop('disabled', false);
    arCheckbox.prop('disabled', false);
    dcCheckbox.prop('disabled', false);

    var AirExportHawbCopy = {
        updateAccountingCheckboxState() {
            var apChecked = apCheckbox.is(':checked');
            var arChecked = arCheckbox.is(':checked');
            var dcChecked = dcCheckbox.is(':checked');

            accountingCheckbox.prop('checked', apChecked || arChecked || dcChecked);

            apCheckbox.prop('disabled', !accountingCheckbox.is(':checked'));
            arCheckbox.prop('disabled', !accountingCheckbox.is(':checked'));
            dcCheckbox.prop('disabled', !accountingCheckbox.is(':checked'));
        }
    }
    
    accountingCheckbox.change(function () {
        var isChecked = accountingCheckbox.is(':checked');
        
        apCheckbox.prop('checked', isChecked);
        arCheckbox.prop('checked', isChecked);
        dcCheckbox.prop('checked', isChecked);
        
        apCheckbox.prop('disabled', !isChecked);
        arCheckbox.prop('disabled', !isChecked);
        dcCheckbox.prop('disabled', !isChecked);
    });
    
    apCheckbox.change(function () {
        AirExportHawbCopy.updateAccountingCheckboxState();
    });
    arCheckbox.change(function () {
        AirExportHawbCopy.updateAccountingCheckboxState();
    });
    dcCheckbox.change(function () {
        AirExportHawbCopy.updateAccountingCheckboxState();
    });
});

$('#copyForm').on('abp-ajax-success', function (result, rs) {
    var url = new URL(window.location.href);

    location.href = url + '&ResultedMawbId=' + rs.responseText.MawbId + '&copyHawbMessage=True';
});