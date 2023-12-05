$(function () {
    var url = new URL(window.location.href);
    var selectedHblNo;

    dolphin.freight.importExport.oceanExports.oceanExportHbl.getHblCardsById(url.searchParams.get('Id'), true, 0)
        .done(function (hblCards) {
            debugger;
            if (hblCards && hblCards.length) {
                $('#cardSettingArea').show();
                hblCards.forEach(function (hblCard, index) {

                    let abpcard = createHawbCard();

                    abpcard = setHawbCardValues(abpcard, hblCard.id, hblCard.hblNo, index, hblCard.hblConsigneeName, hblCard.hblShipperName, hblCard.weight, hblCard.measurement, hblCard.packageType);

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

});

var OceanExportCopyModal = new abp.ModalManager({
    viewUrl: '/OceanExports/ModalPopups/OceanExportMBLCopy'
});

var EditModelOEScript = {
    RextoHexColorCode: function (rgb) {
        var result = /^rgba?\((\d+),\s*(\d+),\s*(\d+)(?:,\s*(\d*(?:\.\d+)?))?\)$/.exec(rgb);
        if (result) {
            var hex = '#' +
                (1 << 24 | parseInt(result[1]) << 16 | parseInt(result[2]) << 8 | parseInt(result[3])).toString(16).slice(1).toUpperCase();

            if (result[4]) {  // If alpha is provided
                var alpha = Math.round(parseFloat(result[4]) * 255);
                var alphaHex = ("00" + alpha.toString(16)).slice(-2).toUpperCase();
                hex += alphaHex;
            }

            return hex;
        } else {
            return undefined;
        }
    },
    OceanExportCopy: function (id) {
        OceanExportCopyModal.open({
            id
        });
    }
}