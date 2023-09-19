$(function () {
    var url = new URL(window.location.href);

    dolphin.freight.importExport.airExports.airExportHawb.getHblCardsById(url.searchParams.get('Id')).done(function (hblCards) {
        if (hblCards && hblCards.length) {
            hblCards.forEach(function (hblCard, index) {
                let abpcard = createHawbCard();

                abpcard = setHawbCardValues(abpcard, hblCard.id, hblCard.hawbNo, index);

                $('#hblCardDiv').append(abpcard);
            });
            setTimeout(() => {
                $('.hblCardTitle')[0].click();
            }, 500);
        }
    });
});