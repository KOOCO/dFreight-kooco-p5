$(function () {
    var l = abp.localization.getResource('Freight');
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
            return '<input type="checkbox"  style=" cursor: pointer;">';
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
    {
        title: '<div  style=" cursor: pointer;"><span><i class="fa fa-unlock"></i>Status</span></div>',
        orderable: false,
        render: function (data) {
            if (!data) {
                return '';
            }
        }
    }]

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

    var dataTable;

    dolphin.freight.web.controllers.configuration.getJsonConfig('AirExportMawbList').done(function (data) {
        data.forEach(function (item) {
            if (!item.lock && item.checkable) {
                var itemData = item.name;

                var dataFields = ['mawbNo', 'officeName', 'depatureAirportName', 'destinationAirportName', 'shipper', 'consigneeName', 'carrierTPName', 'flightNo', 'hawbNos', 'awbAccountCarrierName', 'sales', 'oP'];
                dataFields.forEach(function (field) {
                    if (item.name.includes(field)) {
                        itemData = function (row, type, set) {
                            if (!row[field]) {
                                return '-';
                            }
                            return row[field];
                        }
                    }
                });

                var dateFields = ['depatureDate', 'arrivalDate', 'awbDate', 'postDate'];
                dateFields.forEach(function (field) {
                    if (item.name.includes(field)) {
                        itemData = function (row, type, set) {
                            var dateValue = new Date(row[field]);
                            if (dateValue.getFullYear() == 1) {
                                return '-';
                            }
                            return dateValue.toLocaleDateString();
                        }
                    }
                });

                var floatFields = ['arTotal', 'apTotal', 'dcTotal'];
                floatFields.forEach(function (field) {
                    if (item.name.includes(field)) {
                        itemData = function (row, type, set) {
                            if (!row[field]) {
                                return '-';
                            }
                            return parseFloat(row[field]).toFixed(2);
                        }
                    }
                });

                var column = {
                    title: l(item.name),
                    data: itemData
                };
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