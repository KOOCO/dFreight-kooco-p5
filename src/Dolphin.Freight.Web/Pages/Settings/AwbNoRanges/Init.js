var selectItems;
$(function () {
    var l = abp.localization.getResource('Freight');
    $("#AwbNoRange_CompanyId").hide();
    dolphin.freight.tradePartners.tradePartner.getList({}).done(function (result) {
        initTradePartnerSelect(result.items, "CarrierId", $("#formCarrierId").val());
    });

    $("#saveBtn").click(function () { doSubmit() });

});
function initTradePartnerSelect(selectItems, tagName, tagValue) {
    var l = abp.localization.getResource('Freight');
    var tag = l("Dropdown:Select");
    var drophtml = "";

    if (selectItems.length > 0) {
        for (var i = 0; i < selectItems.length; i++) {
            drophtml = drophtml + "<li class='form-control' style='width:450px;'><a  style='width:400px;' class='dropdown-item'  href='#' onclick='changeDropdownValue(\"" + tagName + "\",\"" + selectItems[i].id + "\",\"" + selectItems[i].tpName + "/" + selectItems[i].tpCode + "\")'>" + selectItems[i].tpName + "/" + selectItems[i].tpCode + "</div></a></li>"
            if (tagValue == selectItems[i].id) {
                tag = selectItems[i].tpName + "/" + selectItems[i].tpCode;
            } 
        }
    }
    $("#drop_" + tagName).html(tag);
    $("#" + tagName).val(tagValue);
    $("#dropdownMenuButton_" + tagName).html(drophtml);
    $("#dropdownMenuButton_" + tagName).html(drophtml);
}
function changeDropdownValue(tagName, tagValue, showCode) {
    $("#" + tagName).val(tagValue);
    $("#drop_" + tagName).html(showCode);
}
function doSubmit()
{
    $("#formStartNo").val($("#AwbNoRange_StartNo").val());
    $("#formEndNo").val($("#AwbNoRange_EndNo").val());
    $("#formNote").val($("#AwbNoRange_Note").val());
    $("#formCarrierId").val($("#CarrierId").val());
    $("#createForm").submit();
}