$(function () {
    var url = new URL(window.location.href);
    var selectedHblNo;
    var isHawbLocked;
    var id = url.searchParams.get('Id');
    var HawbCopy = url.searchParams.get('CopyHawb');
    if (id) {
        dolphin.freight.importExport.airExports.airExportHawb.getHblCardsById(id)
            .done(function (hblCards) {
                if (hblCards && hblCards.length) {
                    if (HawbCopy == "AllHAWB" || HawbCopy == null) {
                        hblCards.forEach(function (hblCard, index) {

                            let abpcard = createHawbCard();

                            abpcard = setHawbCardValues(abpcard, hblCard.id, hblCard.hawbNo, index);

                            $('#hblCards').append(abpcard);

                            if (hblCard.id == url.searchParams.get('Hid')) {
                                selectedHblNo = hblCard.hawbNo;
                                isHawbLocked = hblCard.isLocked;
                            }

                        })
                    }
                    else {
                        let abpcard = createHawbCard();

                        abpcard = setHawbCardValues(abpcard, hblCards[0].id, hblCards[0].hawbNo, 0);

                        $('#hblCards').append(abpcard);

                        if (hblCards[0].id == url.searchParams.get('Hid')) {
                            selectedHblNo = hblCards[0].hawbNo;
                            isHawbLocked = hblCards[0].isLocked;
                        } }
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
