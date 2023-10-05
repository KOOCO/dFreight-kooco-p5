﻿$(document).on('change', '[name=PackagingUnit]', function () {
    $('#totalPackageTypeUnit').text($('#PackagingUnit').find('option:selected').text());
});

$(document).on('input', 'input[name^="length_"], input[name^="width_"], input[name^="height_"], input[name^="pcs_"]', function () {
    const row = $(this).closest('tr');
    calculateWeights(row);
});

$(document).on('click', '#applyDimensions', function () {
    applyPopupValues();
});

$(document).on('input', 'input', function () {
    updateTotals();
});

$("#saveBtn").click(function () {
    $("#mPoNo").val($("#PoNoTag").tagsStr());

    $("#edit2Form").submit();
});

let rowCount = 0;

function getHblCheckbox(mblId, index, callback) {
    dolphin.freight.importExport.oceanImports.oceanImportHbl.getHblCardsById(mblId).done(function (res) {
        let checkboxesHTML = '';
        let headersHTML = '';
        var tdindex = 0;
        for (let hbl of res) {
            checkboxesHTML += `<td style='display: none;'><input type='checkbox' data-id='${hbl.id}' data-containerNo='' onclick='EditModel2.SaveHBLContainer()' id='assignContainerCheckbox_${index}_${tdindex}' style='cursor: pointer;'></td>`;
            headersHTML += `<th style="text-align: center; display: none;"><div style="background-color: ${hbl.cardColorValue}; width: 12px; height: 12px; border-radius: 50%; margin: 0 auto;"></div><input type="checkbox" id="hblHeaders_${hbl.hblNo}" style="cursor: pointer; margin-top: 10px;"></th>`
            tdindex++;
        }
        callback(checkboxesHTML, headersHTML);
    });
}

function updateDeleteButtonState() {
    if ($('#popuptrtbody tr').length > 0) {
        $('#deleteDimensionBtn').prop('disabled', false);
    } else {
        $('#deleteDimensionBtn').prop('disabled', true);
    }
}

function calculateWeights(row) {
    const length = parseFloat(row.find('input[name^="length_"]').val()) || 0;
    const width = parseFloat(row.find('input[name^="width_"]').val()) || 0;
    const height = parseFloat(row.find('input[name^="height_"]').val()) || 0;
    const pcs = parseFloat(row.find('input[name^="pcs_"]').val()) || 0;

    const kgWeight = (length * width * height * pcs) / 6000;
    const lbsWeight = kgWeight * 2.20462062;
    const cft = kgWeight / 4.72 + 0.0049;
    const cbm = cft * 0.0283168;

    row.find('input[name^="kg_"]').val(kgWeight.toFixed(2));
    row.find('input[name^="lb_"]').val(lbsWeight.toFixed(2));
    row.find('input[name^="cbm_"]').val(cbm.toFixed(2));
    row.find('input[name^="cft_"]').val(cft.toFixed(2));
}

function updateTotals() {
    let totalKG = 0, totalLB = 0, totalCFT = 0, totalCBM = 0, totalPCS = 0;

    $('#popuptrtbody tr').each(function () {
        totalKG += parseFloat($(this).find('input[name^="kg_"]').val()) || 0;
        totalLB += parseFloat($(this).find('input[name^="lb_"]').val()) || 0;
        totalCFT += parseFloat($(this).find('input[name^="cft_"]').val()) || 0;
        totalCBM += parseFloat($(this).find('input[name^="cbm_"]').val()) || 0;
        totalPCS += parseFloat($(this).find('input[name^="pcs_"]').val()) || 0;
    });

    $('#totalPCS').text(totalPCS);
    $('#totalKG').text(totalKG.toFixed(2));
    $('#totalLB').text(totalLB.toFixed(2));
    $('#totalCFT').text(totalCFT.toFixed(2));
    $('#totalCBM').text(totalCBM.toFixed(2));
}

function applyPopupValues() {
    let totalPCS = 0, totalKG = 0, totalCBM = 0;

    let dimensionsData = [];

    $('#popuptrtbody tr').each(function () {
        const length = parseFloat($(this).find('input[name^="length_"]').val()) || 0; 
        const width = parseFloat($(this).find('input[name^="width_"]').val()) || 0; 
        const height = parseFloat($(this).find('input[name^="height_"]').val()) || 0; 
        const pcs = parseFloat($(this).find('input[name^="pcs_"]').val()) || 0;
        const kg = parseFloat($(this).find('input[name^="kg_"]').val()) || 0;
        const cbm = parseFloat($(this).find('input[name^="cbm_"]').val()) || 0;

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

    const extraProperties = {
        dimensions: dimensionsData
    };

    let targetRowIndex = $('#applyDimensions').data('target-row');

    var containerid = $('#fCalc_' + targetRowIndex).data('containerid');

    let targetRow = $('#trtbody #tr_' + targetRowIndex);

    targetRow.find('input[name^="CreateUpdateContainerDtos["]').filter('input[id^="f4_"]').val(totalPCS);
    targetRow.find('input[name^="CreateUpdateContainerDtos["]').filter('input[id^="f5_"]').val(totalKG);
    targetRow.find('input[name^="CreateUpdateContainerDtos["]').filter('input[id^="f6_"]').val(totalCBM);

    countTotal();
    countTotalVolume();
    countPackageType();

    const sendData = {
        dimensions: dimensionsData,
        containerId: containerid
    };

    dolphin.freight.importExport.oceanImports.oceanImportMbl.saveDimensions(sendData).done(function () { });

    $('#CreateModal').modal('hide');
}

function addDimensionRow(dimension = null) {
    rowCount++;

    let newRow = $('<tr>');

    let checkboxCell = $('<td>');
    let checkbox = $('<input>', { type: 'checkbox', name: 'dimensionCheckbox_' + rowCount });
    checkboxCell.append(checkbox);

    let lengthInput = $('<td>').append($('<input>', { type: 'text', name: 'length_' + rowCount, value: dimension ? dimension.Length : '' }));
    let widthInput = $('<td>').append($('<input>', { type: 'text', name: 'width_' + rowCount, value: dimension ? dimension.Width : '' }));
    let heightInput = $('<td>').append($('<input>', { type: 'text', name: 'height_' + rowCount, value: dimension ? dimension.Height : '' }));
    let pcsInput = $('<td>').append($('<input>', { type: 'text', name: 'pcs_' + rowCount, value: dimension ? dimension.Pcs : '' }));
    let kgInput = $('<td>').append($('<input>', { type: 'text', name: 'kg_' + rowCount, disabled: true }));
    let lbInput = $('<td>').append($('<input>', { type: 'text', name: 'lb_' + rowCount, disabled: true }));
    let cbmInput = $('<td>').append($('<input>', { type: 'text', name: 'cbm_' + rowCount, disabled: true }));
    let cftInput = $('<td>').append($('<input>', { type: 'text', name: 'cft_' + rowCount, disabled: true }));

    newRow.append(checkboxCell, lengthInput, widthInput, heightInput, pcsInput, kgInput, lbInput, cbmInput, cftInput);

    newRow.find('input[name^="length_"], input[name^="width_"], input[name^="height_"], input[name^="pcs_"]').on('input', function () {
        calculateWeights(newRow);
    });

    if (dimension) {
        // Trigger the calculation to update kg, lb, cft, and cbm
        calculateWeights(newRow);
    }

    $('#popuptrtbody').append(newRow);
    updateTotals();
    updateDeleteButtonState();
}
class EditModel2 {
    static showHideHBLCheckboxes() {
        if (!$('input[id^="hblHeaders_"]').parent().is(":visible")) {
            $('input[id^="hblHeaders_"]').parent().show();
        } else {
            $('input[id^="hblHeaders_"]').parent().hide();
        }

        if (!$('input[id^="assignContainerCheckbox_"]').parent().is(":visible")) {
            $('input[id^="selectAllAssignContainerCheckbox_"]').parent().show();
            $('input[id^="assignContainerCheckbox_"]').parent().show();
        } else {
            $('input[id^="selectAllAssignContainerCheckbox_"]').parent().hide();
            $('input[id^="assignContainerCheckbox_"]').parent().hide();
        }
    }

    static SaveHBLContainer() {
        if ($('input[id^="assignContainerCheckbox_"]:checked').length > 0 && $('input[id^="assignContainerCheckbox_"]:checked').is(":visible")) {
            var ids = [];
            var containers = [];
            var containerid;
            $('input[id^="assignContainerCheckbox_"]:checked').each(function (i, e) {
                var id = e.attributes[1].value;
                var container = e.attributes[2].value;
                ids.push(id);
                containers.push(container);
            });

            const promises = ids.map(id => {
                return new Promise((resolve) => {
                    dolphin.freight.importExport.containers.container.getContainerByHblId(id).done(function (r) {
                        if (r && r.id) {
                            resolve(r.id);
                        } else {
                            resolve('00000000-0000-0000-0000-000000000000');
                        }
                    });
                });
            });

            Promise.all(promises).then(results => {
                var AppModel = { Ids: ids, Containers: containers, ContainerId: results };
                dolphin.freight.importExport.oceanImports.oceanImportHbl.saveAssignContainerToHbl(AppModel).done(function (res) { });
            });
        }
    }

    static openPopUp(trIndex) {
        const containerId = $('#fCalc_' + trIndex).data('containerid');

        $('#popuptrtbody').empty();

        dolphin.freight.importExport.containers.container.get(containerId).then(function (response) {
            if (response && response.extraProperties && response.extraProperties.Dimensions) {
                for (let dimension of response.extraProperties.Dimensions) {
                    addDimensionRow(dimension);
                }
            }

            $('#CreateModal').modal('show');
            $('#applyDimensions').data('target-row', trIndex);
        });
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
                let nameAttr = $(this).attr('name');
                if (nameAttr) {
                    let baseName = nameAttr.split('_')[0];
                    $(this).attr('name', baseName + '_' + rowCount);
                }
            });
            $(this).find('input[type="checkbox"]').attr('name', 'dimensionCheckbox_' + rowCount);
        });

        updateTotals();

        updateDeleteButtonState();
    }
}

