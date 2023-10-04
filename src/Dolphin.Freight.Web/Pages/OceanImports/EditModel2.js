$(document).on('change', '[name=PackagingUnit]', function () {
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

    EditModel2.SaveHBLContainer();

    $("#edit2Form").submit();
});

let rowCount = 0;

function getHblCheckbox(mblId, index, callback) {
    dolphin.freight.importExport.oceanImports.oceanImportHbl.getHblCardsById(mblId).done(function (res) {
        let checkboxesHTML = '';
        let headersHTML = '';
        var tdindex = 0;
        for (let hbl of res) {
            checkboxesHTML += `<td style='display: none;'><input type='checkbox' data-id='${hbl.id}' data-containerNo='' id='assignContainerCheckbox_${index}_${tdindex}' onclick='EditModel2.SingleHBLContainer(this)' style='cursor: pointer;'></td>`;
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
    const lbsWeight = kgWeight * 2.20462;
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

    $('#popuptrtbody tr').each(function () {
        totalPCS += parseFloat($(this).find('input[name^="pcs_"]').val()) || 0;
        totalKG += parseFloat($(this).find('input[name^="kg_"]').val()) || 0;
        totalCBM += parseFloat($(this).find('input[name^="cbm_"]').val()) || 0;
    });

    let targetRowIndex = $('#applyDimensions').data('target-row');

    let targetRow = $('#trtbody #tr_' + targetRowIndex);

    targetRow.find('input[name^="CreateUpdateContainerDtos["]').filter('input[id^="f4_"]').val(totalPCS);
    targetRow.find('input[name^="CreateUpdateContainerDtos["]').filter('input[id^="f5_"]').val(totalKG);
    targetRow.find('input[name^="CreateUpdateContainerDtos["]').filter('input[id^="f6_"]').val(totalCBM);

    countTotal();
    countTotalVolume();
    countPackageType();

    $('#CreateModal').modal('hide');
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
            $('input[id^="assignContainerCheckbox_"]:checked').each(function (i, e) {
                var id = e.attributes[1].value;
                var container = e.attributes[2].value;
                ids.push(id);
                containers.push(container);
            });

            var AppModel = { Ids: ids, Containers: containers };
            dolphin.freight.importExport.oceanImports.oceanImportHbl.saveAssignContainerToHbl(AppModel).done(function (res) {
                debugger;
            });
        }
    }

    static openPopUp(trIndex) {
        $('#CreateModal').modal('show');
        $('#applyDimensions').data('target-row', trIndex);
    }

    static createNewDimension() {
        rowCount++;

        let newRow = $('<tr>');
        let checkboxCell = $('<td>');
        let checkbox = $('<input>', { type: 'checkbox', name: 'dimensionCheckbox_' + rowCount });
        checkboxCell.append(checkbox);

        let lengthInput = $('<td><input type="text" name="length_' + rowCount + '"></td>');
        let widthInput = $('<td><input type="text" name="width_' + rowCount + '"></td>');
        let heightInput = $('<td><input type="text" name="height_' + rowCount + '"></td>');
        let pcsInput = $('<td><input type="text" name="pcs_' + rowCount + '"></td>');
        let kgInput = $('<td><input type="text" name="kg_' + rowCount + '" disabled></td>');
        let lbInput = $('<td><input type="text" name="lb_' + rowCount + '" disabled></td>');
        let cbmInput = $('<td><input type="text" name="cbm_' + rowCount + '" disabled></td>');
        let cftInput = $('<td><input type="text" name="cft_' + rowCount + '" disabled></td>');

        newRow.append(checkboxCell, lengthInput, widthInput, heightInput, pcsInput, kgInput, lbInput, cbmInput, cftInput);

        $('#popuptrtbody').append(newRow);

        calculateWeights(newRow);

        updateTotals();

        updateDeleteButtonState();
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

