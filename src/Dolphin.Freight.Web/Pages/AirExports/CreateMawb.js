
$(document).on('input', 'input[id^="length_"], input[id^="width_"], input[id^="height_"], input[id^="pcs_"]', function () {
    const row = $(this).closest('tr');
    calculateWeights(row);
});
$(document).on('input', 'input', function () {
    updateTotals();
});
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
            "AirExportMawbDto_RouteTrans3DepatureDate",
            "MawbModel_RouteDepartureArrivalDate",
           
            "MawbModel_RouteDepatureDate",
            
            "MawbModel_RouteDestinationArrivalDate",
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

$(document).on('click', '#applyDimensions', function () {
    applyPopupValues();
});

var extraProperties;

$('#createForm').on('abp-ajax-success', function (result, rs) {
    location.href = 'EditModal?ShowMsg=true&Id=' + rs.responseText.id
});

let rowCount = 0;
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
                    $('#ChargeableWeightLb').val(parseFloat(Elem.currentTarget.value * 2.20462).toFixed(2));
                    $('#AwbChargeableWeightLb').val(parseFloat(Elem.currentTarget.value * 2.20462).toFixed(2));
                    if ($('#MawbModel_ChargeableWeightAmount').length == 0) {
                        if ($('#AirExportMawbDto_SellingRateUnitType').val() == 1) {
                            $('#AirExportMawbDto_ChargeableWeightAmount').val(parseFloat(Elem.currentTarget.value * SellingRate).toFixed(2));
                            $('#AirExportMawbDto_AwbChargeableWeightAmount').val(parseFloat(Elem.currentTarget.value * SellingRate).toFixed(2));
                        }
                        else if ($('#AirExportMawbDto_SellingRateUnitType').val() == 2) {
                            $('#AirExportMawbDto_ChargeableWeightAmount').val(parseFloat(Elem.currentTarget.value * 2.20462 * SellingRate).toFixed(2));
                            $('#AirExportMawbDto_AwbChargeableWeightAmount').val(parseFloat(Elem.currentTarget.value * 2.20462 * SellingRate).toFixed(2));
                        } else {
                            $('#AirExportMawbDto_ChargeableWeightAmount').val(0);
                            $('#AirExportMawbDto_AwbChargeableWeightAmount').val(0);
                        }
                    } else {
                        if ($('#MawbModel_SellingRateUnit').val() == 1) {
                            $('#MawbModel_ChargeableWeightAmount').val(parseFloat(Elem.currentTarget.value * SellingRate).toFixed(2));
                            $('#MawbModel_AwbChargeableWeightAmount').val(parseFloat(Elem.currentTarget.value * SellingRate).toFixed(2));
                        } else if ($('#MawbModel_SellingRateUnit').val() == 2) {
                            $('#MawbModel_ChargeableWeightAmount').val(parseFloat(Elem.currentTarget.value * 2.20462 * SellingRate).toFixed(2));
                            $('#MawbModel_AwbChargeableWeightAmount').val(parseFloat(Elem.currentTarget.value * 2.20462 * SellingRate).toFixed(2));
                        } else {
                            $('#MawbModel_ChargeableWeightAmount').val(0);
                            $('#MawbModel_AwbChargeableWeightAmount').val(0);
                        }
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
                    $('#ChargeableWeightLb').val(parseFloat(KG * 2.20462).toFixed(2));
                    $('#AwbChargeableWeightKg').val(KG);
                    $('#AwbChargeableWeightLb').val(parseFloat(KG * 2.20462).toFixed(2));
                    if ($('#MawbModel_ChargeableWeightAmount').length == 0) {
                        if ($('#AirExportMawbDto_SellingRateUnitType').val() == 1) {
                            $('#AirExportMawbDto_ChargeableWeightAmount').val(parseFloat(KG * SellingRate).toFixed(2));
                            $('#AirExportMawbDto_AwbChargeableWeightAmount').val(parseFloat(KG * SellingRate).toFixed(2));
                        } else if ($('#AirExportMawbDto_SellingRateUnitType').val() == 2) {
                            $('#AirExportMawbDto_ChargeableWeightAmount').val(parseFloat(KG * 2.2046 * SellingRate).toFixed(2));
                            $('#AirExportMawbDto_AwbChargeableWeightAmount').val(parseFloat(KG * 2.2046 * SellingRate).toFixed(2));
                        } else {
                            $('#AirExportMawbDto_ChargeableWeightAmount').val(0);
                            $('#AirExportMawbDto_AwbChargeableWeightAmount').val(0);
                        }
                    } else {
                        if ($('#MawbModel_SellingRateUnit').val() == 1) {
                            $('#MawbModel_ChargeableWeightAmount').val(parseFloat(KG * SellingRate).toFixed(2));
                            $('#MawbModel_AwbChargeableWeightAmount').val(parseFloat(KG * SellingRate).toFixed(2));
                        } else if ($('#MawbModel_SellingRateUnit').val() == 2) {
                            $('#MawbModel_ChargeableWeightAmount').val(parseFloat(KG * 2.2046 * SellingRate).toFixed(2));
                            $('#MawbModel_AwbChargeableWeightAmount').val(parseFloat(KG * 2.2046 * SellingRate).toFixed(2));
                        }
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
        if ($('#MawbModel_SellingRateUnit').val() == undefined) {
            if ($('#AirExportMawbDto_SellingRate').val() != undefined) {
                SellingRate = $('#AirExportMawbDto_SellingRate').val();
            } else {
                SellingRate = 0;
            }
        } else {
            SellingRate = $('#MawbModel_SellingRateUnit').val();
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

    static openPopUp(situation) {
        switch (situation) {
            case 'withId':
                var url = new URL(window.location.href);
                const mblId = url.searchParams.get('Id');

                $('#popuptrtbody').empty();
                if (extraProperties != undefined && extraProperties.dimensions != null && extraProperties.dimensions.length > 0) {
                    for (let dimension of extraProperties.dimensions) {
                        addDimensionRow(dimension);
                    }

                    $('#CreateModal').modal('show');
                }
                else if (mblId != null) {
                    $('#popuptrtbody').empty();
                    dolphin.freight.importExport.airExports.airExportMawb.get(mblId).then(function (response) {
                        if (response && response.extraProperties && response.extraProperties.Dimensions) {
                            for (let dimension of response.extraProperties.Dimensions) {
                                addDimensionRow(dimension);
                            }
                        } else if (extraProperties != undefined && extraProperties.dimensions != null && extraProperties.dimensions.length > 0) {
                            for (let dimension of extraProperties.dimensions) {
                                addDimensionRow(dimension);
                            }

                            $('#CreateModal').modal('show');
                        }

                        $('#CreateModal').modal('show');
                    });
                }
                break;

            default:
                $('#popuptrtbody').empty();
                if (extraProperties != undefined && extraProperties.dimensions != null && extraProperties.dimensions.length > 0) {
                    for (let dimension of extraProperties.dimensions) {
                        addDimensionRow(dimension);
                    }

                    $('#CreateModal').modal('show');
                } else {
                    $('#CreateModal').modal('show');
                }
                break;
        }
    }

    static createNewDimension() {
        addDimensionRow();
    }

    static deleteDimension() {
        $('#popuptrtbody tr').each(function () {
            if ($(this).find('input[type="checkbox"]').prop('checked')) {
                $(this).remove();
            }
        });

        rowCount = 0;
        $('#popuptrtbody tr').each(function () {
            rowCount++;
            $(this).find('input[type="text"]').each(function () {
                let idAttr = $(this).attr('id');
                let nameAttr = $(this).attr('name');
                if (idAttr && nameAttr) {
                    let baseidName = idAttr.split('_')[0];
                    $(this).attr('id', baseidName + '_' + rowCount);

                    let basenameName = nameAttr.split('.')[1];
                    $(this).attr('name', 'Dimensions[' + rowCount + '].' + basenameName);
                }
            });
            $(this).find('input[type="checkbox"]').attr('name', 'dimensionCheckbox_' + rowCount);
        });

        updateTotals();

        updateDeleteButtonState();
    }

    static onChangeSellingRate(e) {
        if ($('#MawbModel_ChargeableWeightAmount').length == 0) {
            if ($('#AirExportMawbDto_SellingRateUnitType').val() == 1) {
                var sellingRate = $(e.currentTarget || e.target || e.srcElement).val();
                var chargableWeightKG = $('#ChargeableWeightKg').val();
                var awbChargableWeightKG = $('#AwbChargeableWeightKg').val();
                $('#AirExportMawbDto_ChargeableWeightAmount').val(parseFloat(sellingRate * chargableWeightKG).toFixed(2));
                $('#AirExportMawbDto_AwbChargeableWeightAmount').val(parseFloat(sellingRate * awbChargableWeightKG).toFixed(2));
            }
            else if ($('#AirExportMawbDto_SellingRateUnitType').val() == 2) {
                var sellingRate = $(e.currentTarget || e.target || e.srcElement).val();
                var chargableWeightLB = $('#ChargeableWeightLb').val();
                var awbChargableWeightLB = $('#AwbChargeableWeightLb').val();
                $('#AirExportMawbDto_ChargeableWeightAmount').val(parseFloat(sellingRate * chargableWeightLB).toFixed(2));
                $('#AirExportMawbDto_AwbChargeableWeightAmount').val(parseFloat(sellingRate * awbChargableWeightLB).toFixed(2));
            }
            else {
                $('#AirExportMawbDto_ChargeableWeightAmount').val(0);
                $('#AirExportMawbDto_AwbChargeableWeightAmount').val(0);
            }
        }
        else {
            if ($('#MawbModel_SellingRateUnit').val() == 1) {
                var sellingRate = $(e.currentTarget || e.target || e.srcElement).val();
                var chargableWeightKG = $('#ChargeableWeightKg').val();
                var awbChargableWeightKG = $('#AwbChargeableWeightKg').val();
                $('#MawbModel_ChargeableWeightAmount').val(parseFloat(sellingRate * chargableWeightKG).toFixed(2));
                $('#MawbModel_AwbChargeableWeightAmount').val(parseFloat(sellingRate * awbChargableWeightKG).toFixed(2));
            }
            else if ($('#MawbModel_SellingRateUnit').val() == 2) {
                var sellingRate = $(e.currentTarget || e.target || e.srcElement).val();
                var chargableWeightLB = $('#ChargeableWeightLb').val();
                var awbChargableWeightLB = $('#AwbChargeableWeightLb').val();
                $('#MawbModel_ChargeableWeightAmount').val(parseFloat(sellingRate * chargableWeightLB).toFixed(2));
                $('#MawbModel_AwbChargeableWeightAmount').val(parseFloat(sellingRate * awbChargableWeightLB).toFixed(2));
            }
            else {
                $('#MawbModel_ChargeableWeightAmount').val(0);
                $('#MawbModel_AwbChargeableWeightAmount').val(0);
            }
        }
    }

    static onChangeUnit(e) {
        if ($('#MawbModel_ChargeableWeightAmount').length == 0) {
            if ($(e.currentTarget || e.target || e.srcElement).val() == 1) {
                var sellingRate = $('#AirExportMawbDto_SellingRate').val();
                var chargableWeightKG = $('#ChargeableWeightKg').val();
                var awbChargableWeightKG = $('#AwbChargeableWeightKg').val();
                $('#AirExportMawbDto_ChargeableWeightAmount').val(parseFloat(sellingRate * chargableWeightKG).toFixed(2));
                $('#AirExportMawbDto_AwbChargeableWeightAmount').val(parseFloat(sellingRate * awbChargableWeightKG).toFixed(2));
            }
            else if ($(e.currentTarget || e.target || e.srcElement).val() == 2) {
                var sellingRate = $('#AirExportMawbDto_SellingRate').val();
                var chargableWeightLB = $('#ChargeableWeightLb').val();
                var awbChargableWeightLB = $('#AwbChargeableWeightLb').val();
                $('#AirExportMawbDto_ChargeableWeightAmount').val(parseFloat(sellingRate * chargableWeightLB).toFixed(2));
                $('#AirExportMawbDto_AwbChargeableWeightAmount').val(parseFloat(sellingRate * awbChargableWeightLB).toFixed(2));
            }
            else {
                $('#AirExportMawbDto_ChargeableWeightAmount').val(0);
                $('#AirExportMawbDto_AwbChargeableWeightAmount').val(0);
            }
        }
        else {
            if ($(e.currentTarget || e.target || e.srcElement).val() == 1) {
                var sellingRate = $('#MawbModel_SellingRate').val();
                var chargableWeightKG = $('#ChargeableWeightKg').val();
                var awbChargableWeightKG = $('#AwbChargeableWeightKg').val();
                $('#MawbModel_ChargeableWeightAmount').val(parseFloat(sellingRate * chargableWeightKG).toFixed(2));
                $('#MawbModel_AwbChargeableWeightAmount').val(parseFloat(sellingRate * awbChargableWeightKG).toFixed(2));
            }
            else if ($(e.currentTarget || e.target || e.srcElement).val() == 2) {
                var sellingRate = $('#MawbModel_SellingRate').val();
                var chargableWeightLB = $('#ChargeableWeightLb').val();
                var awbChargableWeightLB = $('#AwbChargeableWeightLb').val();
                $('#MawbModel_ChargeableWeightAmount').val(parseFloat(sellingRate * chargableWeightLB).toFixed(2));
                $('#MawbModel_AwbChargeableWeightAmount').val(parseFloat(sellingRate * awbChargableWeightLB).toFixed(2));
            }
        }
    }
}

function applyPopupValues() {
    let totalPCS = 0, totalKG = 0, totalCBM = 0;

    let dimensionsData = [];

    $('#popuptrtbody tr').each(function () {
        const length = parseFloat($(this).find('input[id^="length_"]').val()) || 0;
        const width = parseFloat($(this).find('input[id^="width_"]').val()) || 0;
        const height = parseFloat($(this).find('input[id^="height_"]').val()) || 0;
        const pcs = parseFloat($(this).find('input[id^="pcs_"]').val()) || 0;
        const kg = parseFloat($(this).find('input[id^="kg_"]').val()) || 0;
        const cbm = parseFloat($(this).find('input[id^="cbm_"]').val()) || 0;

        totalPCS += pcs;
        totalKG += kg;
        totalCBM += cbm;

        dimensionsData.push({
            length: length,
            width: width,
            height: height,
            pcs: pcs
        });
    });

    extraProperties = {
        dimensions: dimensionsData
    };
    
    $('#dimensionsJSON').val(JSON.stringify(extraProperties.dimensions));

    $('#MawbModel_VolumeWeightKg').val(parseFloat(totalKG).toFixed(2));
    $('#AirExportMawbDto_VolumeWeightKg').val(parseFloat(totalKG).toFixed(2));
    $('#AirExportMawbDto_VolumeWeightCbm').val(parseFloat(totalCBM).toFixed(2));
    $('#MawbModel_VolumeWeightCbm').val(parseFloat(totalCBM).toFixed(2));
    $('#ChargeableWeightKg').val(parseFloat(totalKG).toFixed(2));
    $('#AwbChargeableWeightKg').val(parseFloat(totalKG).toFixed(2));
    $('#ChargeableWeightLb').val(parseFloat(totalKG * 2.20462).toFixed(2));
    $('#AwbChargeableWeightLb').val(parseFloat(totalKG * 2.20462).toFixed(2));

    $('#CreateModal').modal('hide');
}

function addDimensionRow(dimension = null) {
    rowCount++;
    let newRow = $('<tr>');

    let checkboxCell = $('<td>');
    let checkbox = $('<input>', { type: 'checkbox', name: 'dimensionCheckbox_' + rowCount });
    checkboxCell.append(checkbox);

    let lengthInput = $('<td>').append($('<input>', { type: 'text', name: 'Dimensions[' + rowCount + '].Length', id: 'length_' + rowCount, value: dimension ? dimension.Length || dimension.length : '' }));
    let widthInput = $('<td>').append($('<input>', { type: 'text', name: 'Dimensions[' + rowCount + '].Width', id: 'width_' + rowCount, value: dimension ? dimension.Width || dimension.width : '' }));
    let heightInput = $('<td>').append($('<input>', { type: 'text', name: 'Dimensions[' + rowCount + '].Height', id: 'height_' + rowCount, value: dimension ? dimension.Height || dimension.height: '' }));
    let pcsInput = $('<td>').append($('<input>', { type: 'text', name: 'Dimensions[' + rowCount + '].Pcs', id: 'pcs_' + rowCount, value: dimension ? dimension.Pcs || dimension.pcs : '' }));
    let kgInput = $('<td>').append($('<input>', { type: 'text', id: 'kg_' + rowCount, disabled: true }));
    let lbInput = $('<td>').append($('<input>', { type: 'text', id: 'lb_' + rowCount, disabled: true }));
    let cbmInput = $('<td>').append($('<input>', { type: 'text', id: 'cbm_' + rowCount, disabled: true }));
    let cftInput = $('<td>').append($('<input>', { type: 'text', id: 'cft_' + rowCount, disabled: true }));

    newRow.append(checkboxCell, lengthInput, widthInput, heightInput, pcsInput, kgInput, lbInput, cbmInput, cftInput);

    newRow.find('input[id^="length_"], input[id^="width_"], input[id^="height_"], input[id^="pcs_"]').on('input', function () {
        calculateWeights(newRow);
    });

    if (dimension) {
        calculateWeights(newRow);
    }

    $('#popuptrtbody').append(newRow);
    updateTotals();
    updateDeleteButtonState();
}

function updateDeleteButtonState() {
    if ($('#popuptrtbody tr').length > 0) {
        $('#deleteDimensionBtn').prop('disabled', false);
    } else {
        $('#deleteDimensionBtn').prop('disabled', true);
    }
}

function calculateWeights(row) {
    const length = parseFloat(row.find('input[id^="length_"]').val()) || 0;
    const width = parseFloat(row.find('input[id^="width_"]').val()) || 0;
    const height = parseFloat(row.find('input[id^="height_"]').val()) || 0;
    const pcs = parseFloat(row.find('input[id^="pcs_"]').val()) || 0;

    const kgWeight = (length * width * height * pcs) / 6000;
    const lbsWeight = kgWeight * 2.20462062;
    const cft = kgWeight / 4.72 + 0.0049;
    const cbm = cft * 0.0283168;

    row.find('input[id^="kg_"]').val(kgWeight.toFixed(2));
    row.find('input[id^="lb_"]').val(lbsWeight.toFixed(2));
    row.find('input[id^="cbm_"]').val(cbm.toFixed(2));
    row.find('input[id^="cft_"]').val(cft.toFixed(2));
}

function updateTotals() {
    let totalKG = 0, totalLB = 0, totalCFT = 0, totalCBM = 0, totalPCS = 0;

    $('#popuptrtbody tr').each(function () {
        totalKG += parseFloat($(this).find('input[id^="kg_"]').val()) || 0;
        totalLB += parseFloat($(this).find('input[id^="lb_"]').val()) || 0;
        totalCFT += parseFloat($(this).find('input[id^="cft_"]').val()) || 0;
        totalCBM += parseFloat($(this).find('input[id^="cbm_"]').val()) || 0;
        totalPCS += parseFloat($(this).find('input[id^="pcs_"]').val()) || 0;
    });

    $('#totalPCS').text(totalPCS);
    $('#totalKG').text(totalKG.toFixed(2));
    $('#totalLB').text(totalLB.toFixed(2));
    $('#totalCFT').text(totalCFT.toFixed(2));
    $('#totalCBM').text(totalCBM.toFixed(2));
}
