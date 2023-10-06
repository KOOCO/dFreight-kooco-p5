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


class AirExportMawb {
    static KgToCbmConversion(Elem, Unit) {
        var SellingRate;
        if ($('#MawbModel_SellingRate').val() == undefined) {
            if ($('#AirExportMawbDto_SellingRate').val() != undefined) {
                SellingRate = $('#AirExportMawbDto_SellingRate').val();
            } else {
                SellingRate = 0;
            }
        } else {
            SellingRate = $('#MawbModel_SellingRate').val();
        }
        switch (Unit) {
            case 'KG':
                var CBM = parseFloat(Elem.currentTarget.value * 0.006).toFixed(2);
                $($(Elem.currentTarget).parent().next().next().children()[0]).val(CBM);
                if (Number(Elem.currentTarget.value) >= Number($('#GrossWeightKg').val())) {
                    $('#ChargeableWeightKg').val(Elem.currentTarget.value);
                    $('#AwbChargeableWeightKg').val(Elem.currentTarget.value);
                    $('#ChargeableWeightLb').val(parseFloat(Elem.currentTarget.value * 2.2).toFixed(2));
                    $('#AwbChargeableWeightLb').val(parseFloat(Elem.currentTarget.value * 2.2).toFixed(2));
                    if ($('#MawbModel_ChargeableWeightAmount').length == 0) {
                        $('#AirExportMawbDto_ChargeableWeightAmount').val(parseFloat(Elem.currentTarget.value * SellingRate).toFixed(2));
                        $('#AirExportMawbDto_AwbChargeableWeightAmount').val(parseFloat(Elem.currentTarget.value * SellingRate).toFixed(2));
                    } else {
                        $('#MawbModel_ChargeableWeightAmount').val(parseFloat(Elem.currentTarget.value * SellingRate).toFixed(2));
                        $('#MawbModel_AwbChargeableWeightAmount').val(parseFloat(Elem.currentTarget.value * SellingRate).toFixed(2));
                    }
                } else {
                    $('#ChargeableWeightKg').val($('#GrossWeightKg').val());
                    $('#AwbChargeableWeightKg').val(Elem.currentTarget.value);
                    if ($('#MawbModel_ChargeableWeightAmount').length == 0) {
                        $('#AirExportMawbDto_ChargeableWeightAmount').val(parseFloat($('#GrossWeightKg').val() * SellingRate).toFixed(2));
                        $('#AirExportMawbDto_AwbChargeableWeightAmount').val(parseFloat(Elem.currentTarget.value * SellingRate).toFixed(2));
                    } else {
                        $('#MawbModel_ChargeableWeightAmount').val(parseFloat($('#GrossWeightKg').val() * SellingRate).toFixed(2));
                        $('#MawbModel_AwbChargeableWeightAmount').val(parseFloat(Elem.currentTarget.value * SellingRate).toFixed(2));
                    }
                }
                break;
            case 'CBM':
                var KG = parseFloat(Elem.currentTarget.value * 166.67).toFixed(2);
                $($(Elem.currentTarget).parent().prev().prev().children()[0]).val(KG);
                if (Number(KG) >= Number($('#GrossWeightKg').val())) {
                    $('#ChargeableWeightKg').val(KG);
                    if ($('#MawbModel_ChargeableWeightAmount').length == 0) {
                        $('#AirExportMawbDto_ChargeableWeightAmount').val(parseFloat(KG * SellingRate).toFixed(2));
                        $('#AirExportMawbDto_AwbChargeableWeightAmount').val(parseFloat(KG * SellingRate).toFixed(2));
                    } else {
                        $('#MawbModel_ChargeableWeightAmount').val(parseFloat(KG * SellingRate).toFixed(2));
                        $('#MawbModel_AwbChargeableWeightAmount').val(parseFloat(KG * SellingRate).toFixed(2));
                    }
                } else {
                    $('#ChargeableWeightKg').val($('#GrossWeightKg').val());
                    if ($('#MawbModel_ChargeableWeightAmount').length == 0) {
                        $('#AirExportMawbDto_ChargeableWeightAmount').val(parseFloat($('#GrossWeightKg').val() * SellingRate).toFixed(2));
                    } else {
                        $('#MawbModel_ChargeableWeightAmount').val(parseFloat($('#GrossWeightKg').val() * SellingRate).toFixed(2));
                    }
                }
                break;
        }
    }

    static GrossWeightConversion(Elem, Unit) {
        var VolumeWeightKg;
        var SellingRate;
        if ($('#MawbModel_VolumeWeightKg').val() == undefined) {
            if ($('#AirExportMawbDto_VolumeWeightKg').val() != undefined) {
                VolumeWeightKg = $('#AirExportMawbDto_VolumeWeightKg').val();
            } else {
                VolumeWeightKg = 0;
            }
        } else {
            VolumeWeightKg = $('#MawbModel_VolumeWeightKg').val();
        }
        if ($('#MawbModel_SellingRate').val() == undefined) {
            if ($('#AirExportMawbDto_SellingRate').val() != undefined) {
                SellingRate = $('#AirExportMawbDto_SellingRate').val();
            } else {
                SellingRate = 0;
            }
        } else {
            SellingRate = $('#MawbModel_SellingRate').val();
        }
        switch (Unit) {
            case 'KG':
                if (Number(Elem.currentTarget.value) > Number(VolumeWeightKg)) {
                    $('#ChargeableWeightKg').val(Elem.currentTarget.value);
                    $('#ChargeableWeightLb').val(parseFloat(Elem.currentTarget.value * 2.2).toFixed(2));
                    if ($('#MawbModel_ChargeableWeightAmount').length == 0) {
                        $('#AirExportMawbDto_ChargeableWeightAmount').val(parseFloat(Elem.currentTarget.value * SellingRate).toFixed(2));
                    } else {
                        $('#MawbModel_ChargeableWeightAmount').val(parseFloat(Elem.currentTarget.value * SellingRate).toFixed(2));
                    }
                } else if (Number(Elem.currentTarget.value) <= Number(VolumeWeightKg)) {
                    $('#ChargeableWeightKg').val(VolumeWeightKg);
                    if ($('#MawbModel_ChargeableWeightAmount').length == 0) {
                        $('#AirExportMawbDto_ChargeableWeightAmount').val(parseFloat(VolumeWeightKg * SellingRate).toFixed(2));
                    } else {
                        $('#MawbModel_ChargeableWeightAmount').val(parseFloat(VolumeWeightKg * SellingRate).toFixed(2));
                    }
                }
                break;
            case 'LB':
                var KG = parseFloat(Elem.currentTarget.value / 2.2).toFixed(2);
                if (Number(KG) > Number(VolumeWeightKg)) {
                    $('#ChargeableWeightKg').val(KG);
                    if ($('#MawbModel_ChargeableWeightAmount').length == 0) {
                        $('#AirExportMawbDto_ChargeableWeightAmount').val(parseFloat(KG * SellingRate).toFixed(2));
                    } else {
                        $('#MawbModel_ChargeableWeightAmount').val(parseFloat(KG * SellingRate).toFixed(2));
                    }
                } else if (Number(KG) <= Number(VolumeWeightKg)) {
                    $('#ChargeableWeightKg').val(VolumeWeightKg);
                    if ($('#MawbModel_ChargeableWeightAmount').length == 0) {
                        $('#AirExportMawbDto_ChargeableWeightAmount').val(parseFloat(VolumeWeightKg * SellingRate).toFixed(2));
                    } else {
                        $('#MawbModel_ChargeableWeightAmount').val(parseFloat(VolumeWeightKg * SellingRate).toFixed(2));
                    }
                }
                break;
        }
    }
}

