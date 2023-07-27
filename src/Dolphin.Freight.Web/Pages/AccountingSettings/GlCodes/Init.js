var selectItems;
$(document).ready(function () {
    var l = abp.localization.getResource('Freight');
    var a = $("#formAmsNoId").val()
    console.log("a="+a);
    dolphin.freight.common.ajaxDropdown.getSysCodeDtosByType({ queryType: 'GlTypeId' }).done(function (result) {  
        initSysCodeTag(result, "GlTypeId", $("#formGlTypeId").val());
    });
    //dolphin.freight.common.ajaxDropdown.getSysCodeDtosByType({ queryType: 'GlGroupId' }).done(function (result) {
    //    initSysCodeTag(result, "GlGroupId", $("#formGlGroupId").val());
    //});
    $('#glTypeSelect').on('change', function () {
        var glTypeId = $(this).val();
        debugger;
    });

    $("#saveBtn").click(function () { doSubmit() });
});
function initSysCodeTag(selectItems,tagName, tagValue) {
    var drophtml = "";
    var l = abp.localization.getResource('Freight');
    var tag = l("Dropdown:Select");
    for (var i = 0; i < selectItems.length; i++) {
        
        drophtml = drophtml + "<li class='form-control' style='width:450px;'><a  style='width:400px;' class='dropdown-item'  href='#' onclick='changeDropdownValue(\"" + tagName + "\",\"" + selectItems[i].id + "\",\"" + selectItems[i].codeValue + "\")'>" + selectItems[i].showName + "</div></a></li>"
        if (tagValue == selectItems[i].id)
        {
            tag = selectItems[i].showName;
        } 
    }
    $("#drop_" + tagName).html(tag);
    $("#" + tagName).val(tagValue);
    $("#dropdownMenuButton_" + tagName).html(drophtml);
}
function initSysGroupCodeTag(selectItems, tagName, tagValue) {
    var drophtml = "";
    var l = abp.localization.getResource('Freight');
    var tag = l("Dropdown:Select");
    for (var i = 0; i < selectItems.length; i++) {

        drophtml = drophtml + "<li class='form-control' style='width:450px;'><a  style='width:400px;' class='dropdown-item'  href='#' onclick='changeGroupDropdownValue(\"" + tagName + "\",\"" + selectItems[i].id + "\",\"" + selectItems[i].codeValue + "\")'>" + selectItems[i].showName + "</div></a></li>"
        if (tagValue == selectItems[i].id) {
            tag = selectItems[i].showName;
        }
    }
    $("#drop_" + tagName).html(tag);
    $("#" + tagName).val(tagValue);
    $("#dropdownMenuButton_" + tagName).html(drophtml);
}
function changeDropdownValue(tagName, tagValue, showCode) {
    dolphin.freight.common.ajaxDropdown.getSysCodeDtosByType({ queryType: 'GlGroupId', parentId: tagValue }).done(function (result) {
       initSysGroupCodeTag(result, "GlGroupId", $("#formGlGroupId").val());
    });
    $("#" + tagName).val(tagValue);
    $("#drop_" + tagName).html(showCode);
}
function changeGroupDropdownValue(tagName, tagValue, showCode) {

    $("#" + tagName).val(tagValue);
    $("#drop_" + tagName).html(showCode);
}
function doSubmit()
{
    $("#formCode").val($("#GlCode_Code").val());
    $("#formGlTypeId").val($("#GlTypeId").val());
    $("#formGlGroupId").val($("#GlGroupId").val());
    $("#formRemark").val($("#GlCode_Remark").val());
    $("#formSubInfo").val($("#GlCode_SubInfo").val());
    $("#formAccountingGlCode").val($("#GlCode_AccountingGlCode").val());
    $("#formIsUsed").val($("#GlCode_IsUsed").prop("checked"));
    $("#formIsDeposit").val($("#GlCode_IsDeposit").prop("checked"));
    $("#formIsPayment").val($("#GlCode_IsPayment").prop("checked"));
    $("#formIsRevaluation").val($("#GlCode_IsRevaluation").prop("checked"));
    $("#createForm").submit();
}