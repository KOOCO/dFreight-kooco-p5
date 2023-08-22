var l = abp.localization.getResource('Freight');
var dataTable;
var id;
var _changeInterval = null;
var queryListFilter = function () {
    return {
        search: $("input[name='Search'").val()
    };
};
var columns = [
    {
        orderable: false,
        className: 'select-checkbox',
        targets: 0
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
$(function () {
    dolphin.freight.web.controllers.configuration.getJsonConfig('AirImportHawbList').done(function (data) {
        data.forEach(function (item) {
       
            if (!item.lock && item.checkable) {
                var column;

                if (item.text.toLowerCase().includes('islocked')) {
                    column = {
                        //是否鎖定
                        title: l('Status'),
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
                select: true,
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



    $('#Search').keyup(function () {
        clearInterval(_changeInterval)
        _changeInterval = setInterval(function () {
            dataTable.ajax.reload();
            clearInterval(_changeInterval)
        }, 1000);
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

    $('#HawbListTable').on('click', 'tr', function () {
        debugger;
        var selectedRow = dataTable.row(this);
      
        var selectedData = selectedRow.data();
        id = selectedData.id;
           

            // Use the secondColumnValue in your logic
       
        if ($(this).hasClass('selected')) {
            $(this).removeClass('selected');
            // Call your function to handle row deselection
            // e.g., handleDeselection($(this).data('row-id'));
        } else {
            dataTable.$('tr.selected').removeClass('selected');
            $(this).addClass('selected');
            // Call your function to handle row selection
            // e.g., handleSelection($(this).data('row-id'));
        }

        // Enable/disable your button based on selection status
        updateButtonState();
    });

    // Function to update button state
    function updateButtonState() {
        var $button = $('#btnProfitReport');
        var anySelected = dataTable.rows('.selected').any();
        $button.prop('disabled', !anySelected);
    }
    $('#btnProfitReport').click(function (e) {
        debugger;
        var url = '/Docs/AirImportProfitReport/'+id+'?pageType=AIHBL&reportType=Summary';
       var pname = "ProfitSummary";
        myWindow = window.open(url, pname, 'width=1200,height=800')
        myWindow.focus()
    });

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

