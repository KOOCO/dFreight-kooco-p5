$(function () {
    var url = new URL(window.location.href);
    var selectedHblNo;
   

    dolphin.freight.importExport.airImports.airImportHawb.getHawbCardsByMawbId(url.searchParams.get('Id'))
        .done(function (hblCards) {
            if (hblCards && hblCards.length) {
                hblCards.forEach(function (hblCard, index) {
                   

                    let abpcard = createHawbCard();

                    abpcard = setHawbCardValues(abpcard, hblCard.id, hblCard.hawbNo, index);

                    $('#hblCards').append(abpcard);

                    if (hblCard.id == url.searchParams.get('Hid')) {
                        selectedHblNo = hblCard.hawbNo;
                    }

                })
                setTimeout(() => {
                    debugger
                    var from = url.searchParams.get('from');
                    if (from == 'NewHawb') {
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

})