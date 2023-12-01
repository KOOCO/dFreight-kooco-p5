var l = abp.localization.getResource('Freight');
var dataTable;
var copyModalMawbList = new abp.ModalManager({
    viewUrl: '/AirExports/CopyModalMawbList',
});
$(function () {
   
    var _changeInterval = null;
    var queryListFilter = function () {
        return {
            search: $("#Keyword").val(),

           
            shipperId: $("#VesselSchedule_ShipperId").val() == '' ? null : $("#VesselSchedule_ShipperId").val(),
          
            consigneeId: $("#VesselSchedule_ConsigneeId").val() == '' ? null : $("#VesselSchedule_ConsigneeId").val(),
            carrierId: $("#VesselSchedule_CarrierId").val() == '' ? null : $("#VesselSchedule_CarrierId").val(),
            depatureId: $("#VesselSchedule_DepatureId").val() == '' ? null : $("#VesselSchedule_DepatureId").val(),
            destinationId: $("#VesselSchedule_DestinationId").val() == '' ? null : $("#VesselSchedule_DestinationId").val(),
            awbCancelled: $("#AwbCancelled").val() == '' ? null : $("#AwbCancelled").val(),
            directMaster: $("#DirectMaster").val() == '' ? null : $("#DirectMaster").val(),
            flightNo: $("#FlightNo").val(),
            officeId: $("#VesselSchedule_OfficeId").val() == '' ? null : $("#VesselSchedule_OfficeId").val(),
           


            depatureDate: $("#DepatureDate").val() == '' || $("#DepatureDate").val() == null ? null : new Date($("#DepatureDate").val()),
            arrivalDate: $("#ArrivalDate").val() == '' || $("#ArrivalDate").val() == null ? null : new Date($("#ArrivalDate").val()),
            postDate: $("#PostDate").val() == '' || $("#PostDate").val() == null ? null : new Date($("#PostDate").val()),
            creationDate: $("#CreationDate").val() == '' || $("#CreationDate").val() == null ? null : new Date($("#CreationDate").val()),

        };
    };

    var columns = [{
        title: '<input type="checkbox" id="selectAllCheckbox" onclick="AirExportsMawbList.selectAllCheckbox(this)" style=" cursor: pointer;">',
        data: null,
        orderable: false,
        "render": function (data, type, row) {
            var id = row.id;
            var filingNo = row.filingNo;
            return '<input type="checkbox" class="selectCheckbox" data-id="' + id + '" data-filingNo="' + filingNo + '" onclick="AirExportsMawbList.selectCheckbox(this)" style=" cursor: pointer;">';
        }
    },
    {
        title: '<div  style=" cursor: pointer;"><span><i class="fa fa-lock"></i></span></div>',
        orderable: false,
        "render": function (data, type, row) {
            var isCkecked = row.isLocked;
            var id = row.id;
            if (isCkecked) {
                return '<input type="checkbox" class="lockUnlockCheckbox" data-id="' + id + '"  checked="' + isCkecked + '" onclick="AirExportsMawbList.lockCheckBox(this)"  style=" cursor: pointer;">';
            } else {
                return '<input type="checkbox" class="lockUnlockCheckbox" data-id="' + id + '" onclick="AirExportsMawbList.lockCheckBox(this)"   style=" cursor: pointer;">';
            }
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
                            location.href = 'EditModal?Id=' + data.record.id;
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
                                            dolphin.freight.importExport.airExports.airExportMawb
                                                .delete(data.record.id)
                                                .then(function () {
                                                    abp.message.success(l('SuccessfullyDeleted')).then(function () {
                                                        dataTable.ajax.reload();
                                                    });
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
    //{
    //    title: '<div  style=" cursor: pointer;"><span><i class="fa fa-unlock"></i>Status</span></div>',
    //    orderable: false,
    //    render: function (data) {
    //        if (!data) {
    //            return '';
    //        }
    //    }
    //    }
    ]

    var otherFixedColumns = [{
            title: l('GrossWeight'),
            data: null,
            render: function (data, row) {
                return `<span class="kg">KG : ${data.grossWeightKg}</span><span class="lb">LB : ${data.grossWeightLb}</span>`;
            }
        },
        {
            title: l('ChargableWeight'),
            data: null,
            render: function (data, row) {
                return `<span class="kg">KG : ${data.chargeableWeightKg}</span><span class="lb">LB : ${data.chargeableWeightLb}</span>`;
            }
        }];

    dolphin.freight.web.controllers.configuration.getJsonConfig('AirExportMawbList').done(function (data) {
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
                    var itemData = item.text;

                    var fieldMappings = {
                        'File No.': 'filingNo',
                        'MAWB No.': 'mawbNo',
                        'Office': 'officeName',
                        'Departure Date/Time': 'depatureDate',
                        'Arrival Date/Time': 'arrivalDate',
                        'Departure': 'depatureAirportName',
                        'Destination': 'destinationAirportName',
                        'B/L Date': 'awbDate',
                        'Shipper (Shipping Agent)': 'shipper',
                        'Consignee (Oversea Agent)': 'consigneeName',
                        'Carrier': 'carrierTPName',
                        'Flight No.': 'flightNo',
                        'Hawb Number': 'hawbNos',
                        'Bill To': 'awbAccountCarrierName',
                        'A/R Balance': 'arTotal',
                        'A/P Balance': 'apTotal',
                        'D/C Balance': 'dcTotal',
                        'Post Date': 'postDate',
                        'Sales': 'sales',
                        'Operator': 'oP'
                    }

                    var dataFields = ['File No.', 'MAWB No.', 'Office', 'Departure', 'Destination', 'Shipper (Shipping Agent)', 'Consignee (Oversea Agent)', 'Carrier', 'Flight No.', 'Hawb Number', 'Bill To', 'Sales', 'Operator'];
                    dataFields.forEach(function (field) {
                        if (item.text.includes(field)) {
                            var actualFieldName = fieldMappings[field];
                            itemData = createGenericFunction('data', actualFieldName);
                        }
                    });

                    var dateFields = ['Departure Date/Time', 'Arrival Date/Time', 'B/L Date', 'Post Date'];
                    dateFields.forEach(function (field) {
                        if (item.text.includes(field)) {
                            var actualFieldName = fieldMappings[field];
                            itemData = createGenericFunction('date', actualFieldName);
                        }
                    });

                    var floatFields = ['A/R Balance', 'A/P Balance', 'D/C Balance'];
                    floatFields.forEach(function (field) {
                        if (item.text.includes(field)) {
                            var actualFieldName = fieldMappings[field];
                            itemData = createGenericFunction('float', actualFieldName);
                        }
                    });

                    var column = {
                        title: l(item.text),
                        data: itemData
                    };
                }
                columns.push(column);
            }
        });

        columns = columns.concat(otherFixedColumns);
        var col = (columns.length > 1) ? [[1, 'asc']] : [[0, 'asc']];

        dataTable = $('#MawbListTable').DataTable(
            abp.libs.datatables.normalizeConfiguration({
                serverSide: true,
                paging: true,
                order: col,
                searching: false,
                scrollX: true,
                processing: true,
                responsive: {
                    details: {
                        type: 'column'
                    }
                },
                ajax: abp.libs.datatables.createAjax(dolphin.freight.importExport.airExports.airExportMawb.getList, queryListFilter),
                columnDefs: columns,
                drawCallback: function (settings) {
                    $('.sorting_asc').removeClass('sorting_asc');
                }
            })
        );
    });

    function createGenericFunction(dataType, fieldName) {
        return function (row, type, set) {
            switch (dataType) {
                case 'data':
                    return row[fieldName] || '-';
                case 'date':
                    var dateValue = new Date(row[fieldName]);
                    if (dateValue.getFullYear() == 1) {
                        return '-';
                    }
                    return dateValue.toLocaleDateString();
                case 'float':
                    if (!row[fieldName]) {
                        return '-';
                    }
                    return parseFloat(row[fieldName]).toFixed(2);
            }
        }
    }

    $('#Search').click(function (e) {
        dataTable.ajax.reload();
    });
    
    $('#btnConfiguration').click(function (e) {
        var _configurationModal = new abp.ModalManager({
            viewUrl: abp.appPath + 'Configuration',
            modalClass: 'ConfigurationViewModel'
        });

        _configurationModal.open({
            src: 'AirExportMawbList'
        });
    })
});

var AirExportsMawbList = {
    selectAllCheckbox: function (element) {
        var isChecked = $(element).prop('checked');
        $('#MawbListTable tbody input.selectCheckbox[type="checkbox"]').prop('checked', isChecked);

        if (isChecked) {
            $('#summaryId').prop('disabled', false);
            $('#detailedId').prop('disabled', false);
        } else {
            $('#summaryId').prop('disabled', true);
            $('#detailedId').prop('disabled', true);
        }
    },
    selectCheckbox: function (checkbox) {
        var checkedCheckboxes = $('.selectCheckbox:checked');
        if (checkbox.checked) {
            $('#summaryId').prop('disabled', false);
            $('#detailedId').prop('disabled', false);
        
            if (checkedCheckboxes.length == 1) {
                $('#copyId').prop('disabled', false);
            }
            else {
                $('#copyId').prop('disabled', true);
            }
            var isAnyLocked = false;
            var isAnyUnlocked = false
            checkedCheckboxes.each(function (index, checkbox1) {
                var id = $(checkbox1).data('id');
        
                var isLock = $('#lock_' + id).find('i').hasClass('fa-lock');
                if (isLock) {
                    isAnyLocked = true;
                }
                else {
                    isAnyUnlocked = true;
                }
            });
            $('#lockId').prop('disabled', !isAnyUnlocked);
            $('#unlockId').prop('disabled', !isAnyLocked);
        } else {
            $('#summaryId').prop('disabled', true);
            $('#detailedId').prop('disabled', true);
            var checkedCheckboxes = $('.selectCheckbox:checked');
            if (checkedCheckboxes.length == 1) {
                $('#copyId').prop('disabled', false);
            }
            else {
                $('#copyId').prop('disabled', true);
            }
            checkedCheckboxes.each(function (index, checkbox1) {
                var id = $(checkbox1).data('id');
                var isLock = $('#lock_' + id).find('i').hasClass('fa-lock');
                if (isLock) {
                    isAnyLocked = true;
                }
                else {
                    isAnyUnlocked = true;
                }
            });
            $('#lockId').prop('disabled', !isAnyUnlocked);
            $('#unlockId').prop('disabled', !isAnyLocked);
        }
        if (!$(checkbox).prop('checked')) {
            $('#selectAllCheckbox').prop('checked', false);
        } else {
            var allChecked = true;
            $('#MawbListTable tbody input.selectCheckbox[type="checkbox"]').each(function () {
                if (!$(this).prop('checked')) {
                    allChecked = false;
                    return false;
                }
            });
            $('#selectAllCheckbox').prop('checked', allChecked);
        }
    },
    lockCheckBox: function (checkbox) {
        var selectedCheckboxes = $('#MawbListTable tbody input.lockUnlockCheckbox[type="checkbox"]:checked');
        var id = checkbox.attributes[2].value;
        var isLock = $('#lock_' + id).find('i').hasClass('fa-lock');
        abp.message.confirm(l(isLock ? 'UnlockConfirmationMessage' : 'LockConfirmationMessage')).then(function (confirmed) {
            if (confirmed) {
                dolphin.freight.importExport.airExports.airExportMawb.lockedOrUnLockedAirExportMawb(id).done(function () {
                    if (isLock) {
                        abp.message.success(l('Message:SuccessUnlock'));
                    } else {
                        abp.message.success(l('Message:SuccessLock'));
                    }
                    dataTable.ajax.reload();
                });
            }
            else {
                debugger
                if (checkbox.checked) {
                    $(checkbox).prop('checked', false);
                }
                else {
                    $(checkbox).prop('checked', true);
                }
            }
        });
    },
    selectedLock: function () {
        var ids = [];
        var selectedCheckboxes = $('#MawbListTable tbody input.selectCheckbox[type="checkbox"]:checked');
        for (var i = 0; i < selectedCheckboxes.length; i++) {
            var id = selectedCheckboxes[i].attributes[2].value;
            var isLock = $('#lock_' + id).find('i').hasClass('fa-lock')
            if (!isLock) {
                ids.push(id);
            }
            abp.message.confirm(l('LockConfirmationMessage')).then(function (confirmed) {
                if (confirmed) {
                    dolphin.freight.importExport.airExports.airExportMawb.selectedLockedAirExportMawb(ids).done(function () {
                        abp.message.success(l('Message:SuccessLock'));
                        dataTable.ajax.reload();
                    });
                }
            });
        }
    },
    selectedUnLock: function() {
        var ids = [];
        var selectedCheckboxes = $('#MawbListTable tbody input.selectCheckbox[type="checkbox"]:checked');
        for (var i = 0; i < selectedCheckboxes.length; i++) {
            var id = selectedCheckboxes[i].attributes[2].value;
            var isLock = $('#lock_' + id).find('i').hasClass('fa-lock')
            if (isLock) {
                ids.push(id);
            }
            abp.message.confirm(l('UnlockConfirmationMessage')).then(function (confirmed) {
                if (confirmed) {
                    dolphin.freight.importExport.airExports.airExportMawb.selectedUnLockedAirExportMawb(ids).done(function () {
                        abp.message.success(l('Message:Message:SuccessUnlock'));
                        dataTable.ajax.reload();
                    });
                }
            });
        }
    },
    openCopyModal: function (){
        var selectedCheckboxes = $('#MawbListTable tbody input.selectCheckbox[type="checkbox"]:checked');
        var id = selectedCheckboxes[0].attributes[2].value;
        copyModalMawbList.open({
            id,
        });
    },
    getProfitReport: function (reportType) {
        var params = "";
        var selectedCheckboxes = $('#MawbListTable tbody input.selectCheckbox[type="checkbox"]:checked');
        for (var i = 0; i < selectedCheckboxes.length; i++) {
            var id = selectedCheckboxes[i].attributes[2].value;
            var filingNo = selectedCheckboxes[i].attributes[3].value;
            params += id + ' / ' + filingNo + ',';
        }
        params = params.replace(/^,|,$/g, '');
        OpenWindow('/Docs/ProfitReportMawbListAirExport?reportType=' + reportType + '&pageType=@Dolphin.Freight.Common.FreightPageType.AEMBL&param=' + params);
    },
    createNew: function () {
        window.location.href = "/AirExports/CreateMawb";
    }
}

var lock = function (id) {
    var isLock = $('#lock_' + id).find('i').hasClass('fa-lock');
    abp.message.confirm(l(isLock ? 'UnlockConfirmationMessage' : 'LockConfirmationMessage')).then(function (confirmed) {
        if (confirmed) {
            dolphin.freight.importExport.airExports.airExportMawb.lockedOrUnLockedAirExportMawb(id)
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
