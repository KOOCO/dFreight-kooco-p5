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
    if ($("#mPoNo").length > 0) {
        $("#mPoNo").val($("#PoNoTag").tagsStr());
    }

    $("#edit2Form").submit();
});

let rowCount = 0;
var htrindex = 0;
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

    if (containerid == null || typeof containerid === 'undefined' || containerid === '') {
        $('#CreateModal').modal('hide');
        abp.message.warn(l("Message:SaveContainer"));
        return;
    }

    const sendData = {
        dimensions: dimensionsData,
        containerId: containerid
    };

    dolphin.freight.importExport.oceanExports.oceanExportMbl.saveDimensions(sendData).done(function () { });

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

function checkAllHeaderCheckbox(Elem, checkboxindex, tdindex) {
    if ($(Elem)[0].checked) {
        $('#trtbody .tr').each(function (i, e) {
            $('#assignContainerCheckbox_' + i + '_' + tdindex).prop('checked', true);
        });
        EditModelOEContainer.SaveHBLContinerNo(tdindex, Elem);
    } else {
        $('#trtbody .tr').each(function (i, e) {
            $('#assignContainerCheckbox_' + i + '_' + tdindex).prop('checked', false);
        });
        EditModelOEContainer.SaveHBLContinerNo(tdindex, Elem);
    }
}

function checkContainerHasHblIdAsync(mblId, hblId) {
    return new Promise((resolve, reject) => {
        dolphin.freight.importExport.containers.container.getContainerByMblId(mblId).done(function (result) {
            let isChecked = false;
            result.forEach(function (i, e) {
                debugger;
                if (i.hblId === hblId) {
                    isChecked = true;
                }
            });
            resolve(isChecked);
        }).fail(function (error) {
            reject(error);
        });
    });
}

class EditModelOEContainer {
    //static getHblCheckbox(mblId, index, callback) {
    //    dolphin.freight.importExport.oceanExports.oceanExportHbl.getHblCardsById(mblId).done(function (res) {
    //        let checkboxesHTML = '';
    //        let headersHTML = '';
    //        var tdindex = 0;
    //        for (let hbl of res) {
    //            checkboxesHTML += `<td style='display: none;'><input type='checkbox' data-id='${hbl.id}' data-containerNo='' onclick='EditModelOEContainer.SaveHBLContainer()' id='assignContainerCheckbox_${index}_${tdindex}' style='cursor: pointer;'></td>`;
    //            headersHTML += `<th style="text-align: center; display: none;"><div style="background-color: ${hbl.cardColorValue}; width: 12px; height: 12px; border-radius: 50%; margin: 0 auto;"></div><input type="checkbox" id="hblHeaders_${hbl.hblNo}" style="cursor: pointer; margin-top: 10px;"></th>`
    //            tdindex++;
    //        }
    //        callback(checkboxesHTML, headersHTML);
    //    });
    //}

    static async getHblCheckbox(mblId, index, callback) {
        let res = await new Promise((resolve, reject) => {
            dolphin.freight.importExport.oceanExports.oceanExportHbl.getHblCardsById(mblId).done(function (data) {
                resolve(data);
            }).fail(function (error) {
                reject(error);
            });
        });
    
        let checkboxesHTML = '';
        let headersHTML = '';
        var tdindex = 0;
    
        for (let hbl of res) {
            var isChecked = await checkContainerHasHblIdAsync(mblId, hbl.id);
            var checked = isChecked ? 'checked' : '';
            checkboxesHTML += `<td style='display: none;'><input type='checkbox' data-id='${hbl.id}' data-containerNo='' data-containerid='${hbl.containerIds}' data-mblid='${mblId}' onclick='EditModelOEContainer.SaveHBLContainer(this)' id='assignContainerCheckbox_${index}_${tdindex}' ${checked} style='cursor: pointer;'></td>`;
            headersHTML += `<th style="text-align: center; display: none;"><div style="background-color: ${hbl.cardColorValue}; width: 12px; height: 12px; border-radius: 50%; margin: 0 auto;"></div><input type="checkbox" data-hblid='${hbl.id}' onclick="checkAllHeaderCheckbox(this, '${index}', '${tdindex}')" id="hblHeaders_${hbl.hblNo}" ${checked} style="cursor: pointer; margin-top: 10px;"></th>`;
            tdindex++;
        }
    
        callback(checkboxesHTML, headersHTML);
    }

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

    static SaveHorizontalHBLContainer(Elem, index) {

        if (!$(Elem).prop('checked')) {
            
        } else {
            var ids = []; var container; var containerid; var mblId;
            $('input[id^="assignContainerCheckbox_"]:checked').filter(function () { return this.id.match(new RegExp("assignContainerCheckbox_" + index + "_\\d+")); }).each(function (i, e) {
                var id = e.attributes[1].value;
                container = e.attributes[2].value;
                containerid = $('input[name="CreateUpdateContainerDtos[' + i + '].Id"]').attr('value');
                mblId = e.attributes[4].value;
                ids.push(id);
            });

            var AppModel = { Ids: ids, ContainerNo: container, Containersid: containerid, MblId: mblId };
            dolphin.freight.importExport.oceanExports.oceanExportHbl.saveAssignContainerToHbl(AppModel).done(function (res) { }).then(function () {
                location.reload();
            });
        }
    }

    static SaveHBLContainer(Elem) {
        debugger;
        switch (Elem) {
            default:
                if (!$(Elem).prop('checked')) {
                    var url = new URL(window.location.href);
                    var hblId = $(Elem).attr('data-id');
                    var mblId = url.searchParams.get('Id');
                    var i = $(Elem).attr('id').split('_')[1];
                    var containerId = $('input[name="CreateUpdateContainerDtos[' + i + '].Id"]').attr('value');
                    abp.message.confirm(l('Message:DeAssignHblFromContainers')).then(function (confirmed) {
                        if (confirmed) {
                            var AppModel = { MblId: mblId, HblId: hblId, Containersid: containerId };
                            dolphin.freight.importExport.oceanExports.oceanExportHbl.saveAssignSingleContainerNoToHbl(AppModel).done(function (res) { }).then(function () {
                                location.reload();
                            });
                        }
                    })
                } else {
                    var url = new URL(window.location.href);
                    var hblId = $(Elem).attr('data-id');
                    var mblId = url.searchParams.get('Id');
                    var i = $(Elem).attr('id').split('_')[1];
                    debugger;
                    var containerId = $('input[name="CreateUpdateContainerDtos[' + i + '].Id"]').attr('value');

                    var AppModel = { MblId: mblId, HblId: hblId, Containersid: containerId };
                    dolphin.freight.importExport.oceanExports.oceanExportHbl.saveAssignSingleContainerNoToHbl(AppModel).done(function (res) { }).then(function () {
                        location.reload();
                    });
                }
                break;
        }
    }

    static SaveHBLContinerNo(tdindex, Elem) {
        if ($('input[id^="assignContainerCheckbox_"]:checked').filter(function () { return this.id.match(new RegExp("assignContainerCheckbox_\\d+_" + tdindex + "$")); }).length > 0) {
            var id;
            var hblid;
            var containers = [];
            var containerids;
            $('input[id^="assignContainerCheckbox_"]:checked').filter(function () { return this.id.match(new RegExp("assignContainerCheckbox_\\d+_" + tdindex + "$")); }).each(function (i, e) {
                id = e.attributes[4].value;
                hblid = e.attributes[1].value;
                containerids = e.attributes[3].value;
            });

            var AppModel = { MblId: id, HblId: hblid, Containersid: containerids, ContainerNos: containers };
            dolphin.freight.importExport.oceanExports.oceanExportHbl.saveAssignContainerNoToHbl(AppModel).done(function (res) { }).then(function () {
                location.reload();
            });
        } else {
            var url = new URL(window.location.href);
            var hblId = $(Elem).attr('data-hblid');
            var mblId = url.searchParams.get('Id');

            abp.message.confirm(l('Message:DeAssignHblFromContainers')).then(function (confirmed) {
                if (confirmed) {
                    var AppModel = { MblId: mblId, HblId: hblId };
                    dolphin.freight.importExport.oceanExports.oceanExportHbl.saveDeAssignContainerNoFromHbl(AppModel).done(function (res) { }).then(function () {
                        location.reload();
                    });
                }
            })
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

    static AddHblContainerTr(containerNo, containerIdValue, hblIdValue) {
        var htrHtml = "<tr id='htr_" + htrindex + "'><input name='OceanExportHblContainer[" + htrindex + "].ContainerId' type='hidden' value='" + containerIdValue + "' /><input name='OceanExportHblContainer[" + htrindex + "].Id' type='hidden' value='" + hblIdValue + "' /><td style='align-items:center'><input type='radio' name='SurplusType' id='SurplusType_" + htrindex + "' /></td>";
        htrHtml += "<td><input name='OceanExportHblContainer[" + htrindex + "].ContainerNo' id='OceanExportHbl_PackageNo_" + htrindex + "' type='text' class='form-control' value='" + containerNo + "' readonly/></td>";
        htrHtml += "<td><input type='text' class='form-control' id='oceanExportHbl_PackageType_" + htrindex + "' onkeyup='countPackageType('HBL')' value='' /></td>";
        htrHtml += "<td><input name='OceanExportHblContainer[" + htrindex + "].PackageWeight' type='text' class='form-control' onkeyup='countTotal('HBL')' value='' /></td>";
        htrHtml += "<td><input name='OceanExportHblContainer[" + htrindex + "].PackageMeasurement' type='text' class='form-control' onkeyup='countTotalVolume('HBL')' value='' /></td>";
        htrHtml += "<td></td></tr>";
        $('#htrtbody').append(htrHtml);

        htrindex++;
    }
}