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
        var datatable = getAllRowsData();
        $("#datatablelist").val(datatable);
        $("#createForm").submit();
        //var datatableRowArray = JSON.parse(getAllRowData());

        //if (datatableArray.length > 0) {
        //    datatableArray = datatableArray.concat(datatableRowArray);
        //    $("#datatablelist").val(JSON.stringify(datatableArray));
        //    $("#createForm").submit();
        //} else {
        //    $("#datatablelist").val(datatableRowArray);
        //    $("#createForm").submit();
        //}
    });

    function getAllRowsData() {
        var allData = [];

        //var existingData = $('#InvTable').DataTable().rows().data().toArray();
        //debugger;
        //allData = allData.concat(existingData);

        $('#InvTable tbody tr').each(function (index, row) {
            var rowData = {};

            $(row).find('td').each(function (cellIndex, cell) {
                var input = $(cell).find('input, select');
                if (input.length) {
                    var key = input.attr('name');
                    var value = input.val();
                    rowData[key] = value;
                } else {
                    if ($('#InvTable').DataTable().column(cellIndex).dataSrc().toLowerCase() == 'glcodeid') {
                        rowData[$('#InvTable').DataTable().column(cellIndex).dataSrc()] = null;
                    } else {
                        rowData[$('#InvTable').DataTable().column(cellIndex).dataSrc()] = $(cell).text();
                    }
                }
            });

            allData.push(rowData);
        });

        var allDataJson = JSON.stringify(allData);

        return allDataJson;
    }

    //function getAllRowData() {
    //    var allData = [];

    //    $('#InvTable tbody tr').each(function (index, row) {
    //        var rowData = {};

    //        $(row).find('td').each(function (cellIndex, cell) {
    //            var input = $(cell).find('input, select');

    //            if (input.length) {
    //                var key = input.attr('name');
    //                var value = input.val();

    //                rowData[key] = value;
    //            }
    //        });

    //        allData.push(rowData);
    //    });

    //    var allDataJson = JSON.stringify(allData);

    //    return allDataJson;
    //}

    $('#createForm').on('abp-ajax-success', function (result, rs) {           
        abp.message.success('Successfully saved.');
    });
});
