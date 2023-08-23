$(function () {
    var url = new URL(window.location.href);
    var selectedHblNo;
    var isHawbLocked;
    var id = url.searchParams.get('Id');
    if (id) {
        dolphin.freight.importExport.airExports.airExportHawb.getHblCardsById(id)
            .done(function (hblCards) {
                if (hblCards && hblCards.length) {
                    hblCards.forEach(function (hblCard, index) {


                        let abpcard = createHawbCard();

                        abpcard = setHawbCardValues(abpcard, hblCard.id, hblCard.hawbNo, index);

                        $('#hblCards').append(abpcard);

                        if (hblCard.id == url.searchParams.get('Hid')) {
                            selectedHblNo = hblCard.hawbNo;
                            isHawbLocked = hblCard.isLocked;
                            debugger;
                        }

                    })
                    setTimeout(() => {
                        var from = url.searchParams.get('from');
                        if (from == 'accounting') {
                            $('#addHBtn').click();
                        }
                        else if (selectedHblNo) {
                            $('#title_' + selectedHblNo).click();
                        }
                        else {
                            $('.hblCardTitle')[0].click();
                        }
                    }, 500);
                }
            })
    }
})