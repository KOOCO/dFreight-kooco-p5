$(function () {
    var url = new URL(window.location.href);
    var from = url.searchParams.get('from');
   
    dolphin.freight.importExport.oceanImports.oceanImportHbl.getHblCardsById(url.searchParams.get('Id'))
        .done(function (hblCards) {
            $('#cardSettingArea').hide();
            if (hblCards && hblCards.length) {
               
                hblCards.forEach(function (hblCard, index) {

                    let abpcard = createHawbCard();

                    abpcard = setHawbCardValues(abpcard, hblCard.id, hblCard.hblNo, index, hblCard.hblConsigneeName, hblCard.hblShipperName, hblCard.arBalance, hblCard.apBalance);

                    $('#hblCards').append(abpcard);
                    if (hblCard.id == url.searchParams.get('Hid')) {
                        selectedHblNo = hblCard.hblNo;
                    }
                });
                setTimeout(() => {
                    if (from === 'NewHbl') {
                        $('#addHBtn').click();
                    } else {
                        $('.hblCardTitle')[0].click();
                    }
                }, 500);
            } else if (from === 'NewHbl') {
                $('#addHBtn').click();
            }
        });
})

var EditModelScript = {
    lockUnlockMblOrHbl: function (id, oceanImport) {

        switch (oceanImport) {
            case 'Mbl':
                var isLock = $('#lockButtonMbl').find('i').hasClass('fa-lock');

                abp.message.confirm(l(isLock ? 'LockConfirmationMessage' : 'UnlockConfirmationMessage')).then(function (confirmed) {
                    if (confirmed) {
                        Swal.fire({
                            title: 'Loading...',
                            didOpen: () => {
                                Swal.showLoading()
                            }
                        });

                        dolphin.freight.importExport.oceanImports.oceanImportMbl.lockedOrUnLockedOceanImportMbl({ MbId: id }).done(function () {
                            if (isLock) {
                                $('#lockButtonMbl').empty().html('<i class="fa fa-lock-open"></i> ' + l("Btn:UnLock"));
                                $('#ActionDropMbl').empty().html('<i class="fa fa-lock"></i> ' + l("Display:Function"));
                                $('#ActionDropHbl').empty().html('<i class="fa fa-lock"></i> ' + l("Display:Function"));
                                $('#lockButtonHbl').empty().html('<i class="fa fa-lock-open"></i> ' + l("Btn:Lock"));
                                abp.message.success(l('Message:SuccessLock'));
                            } else {
                                $('#lockButtonMbl').empty().html('<i class="fa fa-lock"></i> ' + l("Btn:Lock"));
                                $('#ActionDropMbl').empty().html('<i class="fa fa-lock-open"></i> ' + l("Display:Function"));
                                $('#ActionDropHbl').empty().html('<i class="fa fa-lock-open"></i> ' + l("Display:Function"));
                                $('#lockButtonHbl').empty().html('<i class="fa fa-lock"></i> ' + l("Btn:UnLock"));
                                abp.message.success(l('Message:SuccessUnlock'));
                            }
                        });
                    }
                });
                break;

            case 'Hbl':
                var isLock = $('#lockButtonHbl').find('i').hasClass('fa-lock');

                abp.message.confirm(l(isLock ? 'LockConfirmationMessage' : 'UnlockConfirmationMessage')).then(function (confirmed) {
                    if (confirmed) {
                        Swal.fire({
                            title: 'Loading...',
                            didOpen: () => {
                                Swal.showLoading()
                            }
                        });

                        dolphin.freight.importExport.oceanImports.oceanImportHbl.lockedOrUnLockedOceanImportHbl({ HblId: id }).done(function () {
                            if (isLock) {
                                $('#ActionDropHbl').empty().html('<i class="fa fa-lock"></i> ' + l("Display:Function"));
                                $('#lockButtonHbl').empty().html('<i class="fa fa-lock-open"></i> ' + l("Btn:UnLock"));
                                abp.message.success(l('Message:SuccessLock'));
                            } else {
                                $('#ActionDropHbl').empty().html('<i class="fa fa-lock-open"></i> ' + l("Display:Function"));
                                $('#lockButtonHbl').empty().html('<i class="fa fa-lock"></i> ' + l("Btn:Lock"));
                                abp.message.success(l('Message:SuccessUnlock'));
                            }
                        });
                    }
                });
                break;
        }
    },

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