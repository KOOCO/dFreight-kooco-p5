$(document).on('change', '[name=PackagingUnit]', function () {
    $('#totalPackageTypeUnit').text($('#PackagingUnit').find('option:selected').text());
});

$("#saveBtn").click(function () {
    $("#mPoNo").val($("#PoNoTag").tagsStr());

    EditModel2.SaveHBLContainer();

    $("#edit2Form").submit();
});

function getHblCheckbox(mblId, index, callback) {
    dolphin.freight.importExport.oceanImports.oceanImportHbl.getHblCardsById(mblId).done(function (res) {
        let checkboxesHTML = '';
        let headersHTML = '';
        var tdindex = 0;
        for (let hbl of res) {
            checkboxesHTML += `<td style='display: none;'><input type='checkbox' data-id='${hbl.id}' data-containerNo='' id='assignContainerCheckbox_${index}_${tdindex}' onclick='EditModel2.SingleHBLContainer(this)' style='cursor: pointer;'></td>`;
            headersHTML += `<th style="text-align: center; display: none;"><div style="background-color: ${hbl.cardColorValue}; width: 10px; height: 10px; border-radius: 50%; margin: 0 auto;"></div><input type="checkbox" id="hblHeaders_${hbl.hblNo}" style="cursor: pointer; margin-top: 5px;"></th>`
            tdindex++;
        }
        callback(checkboxesHTML, headersHTML);
    });
}

class EditModel2 {
    static showHideHBLCheckboxes() {
        if (!$('input[id^="hblHeaders_"]').parent().is(":visible")) {
            $('input[id^="hblHeaders_"]').parent().show();
        } else {
            $('input[id^="hblHeaders_"]').parent().hide();
        }

        if (!$('input[id^="assignContainerCheckbox_"]').parent().is(":visible")) {
            $('input[id^="selectAllAssignContainerCheckbox_"]').parent().show();
            $('input[id^="assignContainerCheckbox_"]').parent().show();
        } else {
            $('input[id^="selectAllAssignContainerCheckbox_"]').parent().hide();
            $('input[id^="assignContainerCheckbox_"]').parent().hide();
        }
    }

    static SaveHBLContainer() {
        if ($('input[id^="assignContainerCheckbox_"]:checked').length > 0 && $('input[id^="assignContainerCheckbox_"]:checked').is(":visible")) {
            var ids = [];
            var containers = [];
            $('input[id^="assignContainerCheckbox_"]:checked').each(function (i, e) {
                var id = e.attributes[1].value;
                var container = e.attributes[2].value;
                ids.push(id);
                containers.push(container);
            });

            var AppModel = { Ids: ids, Containers: containers };
            dolphin.freight.importExport.oceanImports.oceanImportHbl.saveAssignContainerToHbl(AppModel).done(function (res) {
                debugger;
            });
        }
    }

    static openPopUp() {
        $('#CreateModal').modal('show');
    }
}

