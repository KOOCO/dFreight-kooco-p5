$(function () {
    var url = new URL(window.location.href);

    dolphin.freight.importExport.oceanImports.oceanImportHbl.getHblCardsById(url.searchParams.get('Id'))
           .done(function (hblCards) {
                if (hblCards && hblCards.length) {
                    hblCards.forEach(function (hblCard, index) {

                        let abpcard = createHawbCard();

                        abpcard = setHawbCardValues(abpcard, hblCard.id, hblCard.hblNo, index);

                        $('#hblCards').append(abpcard);

                    })
                    setTimeout(() => {
                        var from = url.searchParams.get('from');
                        if (from === 'NewHbl') {
                            $('#addHBtn').click();
                        } else {
                            $('.hblCardTitle')[0].click();
                        }
                    }, 500);
                }
            })

})