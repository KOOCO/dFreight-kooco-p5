$(function () {
    var l = abp.localization.getResource('Freight');

});
function initSysCodeTag(selectItems, tagName, tagValue) {
    var drophtml = "";
    var l = abp.localization.getResource('Freight');
    var tag = l('Dropdown:Select');
    for (var i = 0; i < selectItems.length; i++) {
        drophtml = drophtml + "<li class='form-control' style='width:450px;'><a   class='dropdown-item2' style='width:400px;white-space: normal;'  href='javascript:void(0)' onclick='changeDropdownValue(\"" + tagName + "\",\"" + selectItems[i].id + "\",\"" + selectItems[i].showName + "\")'>" + selectItems[i].showName + "</div></a></li>"
        if (tagValue == selectItems[i].id) tag = selectItems[i].showName;
    }
    $("#drop_" + tagName).html(tag);
    $("#" + tagName).val(tagValue);
    $("#dropdownMenuButton_" + tagName).html(drophtml);
}
function initSysCodeI18nTag(selectItems, tagName, tagValue) {
    var drophtml = "";
    var l = abp.localization.getResource('Freight');
    var tag = l('Dropdown:Select');
    for (var i = 0; i < selectItems.length; i++) {
        drophtml = drophtml + "<li class='form-control' style='width:450px;'><a  style='width:400px;' class='dropdown-item2'  href='javascript:void(0)' onclick='changeDropdownValue(\"" + tagName + "\",\"" + selectItems[i].codeValue + "\",\"" + selectItems[i].showName + "\")'>" + l(selectItems[i].showName) + "</div></a></li>"
        if (tagValue == selectItems[i].id) tag = selectItems[i].showName;
    }
    $("#drop_" + tagName).html(tag);
    $("#" + tagName).val(tagValue);
    $("#dropdownMenuButton_" + tagName).html(drophtml);
}
function initCancelReasonTag(selectItems, tagName, tagValue) {
    var drophtml = "";
    var l = abp.localization.getResource('Freight');
    var tag = l('Dropdown:Select');
    for (var i = 0; i < selectItems.length; i++) {
        drophtml = drophtml + "<li class='form-control' style='width:450px;'><a  style='width:400px;' class='dropdown-item2'  href='javascript:void(0)' onclick='changeCancelReasionDropdownValue(\"" + tagName + "\",\"" + selectItems[i].codeValue + "\",\"" + selectItems[i].showName + "\")'>" + l(selectItems[i].showName) + "</div></a></li>"
        if (tagValue == selectItems[i].showName) tag = l(selectItems[i].showName);
    }
    $("#drop_" + tagName).html(tag);
    $("#" + tagName).val(tagValue);
    $("#dropdownMenuButton_" + tagName).html(drophtml);
}
function initReferenceTag(selectItems, tagName, tagValue) {
        var drophtml = "";
        var l = abp.localization.getResource('Freight');
        var tag = l('Dropdown:Select');
        for (var i = 0; i < selectItems.length; i++) {
            drophtml = drophtml + "<li class='form-control' style='width:450px; word-break:break-all;'><a  style='width:400px;word-break:break-all;' class='dropdown-item2'  href='javascript:void(0)' onclick='changeDropdownValue(\"" + tagName + "\",\"" + selectItems[i].id + "\",\"" + selectItems[i].referenceNo + "\")'><div style='word-break:break-all;'>" + selectItems[i].referenceNo + "(";

            if (selectItems[i].referenceType == 0) drophtml = drophtml + l("VesselSchedule");
            else drophtml = drophtml + "MBL " + selectItems[i].mblNo
            drophtml = drophtml + ")<br/>";
            if (selectItems[i].polName != null && selectItems[i].polName != "") drophtml = drophtml + selectItems[i].polName +" \\ ";
            if (selectItems[i].podName != null && selectItems[i].podName != "") drophtml = drophtml + selectItems[i].podName;
            drophtml = drophtml + "<br/>";
            if (selectItems[i].etd != null && selectItems[i].etd != "") drophtml = drophtml + selectItems[i].etd + " \\ ";
            if (selectItems[i].eta != null && selectItems[i].eta != "") drophtml = drophtml + selectItems[i].eta;
            drophtml = drophtml + "<br/>" + selectItems[i].carrierName +"</div></a></li>";
            if (tagValue == selectItems[i].id) tag = selectItems[i].referenceNo;
        }
        $("#drop_" + tagName).html(tag);
        $("#" + tagName).val(tagValue);
        $("#dropdownMenuButton_" + tagName).html(drophtml);
}
function initTradePartnerSelect(selectItems, tagName, tagValue) {
    var l = abp.localization.getResource('Freight');
    var tag = l("Dropdown:Select");
    var drophtml = "<li class='dropdown-item2' id='li_" + tagName +"' ><input type='text' class='form-control' id='qid_" + tagName +"'></li>";
    var tpName = "";
    var tpCode = "";
    var tpAliasName = "";
    var tpLocalAddress = "";
    if (selectItems.length > 0) {
        for (var i = 0; i < selectItems.length; i++) {
            drophtml = drophtml + "<li class='form-control' style='width:450px;'><a  style='width:400px;' class='dropdown-item2'  href='javascript:void(0)' onclick='changeTradePartnerDropdownValue(\"" + tagName + "\",\"" + selectItems[i].id + "\",\"" + selectItems[i].tpName + "\",\"" + selectItems[i].tpCode + "\",\"" + selectItems[i].tpAliasName + "\",\"" + selectItems[i].tpLocalAddress + "\")'><div class='col-sm-9'>" + selectItems[i].tpName + "</di><div class='col-sm-2 pull-right'>  " + selectItems[i].tpAliasName + "</div><div class='col-sm-12'> " + selectItems[i].tpCode + "</div></a></li>"
            if (tagValue == selectItems[i].id) {
                tag = selectItems[i].tpName;
                tpName = selectItems[i].tpName;
                tpCode = selectItems[i].tpCode;
                tpAliasName = selectItems[i].tpAliasName;
                tpLocalAddress = selectItems[i].tpLocalAddress;
            } 
        }
    }
    $("#drop_" + tagName).html(tag);
    $("#" + tagName).val(tagValue);
    $("#dropdownMenuButton_" + tagName).html(drophtml);
    if (tagValue != "") {
        changeTradePartnerDropdownValue(tagName, tagValue, tpName, tpCode, tpAliasName, tpLocalAddress)
    }
}
function initSubstationsTag(selectItems, tagName, tagValue) {
    var drophtml = "";
    var l = abp.localization.getResource('Freight');
    var tag = l("Dropdown:Select");
    for (var i = 0; i < selectItems.length; i++) {
        drophtml = drophtml + "<li class='form-control' style='width:450px;'><a  style='width:400px;' class='dropdown-item2'  href='javascript:void(0)' onclick='changeDropdownValue(\"" + tagName + "\",\"" + selectItems[i].id + "\",\"" + selectItems[i].substationName + "\")'>" + selectItems[i].substationName + "</div></a></li>"
        if (tagValue == selectItems[i].id) tag = selectItems[i].substationName;
    }
    $("#drop_" + tagName).html(tag);
    $("#" + tagName).val(tagValue);
    $("#dropdownMenuButton_" + tagName).html(drophtml);
}
function initPortsTag(selectItems, tagName, tagValue) {
    var drophtml = "";
    var l = abp.localization.getResource('Freight');
    var tag = l("Dropdown:Select");
    for (var i = 0; i < selectItems.length; i++) {
        drophtml = drophtml + "<li class='form-control' style='width:450px;'><a  style='width:400px;' class='dropdown-item2'  href='javascript:void(0)' onclick='changeDropdownValue(\"" + tagName + "\",\"" + selectItems[i].id + "\",\"" + selectItems[i].portName + "\")'>" + selectItems[i].portName + "(" + selectItems[i].locode + ")" + "</div></a></li>"
        if (tagValue == selectItems[i].id) tag = selectItems[i].portName;
    }
    $("#drop_" + tagName).html(tag);
    $("#" + tagName).val(tagValue);
    $("#dropdownMenuButton_" + tagName).html(drophtml);
}
function changeDropdownValue(tagName, tagValue, showCode) {
    $("#" + tagName).val(tagValue);
    $("#drop_" + tagName).html(showCode);
}
function changeCancelReasionDropdownValue(tagName, tagValue, showCode) {
    $("#" + tagName).val(tagValue);
    var l = abp.localization.getResource('Freight');
    $("#drop_" + tagName).html(l(showCode));
}
function changeTradePartnerDropdownValue(tagName, tagValue, tpName, tpCode, tpAliasName, tpLocalAddress){
    $("#" + tagName).val(tagValue);
    
    var doprHtml = tpName; 
    
    $("#drop_" + tagName).html(doprHtml);
    $("#after_" + tagName).remove();
    var afterHtml = ' <button class="btn btn-outline-info" type="button" id="after_' + tagName + '" data-busy-text="Processing..." data-bs-toggle="tooltip" data-bs-trigger="hover" data-bs-placement="top"  onclick="gotoTradePartner(\'' + tagValue + '\')" title="'+tpName+' '+ tpCode+' '+ tpAliasName+'" ><li class="fa fa-file-export"  ></li></button >'
    $("#drop_" + tagName).after(afterHtml);
    changeCopover(tagName, tpName, tpCode, tpAliasName, tpLocalAddress);
}

function editTitle(tagId, tagValue) {
    debugger;
    var exampleEl = $("#btn_" + tagId);
    $("#btn_" + tagId).popover('hide');


    $("#modal_" + tagId).modal('show');
}
function changeTextarea(tagId) {

    var exampleEl = $("#btn_" + tagId);
    var popover = new bootstrap.Popover(exampleEl, {
        html: true, // 
        content: $("#" + tagId).val(),
    })
}
function changeCopover(tagName, tpName, tpCode, tpAliasName, tpLocalAddress) {
    var id = tagName.replace('Id', 'Content')
    if ($("#" + id).length === 0)
        return;
    var exampleEl = $("#btn_" + id);
    var alias = "&nbsp;";
    var aliasValue = "";
    if (tpAliasName != null && tpAliasName != 'null') {
        alias = tpAliasName;
        aliasValue = tpAliasName;
    } 
    var popover = new bootstrap.Popover(exampleEl, {
        html: true, // 
        content: alias ,
    })
    $("#" + id).val(aliasValue);
}
function gotoTradePartner(Id)
{
    window.open('/Sales/TradePartner/EditTradePartnerInfo?id=' + Id, 'TradePartner');
}