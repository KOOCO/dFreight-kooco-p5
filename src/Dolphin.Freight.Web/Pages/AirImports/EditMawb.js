$(function () {
    $(document).ready(function () {
        const ids = [
            "MawbModel_PostDate",
            "AIMDepatureDate",
            "MawbModel_ArrivalDate",
            "MawbModel_StorageStartDate",
            "MawbModel_RouteTrans1ArrivalDate",
            "MawbModel_RouteTrans2ArrivalDate",
            "MawbModel_RouteTrans3ArrivalDate",
            "MawbModel_RouteTrans1DepatureDate",
            "MawbModel_RouteTrans2DepatureDate",
            "MawbModel_RouteTrans3DepatureDate"
        ];

        var DateTimePicker = new CustomDateTimePicker();
        DateTimePicker.dateTimePicker(ids);
    });

    var url = new URL(window.location.href);
    var selectedHblNo;
   

    dolphin.freight.importExport.airImports.airImportHawb.getHawbCardsByMawbId(url.searchParams.get('Id'))
        .done(function (hblCards) {
            if (hblCards && hblCards.length) {
                hblCards.forEach(function (hblCard, index) {
                   
                    let abpcard = createHawbCard();

                    abpcard = setHawbCardValues(abpcard, hblCard.id, hblCard.hawbNo, index, hblCard.consigneeName, hblCard.shipperName, hblCard.grossWeightKG, hblCard.volumeWeightCBM, hblCard.package);


                    $('#hblCards').append(abpcard);

                    if (hblCard.id == url.searchParams.get('Hid')) {
                        selectedHblNo = hblCard.hawbNo;
                    }

                })
                setTimeout(() => {
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

