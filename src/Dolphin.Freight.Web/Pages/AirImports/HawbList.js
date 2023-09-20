var l = abp.localization.getResource('Freight');
var dataTable;
var id;
var _changeInterval = null;
var queryListFilter = function () {
    return {
        search: $("#Keyword").val(),


        freightLocationId: $("#VesselSchedule_FreightLocationId").val() == '' ? null : $("#VesselSchedule_FreightLocationId").val(),
        shipperId: $("#VesselSchedule_ShipperId").val() == '' ? null : $("#VesselSchedule_ShipperId").val(),
        notifyId: $("#VesselSchedule_NotifyId").val() == '' ? null : $("#VesselSchedule_NotifyId").val(),
        saleId: $("#VesselSchedule_SaleId").val() == '' ? null : $("#VesselSchedule_SaleId").val(),
        customerId: $("#VesselSchedule_CustomerId").val() == '' ? null : $("#VesselSchedule_CustomerId").val(),
        consigneeId: $("#VesselSchedule_ConsigneeId").val() == '' ? null : $("#VesselSchedule_ConsigneeId").val(),
    

      


    
        postDate: $("#PostDate").val() == '' || $("#PostDate").val() == null ? null : new Date($("#PostDate").val()),
        creationDate: $("#CreationDate").val() == '' || $("#CreationDate").val() == null ? null : new Date($("#CreationDate").val()),

    };
};
var columns = [
    {
    title: '<input type="checkbox" id="selectAllCheckbox" disable="true" onclick="selectAllCheckbox(this)" style=" cursor: pointer;">',
    data: null,
    orderable: false,
    render: function (data, type, row) {

        var id = row.id;
        var filingNo = row.docNo;
        $('#btnAirExportHawbListProfit').prop('disabled', true);
        $('#selectAllCheckbox').prop('checked', false);
        return '<input type="checkbox" class="selectCheckbox" data-id="' + id + '" data-filingNo="' + filingNo + '" onclick="selectCheckbox(this)" style=" cursor: pointer;">';
    }
},
    {
        title: l('Actions'),
        orderable: false,
        rowAction: {
            items:
                [
                    {
                        text: l('Edit'),
                        visible: abp.auth.isGranted('Settings.ItNoRanges.Edit'), //CHECK for the PERMISSION
                        action: function (data) {
                            if (data.record.isLocked) {

                            }
                            location.href = 'EditMawb?Id=' + data.record.mawbId + '&hid=' + data.record.id;
                        }
                    },
                    {
                        text: l('Delete'),
                        visible: function (data) {

                            return !data.isLocked && abp.auth.isGranted('Settings.ItNoRanges.Delete')
                        },
                        action: function (data) {
                            if (!data.record.isLocked) {
                                abp.message.confirm(l('DeletionConfirmationMessage'))
                                    .then(function (confirmed) {
                                        if (confirmed) {
                                            dolphin.freight.importExport.oceanImports.oceanImportMbl
                                                .delete(data.record.mawbId)
                                                .then(function () {
                                                    abp.message.success(l('SuccessfullyDeleted'));
                                                    dataTable.ajax.reload();
                                                });
                                        }
                                    });

                            } else {
                                abp.message.warn("鎖定不能刪除")
                            }
                        }
                    }
                ]
        }

    },


]
function selectAllCheckbox(element) {
    var isChecked = $(element).prop('checked');
    $('#HawbListTable tbody input.selectCheckbox[type="checkbox"]').prop('checked', isChecked);

    if (isChecked) {
        $('#btnAirExportHawbListProfit').prop('disabled', false);
    } else {
        $('#btnAirExportHawbListProfit').prop('disabled', true);
    }
}
function selectCheckbox(checkbox) {
    if ($("#HawbListTable input[type=checkbox]:checked").length > 0) {
        $('#btnAirExportHawbListProfit').prop('disabled', false);
        //check if all rows are selected then make main header checkbox selected
        $('#selectAllCheckbox').prop('checked', false);
        if ($("#HawbListTable input.selectCheckbox[type=checkbox]").length == $("#HawbListTable input[type=checkbox]:checked").length) {
            $('#selectAllCheckbox').prop('checked', true);
        }
    } else {
        $('#btnAirExportHawbListProfit').prop('disabled', true);
    }
}
function getHwabProfitReport(reportType) {
    var params = "";
    var selectedCheckboxes = $('#HawbListTable tbody input.selectCheckbox[type="checkbox"]:checked');
    for (var i = 0; i < selectedCheckboxes.length; i++) {
        var id = selectedCheckboxes[i].attributes['data-id'].value;
        var filingNo = selectedCheckboxes[i].attributes['data-filingNo'].value;


        params += id + ' / ' + filingNo + ',';
    }
    console.log(params);
    params = params.replace(/^,|,$/g, '');
   var id= selectedCheckboxes[0].attributes['data-id'].value;
    OpenWindow('/Docs/AirImportProfitReport/' + id +'?pageType=AIHBL&reportType=Summar');
}
$(function () {
    dolphin.freight.web.controllers.configuration.getJsonConfig('AirImportHawbList').done(function (data) {
        data.forEach(function (item) {
       
            if (!item.lock && item.checkable) {

                if (item.text.toLowerCase().includes('islocked')) {
                    column = {
                        //是否鎖定
                        title: '<div  style=" cursor: pointer;"><span><i class="fa fa-unlock"></i>Status</span></div>',
                        orderable: false,
                        render: function (data, type, row, meta) {
                            if (row.isLocked)
                                return "<a href='javascript:lock(\"" + row.id + "\")' class='btn-lock' id='lock_" + row.id + "'><i class='fa-lg fa-solid fa-lock fa-lg'></i><span>解鎖</span></a>";
                            else
                                return "<a href='javascript:lock(\"" + row.id + "\")' class='btn-lock action' id='lock_" + row.id + "'><i class='fa-lg fa-solid fa-lock-open'></i><span>上鎖</span></a>";
                        }
                    }
                }
                else {
                    debugger;
                    column = {
                       
                        title: l(item.text),
                        data: item.name
                    };
                }
                columns.push(column);
            }
        });

        var col = (columns.length > 2) ? [[2, 'asc']] : [[0, 'asc']];


        dataTable = $('#HawbListTable').DataTable(
            abp.libs.datatables.normalizeConfiguration({
                serverSide: true,
                paging: true,
                order: [[2, "asc"]],
                searching: false,
                processing: true,
                scrollX: true,
               
                responsive: {
                    details: {
                        type: 'column'
                    }
                },
                ajax: abp.libs.datatables.createAjax(dolphin.freight.importExport.airImports.airImportHawb.queryList, queryListFilter),
                columnDefs: columns,
                
            })
        );


    })



    $('#Search').click(function (e) {
        dataTable.ajax.reload();
    });

    $('#NewMblButton').click(function (e) {
        location.href = 'CreateMbl';
    });

    $('#btnConfiguration').click(function (e) {
        var _configurationModal = new abp.ModalManager({
            viewUrl: abp.appPath + 'Configuration',
            modalClass: 'ConfigurationViewModel'
        });

        _configurationModal.open({
            src: 'AirImportHawbList'
        });
    })



});
var lock = function (id) {
    var isLock = $('#lock_' + id).find('i').hasClass('fa-lock');
    abp.message.confirm(l(isLock ? 'UnlockConfirmationMessage' : 'LockConfirmationMessage')).then(function (confirmed) {
        if (confirmed) {
            dolphin.freight.importExport.airImports.airImportHawb.lockedOrUnLockedAirImportHawb(id)
                .done(function () {
                    if (isLock) {
                        abp.message.success(l('Message:SuccessUnlock'));
                    } else {
                        abp.message.success(l('Message:SuccessLock'));
                    }
                    dataTable.ajax.reload();
                });
        }
    });
}

//$(function () {
//    dataTable = $('#HawbListTable').DataTable(
//        abp.libs.datatables.normalizeConfiguration({
//            serverSide: true,
//            paging: true,
//            order: [[2, "asc"]],
//            searching: false,
//            processing: true,
//            scrollX: true,
//            responsive: {
//                details: {
//                    type: 'column'
//                }
//            },
//            ajax: abp.libs.datatables.createAjax(dolphin.freight.importExport.airImports.airImportHawb.queryList, queryListFilter),
//            columnDefs: [
//                //{
//                //    className: 'dtr-control',
//                //    orderable: false,
//                //    "defaultContent": ""
//                //},
//                {
//                    title: l('Actions'),
//                    rowAction: {
//                        items:
//                            [
//                                {
//                                    text: l('Edit'),
//                                    visible: abp.auth.isGranted('Settings.ItNoRanges.Edit'), //CHECK for the PERMISSION
//                                    action: function (data) {
//                                        if (data.record.isLocked) {

//                                        }
//                                        location.href = 'EditMawb?Id=' + data.record.mawbId + '&hid=' + data.record.id;

//                                    }
//                                },
//                                {
//                                    text: l('Delete'),
//                                    visible: function (data) {

//                                        return !data.isLocked && abp.auth.isGranted('Settings.ItNoRanges.Delete')
//                                    },

//                                    action: function (data) {
//                                        if (!data.record.isLocked) {
//                                            abp.message.confirm(l('DeletionConfirmationMessage'))
//                                                .then(function (confirmed) {
//                                                    if (confirmed) {
//                                                        dolphin.freight.importExport.airImports.airImportmawbNo
//                                                            .delete(data.record.id)
//                                                            .then(function () {
//                                                                abp.message.success(l('SuccessfullyDeleted'));
//                                                                dataTable.ajax.reload();
//                                                            });
//                                                    }
//                                                });

//                                        } else {
//                                            abp.message.warn("鎖定不能刪除")
//                                        }

//                                        /*
//                                        if (!data.record.isLocked) {
//                                            dolphin.freight.importExport.oceanExports.oceanExportMbl
//                                                .delete(data.record.id)
//                                                .then(function () {
//                                                    abp.message.success(l('SuccessfullyDeleted'));
//                                                    dataTable.ajax.reload();
//                                                });
//                                        } else {
//                                            abp.message.warn("鎖定不能刪除")
//                                        }*/

//                                    }
//                                }
//                            ]
//                    }
//                },
//                {
//                    //是否鎖定
//                    title: l('IsLocked'),
//                    orderable: false,
//                    render: function (data, type, row, meta) {
//                        if (row.isLocked)
//                            return "<a href='javascript:lock(\"" + row.id + "\")' class='btn-lock' id='lock_" + row.id + "'><i class='fa-lg fa-solid fa-lock fa-lg'></i><span>解鎖</span></a>";
//                        else
//                            return "<a href='javascript:lock(\"" + row.id + "\")' class='btn-lock action' id='lock_" + row.id + "'><i class='fa-lg fa-solid fa-lock-open'></i><span>上鎖</span></a>";
//                    }
//                },
//                {
//                    //Hawb號碼
//                    title: l('HawbNo'),
//                    data: "hawbNo",
//                    defaultContent: ''
//                },
//                {
//                    //Mawb號碼
//                    title: l('MawbId'),
//                    data: "mawbId",
//                    defaultContent: ''
//                },
//                {
//                    //Departure
//                    title: l('QuotationId'),
//                    data: "quotationId",
//                    defaultContent: ''
//                },
//                {
//                    //Destination
//                    title: l('Hsn'),
//                    data: "hsn",
//                    defaultContent: ''
//                },
//                {
//                    //ActualShippedr
//                    title: l('ShipperId'),
//                    data: "shipperId",
//                    defaultContent: ''
//                },
//                {
//                    //Consignee
//                    title: l('ConsigneeId'),
//                    data: "consigneeId",
//                    defaultContent: ''
//                },
//                {
//                    //OverseaAgent
//                    title: l('SalesId'),
//                    data: "salesId",
//                    defaultContent: ''
//                },
     
//                {
//                    //A/P Balance
//                    title: l('Incoterms'),
//                    data: "incoterms",
//                    defaultContent: ''
//                },
//                {
//                    //Booking No.
//                    title: l('Mark'),
//                    data: "mark",
//                    defaultContent: ''
//                },
//                {
//                    //OP
//                    title: l('Description'),
//                    data: "description",
//                    defaultContent: ''
//                },
//                {
//                    //Status
//                    title: l('Remark'),
//                    data: "remark",
//                    defaultContent: ''
//                },


//            ]
//        })
//    );

//    $('#Search').keyup(function () {
//        clearInterval(_changeInterval)
//        _changeInterval = setInterval(function () {
//            dataTable.ajax.reload();
//            clearInterval(_changeInterval)
//        }, 1000);
//    });
//});

