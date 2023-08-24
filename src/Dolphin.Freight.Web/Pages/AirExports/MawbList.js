var l = abp.localization.getResource('Freight');
var dataTable;
var copyModalMawbList = new abp.ModalManager({
    viewUrl: '/AirExports/CopyModalMawbList',
});
$(function () {
   
    var _changeInterval = null;
    var queryListFilter = function () {
        return {
            search: $("input[name='Search'").val()
        };
    };

    var columns = [{
        title: '<input type="checkbox" id="selectAllCheckbox" onclick="selectAllCheckbox(this)" style=" cursor: pointer;">',
        data: null,
        orderable: false,
        "render": function (data, type, row) {

            var id = row.id;
            var filingNo = row.filingNo;

            return '<input type="checkbox" class="selectCheckbox" data-id="' + id + '" data-filingNo="' + filingNo + '" onclick="selectCheckbox(this)" style=" cursor: pointer;">';
        }
    },
    {
        title: '<div  style=" cursor: pointer;"><span><i class="fa fa-lock"></i></span></div>',
        orderable: false,
        "render": function (data, type, row) {
            var isCkecked = row.isLocked;
            var id = row.id;
            debugger;
            if (isCkecked) {
                return '<input type="checkbox" class="lockUnlockCheckbox" data-id="' + id + '"  checked="' + isCkecked + '" onclick="lockCheckBox(this)"  style=" cursor: pointer;">';
            } else {
                return '<input type="checkbox" class="lockUnlockCheckbox" data-id="' + id + '" onclick="lockCheckBox(this)"   style=" cursor: pointer;">';
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

    $('#Search').keyup(function () {
        clearInterval(_changeInterval)
        _changeInterval = setInterval(function () {
            dataTable.ajax.reload();
            clearInterval(_changeInterval)
        }, 1000);
    });
    

    $('#AddMawbButton').click(function (e) {
        window.location = "\CreateMawb";
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
