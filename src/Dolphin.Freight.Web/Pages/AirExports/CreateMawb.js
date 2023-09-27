$(function () {

    $(document).ready(function () {
        const Ids = [
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

        const DefaultTimeIds = [
            "MawbModel_RouteTrans1ArrivalDate",
            "MawbModel_RouteTrans2ArrivalDate",
            "MawbModel_RouteTrans3ArrivalDate",
            "MawbModel_RouteTrans1DepatureDate",
            "MawbModel_RouteTrans2DepatureDate",
            "MawbModel_RouteTrans3DepatureDate",
            "AirExportMawbDto_RouteTrans1ArrivalDate",
            "AirExportMawbDto_RouteTrans2ArrivalDate",
            "AirExportMawbDto_RouteTrans3ArrivalDate",
            "AirExportMawbDto_RouteTrans1DepatureDate",
            "AirExportMawbDto_RouteTrans2DepatureDate",
            "AirExportMawbDto_RouteTrans3DepatureDate"
        ];

        var dateTimePicker = new CustomDateTimePicker();
        dateTimePicker.dateTimePicker(Ids, false, DefaultTimeIds);
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
