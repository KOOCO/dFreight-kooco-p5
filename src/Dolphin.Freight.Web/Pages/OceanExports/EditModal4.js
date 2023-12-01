$(document).ready(function () {
    debugger;
    var url = new URL(window.location.href);
    var selectedindex = 0;
    dolphin.freight.importExport.oceanExports.oceanExportHbl.getHblCardsById(url.searchParams.get('Id'))
        .done(function (hblCards) {
            if (hblCards && hblCards.length) {
                hblCards.forEach(function (hblCard, index) {

                    let abpcard = createHawbCard();
                    debugger;
                    abpcard = setHawbCardValues(abpcard, hblCard.id, hblCard.hblNo, index, hblCard.hblConsigneeName, hblCard.hblShipperName, hblCard.weight, hblCard.measurement, hblCard.packageType);

                    $('#hblCards').append(abpcard);

                    if (hblCard.id == url.searchParams.get('Hid')) {
                        selectedHblNo = hblCard.hblNo;
                        selectedindex = index;
                    }

                })
                setTimeout(() => {
                    /*    $('.hblCardTitle')[selectedindex].click();*/
                    $('#btnHawbCardCollapse_' + selectedHblNo).click();
                }, 500);
            }
        })
});