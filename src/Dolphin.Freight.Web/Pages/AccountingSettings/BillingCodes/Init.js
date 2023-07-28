var selectItems;
$(document).ready(function () {
    var l = abp.localization.getResource('Freight');
    var a = $("#formAmsNoId").val()
    dolphin.freight.accountingSettings.glCodes.glCode.getGlCodes({}).done(function (result) {
        console.log(result)
        initSysCodeTag(result, "RevenueId", $("#formRevenueId").val());
    });
    dolphin.freight.accountingSettings.glCodes.glCode.getGlCodes({}).done(function (result) {
        initSysCodeTag(result, "CostId", $("#formCostId").val());
    });
    dolphin.freight.accountingSettings.glCodes.glCode.getGlCodes({}).done(function (result) {
        initSysCodeTag(result, "CreditId", $("#formCreditId").val());
    });
    dolphin.freight.accountingSettings.glCodes.glCode.getGlCodes({}).done(function (result) {
        initSysCodeTag(result, "DeitId", $("#formDeitId").val());
      
    });

    $("#saveBtn").click(function () { doSubmit() });
});
function initSysCodeTag(selectItems, tagName, tagValue) {
    
    var drophtml = "";
    var l = abp.localization.getResource('Freight');
    var tag = l("Dropdown:Select");
    for (var i = 0; i < selectItems.length; i++) {
        
        drophtml = drophtml + "<li class='form-control' style='width:450px;'><a  style='width:400px;' class='dropdown-item'  href='#' onclick='changeDropdownValue(\"" + tagName + "\",\"" + selectItems[i].id + "\",\"" + selectItems[i].code + "\")'>" + selectItems[i].code + "</div></a></li>"
        if (tagValue == selectItems[i].id)
        {
            tag = selectItems[i].code;
        } 
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
    $("#formCode").val($("#BillingCode_Code").val());
    $("#formBillingName").val($("#BillingCode_BillingName").val());
    $("#formLocalName").val($("#BillingCode_LocalName").val());
    $("#formRevenueId").val($("#RevenueId").val());
    $("#formCostId").val($("#CostId").val());
    $("#formCreditId").val($("#CreditId").val());
    $("#formDeitId").val($("#DeitId").val());
    $("#formIsUsed").val($("#BillingCode_IsUsed").prop("checked"));
    $("#formIsAR").val($("#BillingCode_IsAR").prop("checked"));
    $("#formIsAP").val($("#BillingCode_IsAP").prop("checked"));
    $("#formIsDC").val($("#BillingCode_IsDC").prop("checked"));
    $("#formIsGA").val($("#BillingCode_IsGA").prop("checked"));
    $("#formIsOceanImportMbl").val($("#BillingCode_IsOceanImportMbl").prop("checked"));
    $("#formIsOceanImportHbl").val($("#BillingCode_IsOceanImportHbl").prop("checked"));
    $("#formIsOceanExportMbl").val($("#BillingCode_IsOceanExportMbl").prop("checked"));
    $("#formIsOceanExportHbl").val($("#BillingCode_IsOceanExportHbl").prop("checked"));
    $("#formIsAirImportMbl").val($("#BillingCode_IsAirImportMbl").prop("checked"));
    $("#formIsAirImportHbl").val($("#BillingCode_IsAirImportHbl").prop("checked"));
    $("#formIsAirExportMbl").val($("#BillingCode_IsAirExportMbl").prop("checked"));
    $("#formIsAirExportHbl").val($("#BillingCode_IsAirExportHbl").prop("checked"));
    $("#formIsTkm").val($("#BillingCode_IsTkm").prop("checked"));
    $("#formIsMsm").val($("#BillingCode_IsMsm").prop("checked"));
    $("#formIsWhs").val($("#BillingCode_IsWhs").prop("checked"));
    $("#formIsPayroll").val($("#BillingCode_IsPayroll").prop("checked"));
    $("#createForm").submit();
}