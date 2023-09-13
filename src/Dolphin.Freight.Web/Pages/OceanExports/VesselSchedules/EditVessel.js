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
    abp.message.confirm(l('Message:CreateOneMBLConfirmation')).then(function (confirmed) {
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
    debugger;
    if ($(e.currentTarget).prop('checked') || $(e.target).prop('checked') || $(e.srcElement).prop('checked')) {
        $('#createMblBtn').attr('disabled', false);

        $("[id^='f0']").each(function (i, elem) {
            var dataId = $(elem).data('id');

            if ($(this).prop('checked') && !(dataId && dataId != undefined && dataId != '')) {
                $('#createMblBtn').attr('disabled', true);
                return;
            }
        });
    } else{
        var checkedCheckboxes = $('#trtbody input[type="checkbox"]:checked');
        if (checkedCheckboxes && checkedCheckboxes.length == 0) {
            $('#createMblBtn').attr('disabled', true);
        }
    }
}