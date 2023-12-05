$(function () {
    $("#saveBtn").click(function () {
        $("#createForm").submit();
    })
});

$(document).on('input', 'input[id^="length_"], input[id^="width_"], input[id^="height_"], input[id^="pcs_"]', function () {
    const row = $(this).closest('tr');
    calculateWeights(row);
});
$(document).on('input', 'input', function () {
    updateTotals();
});

$('#createForm').on('abp-ajax-success', function (result, rs) {
    location.href = 'EditMawb?ShowMsg=true&Id=' + rs.responseText.id
});

$(document).on('click', '#applyDimensions', function () {
    applyPopupValues();
});

var extraProperties;
let rowCount = 0;
class AirImportMawb {
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
                    dolphin.freight.importExport.airImports.airImportMawb.get(mblId).then(function (response) {
                        if (response && response.extraProperties && response.extraProperties.Dimensions) {
                            debugger;
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

    if (totalKG != 0) {
        $('#MawbModel_VolumeWeightKg').val(parseFloat(totalKG).toFixed(2));
        $('#ChargeableWeightKg').val(parseFloat(totalKG).toFixed(2));
        $('#ChargeableWeightLb').val(parseFloat(totalKG * 2.20462).toFixed(2));
    }

    if (totalCBM != 0) {
        $('#MawbModel_VolumeWeightCbm').val(parseFloat(totalCBM).toFixed(2));
    }

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
    let heightInput = $('<td>').append($('<input>', { type: 'text', name: 'Dimensions[' + rowCount + '].Height', id: 'height_' + rowCount, value: dimension ? dimension.Height || dimension.height : '' }));
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