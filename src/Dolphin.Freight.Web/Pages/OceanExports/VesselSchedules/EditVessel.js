﻿//(function () {
//    var url = new URL(window.location.href);
//    var selectedHblNo;

//    dolphin.freight.importExport.oceanExports.oceanExportHbl.getHblCardsById(url.searchParams.get('Id'))
//        .done(function (hblCards) {
//            if (hblCards && hblCards.length) {
//                hblCards.forEach(function (hblCard, index) {

//                    let abpcard = createHawbCard();

//                    abpcard = setHawbCardValues(abpcard, hblCard.id, hblCard.hblNo, index);

//                    $('#hblCards').append(abpcard);

//                    if (hblCard.id == url.searchParams.get('Hid')) {
//                        selectedHblNo = hblCard.hblNo;
//                    }

//                })
//                setTimeout(() => {
//                    if (selectedHblNo) {
//                        $('#title_' + selectedHblNo).click();
//                    }
//                    else { $('.hblCardTitle')[0].click(); }

//                }, 500);
//            }
//        })

//})