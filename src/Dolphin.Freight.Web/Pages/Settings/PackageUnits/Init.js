var selectItems;
$(document).ready(function () {
    var l = abp.localization.getResource('Freight');
    var a = $("#formAmsNoId").val()
    console.log("a="+a);
    //dolphin.freight.common.ajaxDropdown.getSysCodeDtosByType({ queryType: 'AmsNoId' }).done(function (result) {
    //    console.log(result)
    //    console.log($("#formAmsNoId").val())
    //    initSysCodeTag(result, "AmsNoId", $("#formAmsNoId").val());
    //});
    //dolphin.freight.common.ajaxDropdown.getSysCodeDtosByType({ queryType: 'EManifestId' }).done(function (result) {
    //    console.log(result)
    //    console.log($("#formEManifestId").val())
    //    initSysCodeTag(result, "EManifestId", $("#formEManifestId").val());
    //});
    $("#saveBtn").click(function () { doSubmit() });
});
function initSysCodeTag(selectItems, tagName, tagValue) {
    var drophtml = "";
    var l = abp.localization.getResource('Freight');
    var tag = l('Dropdown:Select');
    for (var i = 0; i < selectItems.length; i++) {
        drophtml = drophtml + "<li class='form-control' style='width:450px;'><a  style='width:400px;' class='dropdown-item'  href='javascript:void(0)' onclick='changeDropdownValue(\"" + tagName + "\",\"" + selectItems[i].id + "\",\"" + selectItems[i].showName + "\")'>" + selectItems[i].showName + "</div></a></li>"
        if (tagValue == selectItems[i].id) tag = selectItems[i].showName;
    }
    $("#drop_" + tagName).html(tag);
    $("#" + tagName).val(tagValue);
    $("#dropdownMenuButton_" + tagName).html(drophtml);
}
function changeDropdownValue(tagName, tagValue, showCode) {
    $("#" + tagName).val(tagValue);
    $("#drop_" + tagName).html(showCode);
}
function doSubmit()
{
    $("#formPackageCode").val($("#PackageUnit_PackageCode").val());
    $("#formPackageName").val($("#PackageUnit_PackageName").val());
    $("#formAmsNoId").val($("#PackageUnit_AmsNoId").val());
    $("#formEManifestId").val($("#PackageUnit_EManifestId").val());
    $("#formDescription").val($("#PackageUnit_Description").val());
    $("#createForm").submit();
}