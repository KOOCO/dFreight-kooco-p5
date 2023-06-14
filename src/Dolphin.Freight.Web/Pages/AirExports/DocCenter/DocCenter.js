$(function () {

    var urlPath = window.location.href;
    var id = urlPath.substring(urlPath.lastIndexOf("/") + 1, urlPath.length);
    dolphin.freight.importExport.airExports.airExportHawb.getDocCenterCardsById(id)
        .done(function (hblCards) {
            if (hblCards && hblCards.length) {
                hblCards.forEach(function (hblCard, index) {

                    let abpcard = createHawbCard();

                    abpcard = setHawbCardValues(abpcard, hblCard.id, hblCard.hawbNo, index);

                    $('#hblCards').append(abpcard);

                })
                setTimeout(() => {
                    $('.hblCardTitle')[0].click();
                }, 500);
            }
        })

})