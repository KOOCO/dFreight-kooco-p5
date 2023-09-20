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
    let abpCard = '<div class="{{divClass}}"><div class="card-header pointer"><h5 class="card-title bookingCardTitle" id="title_{{bookingNo}}"' +
        ' onclick="onBookingCardClick(event,{{bookingCardId}})">{{bookingNo}}</h5>' +
        ' <button type="button" class="btn d-none btn-collapse collapsed btnBookingCardCollapse" data-val="{{index}}" data-bs-toggle="collapse" id="btnBookingCardCollapse_{{bookingNo}}" data-bs-target="#{{cardBodyId}}" aria-expanded="false" aria-controls="mawbDiv">' +
        ' <i class="fa fa-caret-down" style="color:#FFFFFF;"></i></button > </div><div class="card-body collapse" id="{{cardBodyId}}" style="min-height:150px !important"></div>';
    return abpCard;
}

function setBookingCardValues(abpcard, cardId, bookingNo, index) {
    let apbCardClass = 'card board-item booking-sm-' + index;
    let cardBodyId = 'bookingCard_' + index;
    cardId = "'" + cardId + "'";

    abpcard = abpcard.replaceAll("{{divClass}}", apbCardClass);
    abpcard = abpcard.replaceAll("{{bookingNo}}", bookingNo);
    abpcard = abpcard.replaceAll("{{bookingCardId}}", cardId);
    abpcard = abpcard.replaceAll("{{index}}", index);
    abpcard = abpcard.replaceAll("{{cardBodyId}}", cardBodyId);

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
                setTimeout(() => {
                    var l = abp.localization.getResource('Freight');

                    $('#ExportBooking_SoNoDate').datetimepicker({
                        format: 'Y-m-d H:i',
                        step: 15,
                        allowInput: false,
                        value: new Date()
                    });

                    const ids = [
                        'ExportBookingDto_SoNoDate',
                        'ExportBookingDto_PorEtd',
                        'ExportBookingDto_PodEta',
                        'ExportBookingDto_DelEta',
                        'ExportBookingDto_FdestEta',
                        'ExportBookingDto_WhCutOffTime',
                        'ExportBookingDto_DocCutOffTime',
                        'ExportBookingDto_PortCutOffTime',
                        'ExportBookingDto_VgmCutOffTime',
                        'ExportBookingDto_RailCutOffTime',
                        'ExportBookingDto_EarlyReturnDatetime',
                        'ExportBookingDto_Trans1Eta'
                    ];

                    ids.forEach(function (id) {
                        let dateElem = $('#' + id);

                        if (dateElem.length === 0) {
                            return;
                        }

                        dateElem.removeAttr('type').datetimepicker({
                            format: 'Y-m-d H:i',
                            step: 15,
                            allowInput: false
                        });

                        let currentVal = dateElem.val();
                        if (currentVal.includes('T')) {
                            dateElem.val(currentVal.replace('T', ' '));
                        }
                    });

                    initializeDropdownSearch("ExportBookingDto_CancelReason");
                    initializeDropdownSearch("ExportBookingDto_ReferenceId");
                    initializeDropdownSearch("ExportBookingDto_CarrierId");
                    initializeDropdownSearch("ExportBookingDto_ShipModeId");
                    initializeDropdownSearch("ExportBookingDto_FreightTermForBuyerId");
                    initializeDropdownSearch("ExportBookingDto_FreightTermForSalerId");
                    initializeDropdownSearch("ExportBookingDto_SvcTermFromId");
                    initializeDropdownSearch("ExportBookingDto_SvcTermToId");
                    initializeDropdownSearch("ExportBookingDto_IncotermsId");
                    initializeDropdownSearch("ExportBookingDto_CargoTypeId");
                    initializeDropdownSearch("ExportBookingDto_ShipperId");
                    initializeDropdownSearch("ExportBookingDto_CustomerId");
                    initializeDropdownSearch("ExportBookingDto_BillToId");
                    initializeDropdownSearch("ExportBookingDto_ConsigneeId");
                    initializeDropdownSearch("ExportBookingDto_NotifyId");
                    initializeDropdownSearch("ExportBookingDto_ShippingAgentId");
                    initializeDropdownSearch("ExportBookingDto_HblAgentId");
                    initializeDropdownSearch("ExportBookingDto_CoLoaderId");
                    initializeDropdownSearch("ExportBookingDto_FbaId");
                    initializeDropdownSearch("ExportBookingDto_CargoPickupId");
                    initializeDropdownSearch("ExportBookingDto_TruckerId");
                    initializeDropdownSearch("ExportBookingDto_DeliveryToId");
                    initializeDropdownSearch("ExportBookingDto_DelId");
                    initializeDropdownSearch("ExportBookingDto_SalespersonId");
                    initializeDropdownSearch("ExportBookingDto_PorId");
                    initializeDropdownSearch("ExportBookingDto_PolId");
                    initializeDropdownSearch("ExportBookingDto_PodId");
                    initializeDropdownSearch("ExportBookingDto_FdestId");
                    initializeDropdownSearch("ExportBookingDto_TransPort1Id");
                    initializeDropdownSearch("ExportBookingDto_OfficeId");
                    initializeDropdownSearch("ExportBookingDto_ReferralById");
                    initializeDropdownSearch("ExportBookingDto_EmptyPickupId");

                    $("#checkHideBtn").click(function () {
                        initHideBtn();
                    });
                    $("#ExportBooking_IsCanceled").change(function () {
                        initReasonStatus()
                    });
                    $('#ExportBooking_IsCreateBySystem').on('click', function (res) {
                        var l = abp.localization.getResource('Freight');
                        if (res.currentTarget.checked) {
                            abp.message.confirm(l('SystemBookingNoConfirm')).then(function (confirmed) {
                                if (confirmed) {
                                    dolphin.freight.settings.sysCodes.sysCode.getSystemBookingNo({ queryType: 'ExportBooking_SoNo' }).done(function (rs) {
                                        $('#ExportBooking_SoNo').val(rs);
                                    });
                                }
                            });
                        }
                    })

                    onSetCommodity();
                    setPoValue();
                    initHideBtn();
                    initRemark(0);
                    initReasonStatus();

                    dolphin.freight.importExport.oceanExports.exportBookings.exportBooking.getBookingCardById($('#ExportBookingDto_Id').val()).done(function (res) {
                        var sono = res.soNo;
                        $('#ExportBooking_SoNo').val(sono);
                    });
                }, 500);
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