var HblextraProperties;
let HblrowCount = 0;

$(document).ready(function () {
    const ids = [
        "HawbModel_FinalETA",
        "HawbModel_LastFreeDay",
        "HawbModel_StorageStartDate",
        "HawbModel_ITDate",
        "HawbModel_FrtRelease",
        "HawbModel_CReleasedDate",
        "HawbModel_DoorDelivered"
    ];

    var dateTimePicker = new CustomDateTimePicker();
    dateTimePicker.dateTimePicker(ids, false);

    if ($('#HawbCancelledDate').val() == '0001-01-01T00:00') {
        $('#HawbCancelledDate').val('');
    }
    if ($('#HawbModel_FrtRelease').val() == '0001-01-01T00:00') {
        $('#HawbModel_FrtRelease').val('');
    }
});

$(document).on('input', 'input[id^="Hlength_"], input[id^="Hwidth_"], input[id^="Hheight_"], input[id^="Hpcs_"]', function () {
    const row = $(this).closest('tr');
    HcalculateWeights(row);
});

$(document).on('input', 'input', function () {
    HupdateTotals();
});

$(document).on('click', '#HapplyDimensions', function () {
    HapplyPopupValues();
});

class AirImportHawb {
    static OpenInTrackTrace() {
        var url = new URL(window.location.href);
        var mawbId = url.searchParams.get('Id');
        dolphin.freight.importExport.airImports.airImportMawb.get(mawbId).done(function (res) {
            myWindow = window.open('https://www.track-trace.com/aircargo#' + res.mawbNo, '_blank', 'width=1200,height=1000');
            myWindow.focus();
        });
    }

    static onCopyToHawbClick() {
        var id = '@Model.HawbModel.Id';
        createHawbCopyModal.open({
            id,
        });
    }

    static setHawbLockUnlock(isLockedCheck) {
        var id = '@Model.HawbModel.Id';
        isLockedCheck = isLockedCheck === "True";
        if (isLockedCheck) {
            abp.message.confirm(l('UnlockConfirmationMessage')).then(function (confirmed) {
                if (confirmed) {
                    dolphin.freight.importExport.airImports.airImportHawb
                        .lockedOrUnLockedAirImportHawb(id)
                        .then(function () {
                            abp.message.success(l('SuccessfullyUnlocked'));
                            window.location.reload();
                        });
                }
            });
        } else {
            abp.message.confirm(l('LockConfirmationMessage')).then(function (confirmed) {
                if (confirmed) {
                    dolphin.freight.importExport.airImports.airImportHawb
                        .lockedOrUnLockedAirImportHawb(id)
                        .then(function () {
                            abp.message.success(l('SuccessfullyLocked'));
                            window.location.reload();
                        });
                }
            });
        }
    }

    static KgToLbConversion(Elem, Unit) {
        switch (Unit) {
            case 'KG':
                var LB = parseFloat(Elem.currentTarget.value * 2.2).toFixed(2);
                $($(Elem.currentTarget).parent().parent().next().children().children()[1]).val(LB);
                break;
            case 'LB':
                var KG = parseFloat(Elem.currentTarget.value / 2.2).toFixed(2);
                $($(Elem.currentTarget).parent().parent().prev().children().children()[1]).val(KG);
                break;
            case 'VolKG':
                var CBM = parseFloat(Elem.currentTarget.value * 0.01).toFixed(2);
                $($(Elem.currentTarget).parent().parent().next().children().children()[1]).val(CBM);
                break;
            case 'CBM':
                var VolKG = parseFloat(Elem.currentTarget.value * 166.67).toFixed(2);
                $($(Elem.currentTarget).parent().parent().prev().children().children()[1]).val(VolKG);
                break;
        }
    }

    static openPopUp(situation) {
        switch (situation) {
            case 'withId':
                const hblId = $('#HawbModel_Id').val();

                $('#Hpopuptrtbody').empty();
                debugger;
                if (HblextraProperties != undefined && HblextraProperties.dimensions != null && HblextraProperties.dimensions.length > 0) {
                    for (let dimension of HblextraProperties.dimensions) {
                        HaddDimensionRow(dimension);
                    }

                    $('#HCreateModal').modal('show');
                }
                else if (hblId != null && hblId != '00000000-0000-0000-0000-000000000000') {
                    $('#Hpopuptrtbody').empty();
                    dolphin.freight.importExport.airImports.airImportHawb.get(hblId).then(function (response) {
                        if (response && response.extraProperties && response.extraProperties.Dimensions) {
                            for (let dimension of response.extraProperties.Dimensions) {
                                HaddDimensionRow(dimension);
                            }
                        } else if (HblextraProperties != undefined && HblextraProperties.dimensions != null && HblextraProperties.dimensions.length > 0) {
                            for (let dimension of HblextraProperties.dimensions) {
                                HaddDimensionRow(dimension);
                            }

                            $('#HCreateModal').modal('show');
                        }

                        $('#HCreateModal').modal('show');
                    });
                }
                else {
                    $('#HCreateModal').modal('show');
                }
                break;

            default:
                debugger;
                $('#Hpopuptrtbody').empty();
                if (HblextraProperties != undefined && HblextraProperties.dimensions != null && HblextraProperties.dimensions.length > 0) {
                    for (let dimension of HblextraProperties.dimensions) {
                        HaddDimensionRow(dimension);
                    }

                    $('#HCreateModal').modal('show');
                } else {
                    $('#HCreateModal').modal('show');
                }
                break;
        }
    }

    static createNewDimension() {
        HaddDimensionRow();
    }

    static deleteDimension() {
        $('#Hpopuptrtbody tr').each(function () {
            if ($(this).find('input[type="checkbox"]').prop('checked')) {
                $(this).remove();
            }
        });

        HblrowCount = 0;
        $('#Hpopuptrtbody tr').each(function () {
            HblrowCount++;
            $(this).find('input[type="text"]').each(function () {
                let idAttr = $(this).attr('id');
                let nameAttr = $(this).attr('name');
                if (idAttr && nameAttr) {
                    let baseidName = idAttr.split('_')[0];
                    $(this).attr('id', baseidName + '_' + HblrowCount);

                    let basenameName = nameAttr.split('.')[1];
                    $(this).attr('name', 'Dimensions[' + HblrowCount + '].' + basenameName);
                }
            });
            $(this).find('input[type="checkbox"]').attr('name', 'dimensionCheckbox_' + HblrowCount);
        });

        HupdateTotals();

        HupdateDeleteButtonState();
    }
}

function HapplyPopupValues() {
    let HtotalPCS = 0, HtotalKG = 0, HtotalCBM = 0;

    let HdimensionsData = [];

    $('#Hpopuptrtbody tr').each(function () {
        const length = parseFloat($(this).find('input[id^="Hlength_"]').val()) || 0;
        const width = parseFloat($(this).find('input[id^="Hwidth_"]').val()) || 0;
        const height = parseFloat($(this).find('input[id^="Hheight_"]').val()) || 0;
        const pcs = parseFloat($(this).find('input[id^="Hpcs_"]').val()) || 0;
        const kg = parseFloat($(this).find('input[id^="Hkg_"]').val()) || 0;
        const cbm = parseFloat($(this).find('input[id^="Hcbm_"]').val()) || 0;

        HtotalPCS += pcs;
        HtotalKG += kg;
        HtotalCBM += cbm;

        HdimensionsData.push({
            length: length,
            width: width,
            height: height,
            pcs: pcs
        });
    });

    HblextraProperties = {
        dimensions: HdimensionsData
    };

    $('#HawbModel_HawbDimensionsJSON').val(JSON.stringify(HblextraProperties.dimensions));

    $('#HawbModel_VolumeWeightKG').val(parseFloat(HtotalKG).toFixed(2));
    $('#HawbModel_VolumeWeightCBM').val(parseFloat(HtotalCBM).toFixed(2));
    $('#HawbModel_ChargeableWeightKG').val(parseFloat(HtotalKG).toFixed(2));
    $('#HawbModel_ChargeableWeightLB').val(parseFloat(HtotalKG * 2.20462).toFixed(2));

    $('#HCreateModal').modal('hide');
}

function HaddDimensionRow(dimension = null) {
    HblrowCount++;
    let newRow = $('<tr>');

    let checkboxCell = $('<td>');
    let checkbox = $('<input>', { type: 'checkbox', name: 'dimensionCheckbox_' + HblrowCount });
    checkboxCell.append(checkbox);

    let lengthInput = $('<td>').append($('<input>', { type: 'text', name: 'Dimensions[' + HblrowCount + '].Length', id: 'Hlength_' + HblrowCount, value: dimension ? dimension.Length || dimension.length : '' }));
    let widthInput = $('<td>').append($('<input>', { type: 'text', name: 'Dimensions[' + HblrowCount + '].Width', id: 'Hwidth_' + HblrowCount, value: dimension ? dimension.Width || dimension.width : '' }));
    let heightInput = $('<td>').append($('<input>', { type: 'text', name: 'Dimensions[' + HblrowCount + '].Height', id: 'Hheight_' + HblrowCount, value: dimension ? dimension.Height || dimension.height : '' }));
    let pcsInput = $('<td>').append($('<input>', { type: 'text', name: 'Dimensions[' + HblrowCount + '].Pcs', id: 'Hpcs_' + HblrowCount, value: dimension ? dimension.Pcs || dimension.pcs : '' }));
    let kgInput = $('<td>').append($('<input>', { type: 'text', id: 'Hkg_' + HblrowCount, disabled: true }));
    let lbInput = $('<td>').append($('<input>', { type: 'text', id: 'Hlb_' + HblrowCount, disabled: true }));
    let cbmInput = $('<td>').append($('<input>', { type: 'text', id: 'Hcbm_' + HblrowCount, disabled: true }));
    let cftInput = $('<td>').append($('<input>', { type: 'text', id: 'Hcft_' + HblrowCount, disabled: true }));

    newRow.append(checkboxCell, lengthInput, widthInput, heightInput, pcsInput, kgInput, lbInput, cbmInput, cftInput);

    newRow.find('input[id^="Hlength_"], input[id^="Hwidth_"], input[id^="Hheight_"], input[id^="Hpcs_"]').on('input', function () {
        HcalculateWeights(newRow);
    });

    if (dimension) {
        HcalculateWeights(newRow);
    }

    $('#Hpopuptrtbody').append(newRow);
    HupdateTotals();
    HupdateDeleteButtonState();
}

function HupdateDeleteButtonState() {
    if ($('#Hpopuptrtbody tr').length > 0) {
        $('#HdeleteDimensionBtn').prop('disabled', false);
    } else {
        $('#HdeleteDimensionBtn').prop('disabled', true);
    }
}

function HcalculateWeights(row) {
    const Hlength = parseFloat(row.find('input[id^="Hlength_"]').val()) || 0;
    const Hwidth = parseFloat(row.find('input[id^="Hwidth_"]').val()) || 0;
    const Hheight = parseFloat(row.find('input[id^="Hheight_"]').val()) || 0;
    const Hpcs = parseFloat(row.find('input[id^="Hpcs_"]').val()) || 0;

    const kgWeight = (Hlength * Hwidth * Hheight * Hpcs) / 6000;
    const lbsWeight = kgWeight * 2.20462062;
    const cft = kgWeight / 4.72 + 0.0049;
    const cbm = cft * 0.0283168;

    row.find('input[id^="Hkg_"]').val(kgWeight.toFixed(2));
    row.find('input[id^="Hlb_"]').val(lbsWeight.toFixed(2));
    row.find('input[id^="Hcbm_"]').val(cbm.toFixed(2));
    row.find('input[id^="Hcft_"]').val(cft.toFixed(2));
}

function HupdateTotals() {
    let HtotalKG = 0, HtotalLB = 0, HtotalCFT = 0, HtotalCBM = 0, HtotalPCS = 0;

    $('#Hpopuptrtbody tr').each(function () {
        HtotalKG += parseFloat($(this).find('input[id^="Hkg_"]').val()) || 0;
        HtotalLB += parseFloat($(this).find('input[id^="Hlb_"]').val()) || 0;
        HtotalCFT += parseFloat($(this).find('input[id^="Hcft_"]').val()) || 0;
        HtotalCBM += parseFloat($(this).find('input[id^="Hcbm_"]').val()) || 0;
        HtotalPCS += parseFloat($(this).find('input[id^="Hpcs_"]').val()) || 0;
    });

    $('#HtotalPCS').text(HtotalPCS);
    $('#HtotalKG').text(HtotalKG.toFixed(2));
    $('#HtotalLB').text(HtotalLB.toFixed(2));
    $('#HtotalCFT').text(HtotalCFT.toFixed(2));
    $('#HtotalCBM').text(HtotalCBM.toFixed(2));
}

function checkforId() {
    debugger;
    new URL(window.location.href).searchParams.get('Id') == null ? AirImportHawb.openPopUp() : AirImportHawb.openPopUp('withId');
}