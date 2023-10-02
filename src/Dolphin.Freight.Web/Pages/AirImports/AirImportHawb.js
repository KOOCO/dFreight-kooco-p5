$(document).ready(function () {
    const ids = [
        "HawbModel_FinalETA",
        "HawbModel_LastFreeDay",
        "HawbModel_StorageStartDate",
        "HawbModel_ITDate",
        "HawbModel_FrtRelease",
        "HawbModel_CReleasedDate",
        "HawbModel_DoorDelivered"
    ];

    var dateTimePicker = new CustomDateTimePicker();
    dateTimePicker.dateTimePicker(ids, false);

    if ($('#HawbCancelledDate').val() == '0001-01-01T00:00') {
        $('#HawbCancelledDate').val('');
    }
    if ($('#HawbModel_FrtRelease').val() == '0001-01-01T00:00') {
        $('#HawbModel_FrtRelease').val('');
    }
});

class AirImportHawb {
    static OpenInTrackTrace() {
        var url = new URL(window.location.href);
        var mawbId = url.searchParams.get('Id');
        dolphin.freight.importExport.airImports.airImportMawb.get(mawbId).done(function (res) {
            myWindow = window.open('https://www.track-trace.com/bol#' + res.mawbNo, '_blank', 'width=1200,height=1000');
            myWindow.focus();
        });
    }

    static onCopyToHawbClick() {
        var id = '@Model.HawbModel.Id';
        createHawbCopyModal.open({
            id,
        });
    }

    static setHawbLockUnlock(isLockedCheck) {
        var id = '@Model.HawbModel.Id';
        isLockedCheck = isLockedCheck === "True";
        if (isLockedCheck) {
            abp.message.confirm(l('UnlockConfirmationMessage')).then(function (confirmed) {
                if (confirmed) {
                    dolphin.freight.importExport.airImports.airImportHawb
                        .lockedOrUnLockedAirImportHawb(id)
                        .then(function () {
                            abp.message.success(l('SuccessfullyUnlocked'));
                            window.location.reload();
                        });
                }
            });
        } else {
            abp.message.confirm(l('LockConfirmationMessage')).then(function (confirmed) {
                if (confirmed) {
                    dolphin.freight.importExport.airImports.airImportHawb
                        .lockedOrUnLockedAirImportHawb(id)
                        .then(function () {
                            abp.message.success(l('SuccessfullyLocked'));
                            window.location.reload();
                        });
                }
            });
        }
    }

    static KgToLbConversion(Elem, Unit) {
        switch (Unit) {
            case 'KG':
                var LB = parseFloat(Elem.currentTarget.value * 2.2).toFixed(2);
                $($(Elem.currentTarget).parent().parent().next().children().children()[1]).val(LB);
                break;
            case 'LB':
                var KG = parseFloat(Elem.currentTarget.value / 2.2).toFixed(2);
                $($(Elem.currentTarget).parent().parent().prev().children().children()[1]).val(KG);
                break;
            case 'VolKG':
                var CBM = parseFloat(Elem.currentTarget.value * 0.01).toFixed(2);
                $($(Elem.currentTarget).parent().parent().next().children().children()[1]).val(CBM);
                break;
            case 'CBM':
                var VolKG = parseFloat(Elem.currentTarget.value * 166.67).toFixed(2);
                $($(Elem.currentTarget).parent().parent().prev().children().children()[1]).val(VolKG);
                break;
        }
    }
}