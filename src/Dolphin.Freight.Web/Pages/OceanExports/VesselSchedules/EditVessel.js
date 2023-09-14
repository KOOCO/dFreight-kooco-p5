var l = abp.localization.getResource('Freight');
$(function () {
    var url = new URL(window.location.href);
    var selectedBookingNo;

    dolphin.freight.importExport.oceanExports.exportBookings.exportBooking.getBookingCardsById(url.searchParams.get('Id'))
        .done(function (bookingCards) {
            if (bookingCards && bookingCards.length) {
                bookingCards.forEach(function (bookingCard, index) {

                    let abpcard = createBookingCard();

                    abpcard = setBookingCardValues(abpcard, bookingCard.id, bookingCard.soNo, index);

                    $('#bookingCards').append(abpcard);

                    if (bookingCard.id == url.searchParams.get('Hid')) {
                        selectedBookingNo = bookingCard.soNo;
                    }

                })
                setTimeout(() => {
                    if (selectedBookingNo) {
                        $('#title_' + selectedBookingNo).click();
                    }
                    else { $('.bookingCardTitle')[0].click(); }

                }, 500);
            }
        });
})
function createOneMbl() {
    var ids = [];
    var selectedCheckboxes = $('#trtbody input[type="checkbox"]:checked');
    for (var i = 0; i < selectedCheckboxes.length; i++) {
        var id = selectedCheckboxes[i].attributes[1].value;
        ids.push(id);
    }

    for (var i = 0; i < ids.length; i++) {
        if (!ids[i]) {
            abp.message.confirm(l('Message:WarningCreateMblForContainers')).then(function (confirmed) {
                if (confirmed) {
                    $('#saveBtn').click();
                }
                return;
            });
            return;
        }
    }

    abp.message.confirm(l('Message:CreateOneMBLConfirmation')).then(function (confirmed)
    {
        if (confirmed) {
            dolphin.freight.importExport.oceanExports.oceanExportMbl.createOneMBLWithContainer(ids).done(function (res) {
                var url = '/OceanExports/EditModal?Id=' + res.id;
                var linkText = res.filingNo;

                Swal.fire({
                    title: l('Message:CreatedMBLConfirmation'),
                    html: '<a href="' + url + '" target="_blank" style="font-size: 25px; float: left; margin-left: 12px;"><u>' + linkText + '</u></a>',
                    showCancelButton: true,
                    confirmButtonText: 'OK',
                    cancelButtonText: 'Cancel',
                    width: '700px'
                }).then((result) => {
                    if (result.isConfirmed) {
                        window.location.reload();
                    } else {
                        window.location.reload();
                    }
                });
            });
        }
    });
}
function createOneMblforEachBooking() {
    var ids = [];
    var selectedCheckboxes = $('#trtbody input[type="checkbox"]:checked');
    for (var i = 0; i < selectedCheckboxes.length; i++) {
        var id = selectedCheckboxes[i].attributes[1].value;
        ids.push(id);
    }

    for (var i = 0; i < ids.length; i++) {
        if (!ids[i]) {
            abp.message.confirm(l('Message:WarningCreateMblForContainers')).then(function (confirmed) {
                if (confirmed) {
                    $('#saveBtn').click();
                }
                return;
            });
            return;
        }
    }

    abp.message.confirm(l('Message:CreateOneMBLConfirmation')).then(function (confirmed) {
        if (confirmed) {
            dolphin.freight.importExport.oceanExports.oceanExportMbl.createMblWithDiffContainers(ids).done(function (res)
            {
                var linksHtml = "<ul style='list-style-type: none; padding-left: 0; text-align: left;'>";
                for (var i = 0; i < res.ids.length; i++) {
                    var url = '/OceanExports/EditModal?Id=' + res.ids[i];
                    var linkText = res.filingNos[i];
                    linksHtml += '<li><a href="' + url + '" target="_blank" style="font-size: 25px;"><u>' + linkText + '</u></a></li>';
                }
                linksHtml += "</ul>";

                Swal.fire({
                    title: l('Message:CreatedMBLConfirmation'),
                    html: linksHtml,
                    showCancelButton: true,
                    confirmButtonText: 'OK',
                    cancelButtonText: 'Cancel',
                    width: '700px'
                }).then((result) => {
                    if (result.isConfirmed) {
                        window.location.reload();
                    } else {
                        window.location.reload();
                    }
                });
            });
        }
    });
}
function checkDataId(dataId, e) {
    if ($(e.currentTarget).prop('checked') || $(e.target).prop('checked') || $(e.srcElement).prop('checked')) {
        $('#createMblBtn').attr('disabled', false);
    } else{
        var checkedCheckboxes = $('#trtbody input[type="checkbox"]:checked');
        if (checkedCheckboxes && checkedCheckboxes.length == 0) {
            $('#createMblBtn').attr('disabled', true);
        }
    }
}

var index = 0;

function getMblContainersByVesselId(id) {
    dolphin.freight.importExport.oceanExports.vesselScheduleas.vesselSchedule.getMblContainersByVesselId(id).done(function (res) {
        if (res != null && res.containerList.length > 0 && res.mblList.length > 0) {
            for (var i = 0; i < Math.min(res.containerList.length, res.mblList.length); i++) {
                var mblId = res.mblList[i] ? res.mblList[i].id : '';
                var containerId = res.containerList[i] ? res.containerList[i].id : '';
                var containerSizeId = res.containerList[i] ? res.containerList[i].containerSizeId : '';
                var value0 = res.mblList[i] ? res.mblList[i].isLocked : '';
                var value1 = res.mblList[i] ? res.mblList[i].filingNo : '';
                var value2 = res.mblList[i] ? res.mblList[i].mblNo : '';
                var value3 = res.containerList[i] ? res.containerList[i].containerNo : '';
                var value4 = res.containerList[i] ? res.containerList[i].containerSizeName : '';
                var value5 = res.containerList[i] ? res.containerList[i].sealNo : '';
                var value6 = res.containerList[i] ? res.containerList[i].creationTime.split('T')[0] : '';
                if (value2 == null) value2 = ''; if (value1 == null) value1 = ''; if (value5 == null) value5 = '';
                addShipmentTr(mblId, containerId, containerSizeId, value0, value1, value2, value3, value4, value5, value6);
            }
        }
    });
}

function addShipmentTr(mblId, containerId, containerSizeId, value0, value1, value2, value3, value4, value5, value6) {
    const containerSizeSelectHtml = getcontainersSizeSelection(containerSizeId);

    const trhtml = `
        <tr id='tr_${index}' style='background-color: #f4f4f4;'>
            <td style='text-align: center;'>${value0 ? "<i class='fa fa-lock'></i>" : ""}</td>
            <td><a href='/OceanExports/EditModal?Id=${mblId}'>${value1}</a><input type='hidden' name='CreateUpdateOceanExportMblDto[${index}].Id' id='f1_${index}' class='form-control form-control-sm' value='${mblId}' /></td>
            <td><label id='lbl2_${index}'><a href='/OceanExports/EditModal?Id=${mblId}'>${value2}</a></label><input type='text' style='display: none;' name='CreateUpdateOceanExportMblDto[${index}].MblNo' id='f2_${index}' class='form-control form-control-sm' value='${value2}' /></td>
            <td><label id='lbl3_${index}'>${value3}</label><input type='text' style="display:none;" name='ContainerDtos[${index}].ContainerNo' id='f3_${index}' class='form-control form-control-sm' value='${value3}' /></td>
            <td></td>
            <td><label id='lbl4_${index}'>${value4}</label>${containerSizeSelectHtml}</td>
            <td>${value5}<input type='text' style='display: none;' name='ContainerDtos[${index}].SealNo' id='f5_${index}' class='form-control form-control-sm' value='${value5}' /></td>
            <td>${value6}<input type='text' style='display: none;' name='ContainerDtos[${index}].Id' id='f6_${index}' class='form-control form-control-sm' value='${containerId}' /></td>
        </tr>`;

    $("#shipmentbody").append(trhtml);
    index++;
}

function getcontainersSizeSelection(selectedId) {
    var selectHtml = "<select name='ContainerDtos[" + index + "].ContainerSizeId' class='form-select form-select-sm' style='display: none;' >";
    for (var i = 0; i < containerSizes.length; i++) {
        selectHtml = selectHtml + "<option value='" + containerSizes[i].id + "' ";
        if (containerSizes[i].id == selectedId) selectHtml = selectHtml + " selected ";
        selectHtml = selectHtml + ">" + containerSizes[i].containerCode + "</option>"
    }
    selectHtml = selectHtml + "</select>";
    return selectHtml;
}

function checkStatus(value) {
    if (value === 0) {
        $('#ViewStatus').prop('checked', true);
        $('#EditStatus').prop('checked', false);
        toggleEditMode(false);
    } else if (value === 1) {
        $('#EditStatus').prop('checked', true);
        $('#ViewStatus').prop('checked', false);
        toggleEditMode(true);
    }
}

function displayHblBookingBtn(event) {
    var $parent = $(event.currentTarget).parent();

    if ($parent.text().trim() === "HB/L No") {
        $parent.hide();
        $("th:contains('Booking No.')").show();
    }
    else if ($parent.text().trim() === "Booking No.") {
        $parent.hide();
        $("th:contains('HB/L No')").show();
    }
}

function toggleEditMode(isEditMode) {
    $('#shipmentbody tr').each(function () {
        const containerNoInput = $(this).find('input[name^="ContainerDtos["][name$="].ContainerNo"]');
        const associatedLabelContainerNo = $(this).find('#lbl3_' + containerNoInput.attr('id').split('_')[1]);

        const mblNoInput = $(this).find('input[name^="CreateUpdateOceanExportMblDto["][name$="].MblNo"]');
        const associatedLabelMblNo = $(this).find('#lbl2_' + mblNoInput.attr('id').split('_')[1]);

        const containerSizeSelect = $(this).find('select[name^="ContainerDtos["][name$="].ContainerSizeId"]');
        const associatedLabelContainerSize = $(this).find('#lbl4_' + containerSizeSelect.attr('name').split('[')[1].split(']')[0]);

        if (isEditMode) {
            associatedLabelContainerNo.hide();
            containerNoInput.show();
            associatedLabelMblNo.hide();
            mblNoInput.show();
            associatedLabelContainerSize.hide();
            containerSizeSelect.show();
        } else {
            associatedLabelContainerNo.show();
            containerNoInput.hide();
            associatedLabelMblNo.show();
            mblNoInput.hide();
            associatedLabelContainerSize.show();
            containerSizeSelect.hide();
        }
    });
}


