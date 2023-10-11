$(function () {
    var l = abp.localization.getResource('Freight');
    //系統參數
    //dolphin.freight.common.ajaxDropdown.getSysCodeDtosByType({ queryType: 'PaymentLevel_1' }).done(function (result) {
    //    if (result != null && result.length > 0)
    //    {
    //        initSysCodeTag(result, "PaymentLevel", $("#PaymentLevel").val())          
    //    }
    //});

    //類別
    //dolphin.freight.common.ajaxDropdown.getSysCodeDtosByType({ queryType: 'Category' }).done(function (result) {
    //    if (result != null && result.length > 0) {
    //        initSysCodeTag(result, "Category", $("#Category").val())
    //    }
    //});

    //分站
    //dolphin.freight.settings.substations.substation.getSubstations({}).done(function (result) {
    //    if (result != null && result.length > 0) {
    //        initSubstationsTag(result, "OfficeId", $("#OfficeId").val())
    //    }
    //});

    //收款來源
    //dolphin.freight.tradePartners.tradePartner.getList({}).done(function (result) {
    //    var selectItems = result.items;
    //    initTradePartnerSelect(selectItems, "ReceivablesSources", $("#ReceivablesSources").val());
    //});
 
    $("#saveBtn").click(function () {
        var datatable = InvoiceListJS.getAllRowsData();
        $("#datatablelist").val(datatable);
        $("#createForm").submit();
    });

    $('#createForm').on('abp-ajax-success', function (result, rs) {           
        abp.message.success('Successfully saved.');
    });
});
