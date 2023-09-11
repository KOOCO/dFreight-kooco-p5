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