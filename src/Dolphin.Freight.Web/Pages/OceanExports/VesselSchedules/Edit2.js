var l = abp.localization.getResource('Freight');
$(function () {
    var url = new URL(window.location.href);

    dolphin.freight.importExport.oceanExports.exportBookings.exportBooking.getBookingCardsById(url.searchParams.get('Id')).done(function (bookingCards) {
        if (bookingCards && bookingCards.length) {
            bookingCards.forEach(function (bookingCard, index) {

                let abpcard = createBookingCard();

                abpcard = setBookingCardValues(abpcard, bookingCard.id, bookingCard.soNo, index);

                $('#bookingCards').append(abpcard);
            })
            setTimeout(() => {
                $('.bookingCardTitle')[0].click();
            }, 500);
        }
    });
});

function createBookingCard() {
    $('#cardSettingArea').show();

    var check = 'false';
    if ($('#cardCheck').prop('checked')) {
        check = 'True';
    }

    let abpCard = '<div class="{{divClass}}"><div class="card-header pointer"><h6 class="card-title bookingCardTitle" id="title_{{bookingNo}}"' +
        ' onclick="onBookingCardClick(event,{{bookingCardId}})">';

    if (check == 'false') {
        abpCard += '<i class="fa fa-info-circle" data-toggle="tooltip" data-placement="top" data-html="true" title="{{hblCosigneeName}} {{hblShipperName}}"></i>{{bookingNo}}</h6>'
    } else {
        abpCard += '{{bookingNo}}<br>{{hblCosigneeName}}<br>{{hblShipperName}}</h6>'
    }

    abpCard += '<button type="button" class="btn d-none btn-collapse collapsed btnBookingCardCollapse" data-val="{{index}}" data-bs-toggle="collapse" id="btnBookingCardCollapse_{{bookingNo}}" data-bs-target="#{{cardBodyId}}" aria-expanded="false" aria-controls="mawbDiv">' +
               '<i class="fa fa-caret-down" style="color:#FFFFFF;"></i></button > </div><div class="card-body collapse" id="{{cardBodyId}}" style="min-height:150px !important"></div>';

    return abpCard;
}

function setBookingCardValues(abpcard, cardId, bookingNo, index, hblCosigneeName, hblShipperName) {
    let apbCardClass = 'card board-item mb-3 booking-sm-' + index;
    let cardBodyId = 'bookingCard_' + index;
    cardId = "'" + cardId + "'";

    abpcard = abpcard.replaceAll("{{divClass}}", apbCardClass);
    abpcard = abpcard.replaceAll("{{bookingNo}}", bookingNo);
    abpcard = abpcard.replaceAll("{{bookingCardId}}", cardId);
    abpcard = abpcard.replaceAll("{{index}}", index);
    abpcard = abpcard.replaceAll("{{cardBodyId}}", cardBodyId);
    if (hblCosigneeName !== null && hblCosigneeName !== undefined) {
        abpcard = abpcard.replaceAll("{{hblCosigneeName}}", hblCosigneeName);
    } else {
        abpcard = abpcard.replaceAll("{{hblCosigneeName}}", "");
    }

    if (hblShipperName != null && hblShipperName !== undefined) {
        abpcard = abpcard.replaceAll("{{hblShipperName}}", hblShipperName);
    } else {
        abpcard = abpcard.replaceAll("{{hblShipperName}}", "");
    }

    return abpcard;
}

function onBookingCardClick(e, bookingId) {
    var element = e.currentTarget || e.target || e.srcElement;
    var bookingNo = element.id.split('_')[1];

    if ($('#btnBookingCardCollapse_0').length && bookingNo != 0) {
        BOOKINGNO = bookingNo;
        showAlert(alertConfirmation, 'Continue');
    }
    else {
        var color = getComputedStyle(element.parentElement.parentElement).backgroundColor;

        if (!$('#btnBookingCardCollapse_' + bookingNo).attr('class').includes('collapsed')) {
            $('.bookingCardBookingNo').addClass('d-none');
            $('#bookCard_' + bookingNo).removeClass('d-none');
        }
        else {

            var ajaxParams = undefined;

            Swal.fire({
                title: 'Loading...',
                didOpen: () => {
                    Swal.showLoading()
                }
            })

            abp.ajax($.extend(true, {
                url: abp.appPath + 'api/ImportExport/ExportBookingAccounting?Id=' + bookingId,
                type: 'GET',
                dataType: "html",
            }, ajaxParams)).done(function (partial) {
                $('#partialContainer').html('');
                $('#partialContainer').html(partial);
                $('#bookCard').find('.card-header').attr('style', 'background:' + color + ';color:white')
                Swal.clickCancel();
                //if (isLocked == 'True') abp.message.warn(l("Message:LockMessage"));
            })

            $('.btnBookingCardCollapse').each(function (i, e) {
                let tempBookingNo = e.id.split('_')[1];
                if (tempBookingNo == bookingNo) {
                    $('#btnBookingCardCollapse_' + bookingNo).click();
                }
                else {
                    $('#btnBookingCardCollapse_' + tempBookingNo).addClass('collapsed');
                    $('#btnBookingCardCollapse_' + tempBookingNo).attr('aria-expanded', 'false');
                    $('#bookingCard_' + i).removeClass('show');
                }
            })
        }
    }
}

function showAlert(funName, buttonName) {
    Swal.fire({
        icon: 'error',
        text: 'The data has not been saved, are you sure you want to ' + buttonName.toLowerCase() + '?',
        showCancelButton: true,
        confirmButtonText: buttonName,
    }).then(funName)
}

function alertConfirmation(result) {
    if (result.isConfirmed) {
        let indx = $('.booking_sm_area').find('.card').length;
        $('#btnBookingCardCollapse_0').parent().parent().remove();
        if (BOOKINGNO) {
            $('#title_' + BOOKINGNO).click()
        }
        else {
            $('#bookingCard_' + (indx - 2)).parent().find('.card-title').click();
        }
    }
}

function alertConfirmation2() {
    $('#saveBtn').click();
}

function gotoBookingInvoice(BookingId, InvoiceType) {
    location.href = '/Accounting/Invoice?MethodType=4&Bid=' + BookingId + "&InvoiceType=" + InvoiceType;
}

function cardSettingClick() {
    $("#checkModal").modal("show");
}

function closeModal() {
    $("#checkModal").modal("hide");
}

const toggleLink = document.getElementById("toggleLink");

let isAsc = true;
let selectedValue = 1;

toggleLink.addEventListener("click", function (e) {
    e.preventDefault();

    if (isAsc) {
        toggleLink.innerHTML = '<i class="fa fa-sort-down"></i> Desc';
    } else {
        toggleLink.innerHTML = '<i class="fa fa-sort-up"></i> Asc';
    }

    isAsc = !isAsc;
    SortHblCard();
});

function changeSetting() {
    $("#checkModal").modal("hide");
    SortHblCard();
}

function SortHblCard() {
    selectedValue = $('#sortType').val();

    var url = new URL(window.location.href);
    var selectedHblNo;

    dolphin.freight.importExport.oceanExports.exportBookings.exportBooking.getBookingCardsById(url.searchParams.get('Id'), isAsc, selectedValue).done(function (bookingCard) {
        if (bookingCard && bookingCard.length) {
            $('#bookingCards').empty();
            bookingCard.forEach(function (bookingCard, index) {

                let abpcard = createBookingCard();

                abpcard = setBookingCardValues(abpcard, bookingCard.id, bookingCard.soNo, index, bookingCard.hblConsigneeName, bookingCard.hblShipperName);

                $('#bookingCards').append(abpcard);

                if (bookingCard.id == url.searchParams.get('Hid')) {
                    selectedHblNo = bookingCard.hblNo;
                }

            })
            setTimeout(() => {
                if (selectedHblNo) {
                    $('#title_' + selectedHblNo).click();
                }
                else { $('.bookingCardTitle')[0].click(); }

            }, 500);
        }
    })
}