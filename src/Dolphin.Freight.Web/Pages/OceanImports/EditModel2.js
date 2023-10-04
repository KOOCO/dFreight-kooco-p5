$(document).on('change', '[name=PackagingUnit]', function () {
    $('#totalPackageTypeUnit').text($('#PackagingUnit').find('option:selected').text());
});

function getHblCheckbox(mblId, index, callback) {
    dolphin.freight.importExport.oceanImports.oceanImportHbl.getHblCardsById(mblId).done(function (res) {
        let checkboxesHTML = '';
        let headersHTML = '';
        for (let hbl of res) {
            checkboxesHTML += `<td><input type='checkbox' id='assignContainerCheckbox_${index}' style='cursor: pointer;'></td>`;
            headersHTML += `<th style="text-align: center;"><div style="background-color: ${hbl.cardColorValue}; width: 10px; height: 10px; border-radius: 50%; margin: 0 auto;"></div><input type="checkbox" id="hblHeaders_${hbl.hblNo}" style="cursor: pointer; margin-top: 5px;"></th>`
        }
        callback(checkboxesHTML, headersHTML);
    });
}

class EditModel2 {
    static showHideHBLCheckboxes() {
        debugger;
    }
}

