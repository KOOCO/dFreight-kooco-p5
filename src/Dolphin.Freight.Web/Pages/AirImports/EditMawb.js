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

        ids.forEach(function (id) {
            let dateElem = $('#' + id);

            if (dateElem.length === 0) {
                return;
            }

            dateElem.removeAttr('type').datetimepicker({
                format: 'Y-m-d H:i',
                step: 15,
                allowInput: false
            });

            let currentVal = dateElem.val();
            if (currentVal.includes('T')) {
                dateElem.val(currentVal.replace('T', ' '));
            }
        });
    });

    var url = new URL(window.location.href);
    var selectedHblNo;
   

    dolphin.freight.importExport.airImports.airImportHawb.getHawbCardsByMawbId(url.searchParams.get('Id'))
        .done(function (hblCards) {
            if (hblCards && hblCards.length) {
                hblCards.forEach(function (hblCard, index) {
                   
                    debugger;
                    let abpcard = createHawbCard();

                    abpcard = setHawbCardValues(abpcard, hblCard.id, hblCard.hawbNo, index);

                    $('#hblCards').append(abpcard);

                    debugger;
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

