$(function () {
    var url = new URL(window.location.href);

    dolphin.freight.importExport.oceanImports.oceanImportHbl.getHblCardsById(url.searchParams.get('Id'))
        .done(function (hblCards) {
            if (hblCards && hblCards.length) {
                hblCards.forEach(function (hblCard, index) {

                    let abpcard = createHawbCard();

                    abpcard = setHawbCardValues(abpcard, hblCard.id, hblCard.hblNo, index, hblCard.hblConsigneeName, hblCard.hblShipperName, hblCard.arBalance, hblCard.apBalance);

                    $('#hblCards').append(abpcard);
                    if (hblCard.id == url.searchParams.get('Hid')) {
                        selectedHblNo = hblCard.hblNo;
                    }
                })
                setTimeout(() => {
                    $('.hblCardTitle')[0].click();
                }, 500);
            }
        });
});