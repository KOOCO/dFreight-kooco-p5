$(function () {
    var url = new URL(window.location.href);
    var selectedHblNo;

    dolphin.freight.importExport.oceanExports.oceanExportHbl.getHblCardsById(url.searchParams.get('Id'))
        .done(function (hblCards) {
            if (hblCards && hblCards.length) {
                $('#cardSettingArea').show();
                hblCards.forEach(function (hblCard, index) {

                    let abpcard = createHawbCard();

                    abpcard = setHawbCardValues(abpcard, hblCard.id, hblCard.hblNo, index, hblCard.hblConsigneeName, hblCard.hblShipperName);

                    $('#hblCards').append(abpcard);

                    if (hblCard.id == url.searchParams.get('Hid')) {
                        selectedHblNo = hblCard.hblNo;
                    }

                })
                setTimeout(() => {
                    if (selectedHblNo) {
                        $('#title_' + selectedHblNo).click();
                    }
                    else { $('.hblCardTitle')[0].click(); }
                    
                }, 500);
            }
        })

})

var EditModelOEScript = {
    RextoHexColorCode: function (rgb) {
        var result = /^rgb\((\d+),\s*(\d+),\s*(\d+)\)$/.exec(rgb);
        if (result) {
            return '#' +
                (1 << 24 | parseInt(result[1]) << 16 | parseInt(result[2]) << 8 | parseInt(result[3])).toString(16).slice(1).toUpperCase();
        } else {
            return undefined;
        }
    }
}