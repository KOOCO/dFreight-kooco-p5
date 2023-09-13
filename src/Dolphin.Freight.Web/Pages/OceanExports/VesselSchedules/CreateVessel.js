var l = abp.localization.getResource('Freight');
function checkCreateMbl(e) {
    if ($(e.currentTarget).prop('checked') || $(e.target).prop('checked') || $(e.srcElement).prop('checked')) {
        $('#createMblBtn').attr('disabled', false);
    } else {
        var checkedCheckboxes = $('#trtbody input[type="checkbox"]:checked');
        if (checkedCheckboxes && checkedCheckboxes.length == 0) {
            $('#createMblBtn').attr('disabled', true);
        }
    }
}

function createOneMbl() {
    var ids = [];
    var selectedCheckboxes = $('#trtbody input[type="checkbox"]:checked');
    for (var i = 0; i < selectedCheckboxes.length; i++) {
        var id = selectedCheckboxes[i].attributes[1].value;
        ids.push(id);
    }

    for (var i = 0; i < ids.length; i++) {
        if (!ids[i]) {
            abp.message.confirm(l('Message:WarningCreateMblForContainers')).then(function (confirmed) {
                if (confirmed) {
                    $('#saveBtn').click();
                }
                return;
            });
            return;
        }
    }
}

function createOneMblforEachBooking() {
    var ids = [];
    var selectedCheckboxes = $('#trtbody input[type="checkbox"]:checked');
    for (var i = 0; i < selectedCheckboxes.length; i++) {
        var id = selectedCheckboxes[i].attributes[1].value;
        ids.push(id);
    }

    for (var i = 0; i < ids.length; i++) {
        if (!ids[i]) {
            abp.message.confirm(l('Message:WarningCreateMblForContainers')).then(function (confirmed) {
                if (confirmed) {
                    $('#saveBtn').click();
                }
                return;
            });
            return;
        }
    }
}