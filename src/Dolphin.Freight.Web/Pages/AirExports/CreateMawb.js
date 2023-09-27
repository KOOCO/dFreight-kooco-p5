$(function () {

    $(document).ready(function () {
        const ids = [
            "MawbModel_AwbDate",
            "MawbModel_DepatureDate",
            "MawbModel_ArrivalDate",
            "MawbModel_RouteTrans1ArrivalDate",
            "MawbModel_RouteTrans2ArrivalDate",
            "MawbModel_RouteTrans3ArrivalDate",
            "MawbModel_RouteTrans1DepatureDate",
            "MawbModel_RouteTrans2DepatureDate",
            "MawbModel_RouteTrans3DepatureDate",
            "AwbCancelledDate",
            "AirExportMawbDto_AwbDate",
            "AirExportMawbDto_PostDate",
            "AirExportMawbDto_DepatureDate",
            "AirExportMawbDto_ArrivalDate",
            "AirExportMawbDto_RouteTrans1ArrivalDate",
            "AirExportMawbDto_RouteTrans2ArrivalDate",
            "AirExportMawbDto_RouteTrans3ArrivalDate",
            "AirExportMawbDto_RouteTrans1DepatureDate",
            "AirExportMawbDto_RouteTrans2DepatureDate",
            "AirExportMawbDto_RouteTrans3DepatureDate"
        ];

        ids.forEach(function (id) {
            let dateElem = $('#' + id);

            if (dateElem.length === 0) {
                return;
            }

            dateElem.removeAttr('type').datetimepicker({
                format: 'Y-m-d H:i',
                step: 15,
                allowInput: false
            });

            let currentVal = dateElem.val();
            if (currentVal.includes('T')) {
                dateElem.val(currentVal.replace('T', ' '));
            }
        });
    });

    $("#saveBtn").click(function () {
        $('#createForm').valid();
        var validate = $('#createForm').validate();
        var mawbErrors = validate.errorList.filter((error) => error.element.name.includes('MawbModel')); 
        if (mawbErrors.length && $('#btnMawbCollapse').hasClass('collapsed')) {
            $('#btnMawbCollapse').click();
            setTimeout(() => {
                $('#createForm').submit();
            }, 500)
        }
        else {
            $('#createForm').submit();
        }
    })
});


$('#createForm').on('abp-ajax-success', function (result, rs) {
    location.href = 'EditModal?ShowMsg=true&Id=' + rs.responseText.id
});
