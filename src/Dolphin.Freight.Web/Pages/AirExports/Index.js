﻿$(function () {
    var l = abp.localization.getResource('Freight');
    var _changeInterval = null;
    var queryListFilter = function () {
        return {
            search: $("input[name='Search'").val()
        };
    };
    var columns = [{
        title: l('Actions'),
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
    }]

    var otherFixedColumns = [{
        title: l('grossWeightKg'),
        data: "grossWeightKg"
    },
    {
        title: l('grossWeightLb'),
        data: "grossWeightLb"
    },
    {
        title: l('chargeableWeightKg'),
        data: "chargeableWeightKg"
    },
    {
        title: l('chargeableWeightLb'),
        data: "chargeableWeightLb"
    }]

    var dataTable;

    dolphin.freight.web.controllers.configuration.getJsonConfig('AirExport').done(function (data) {
        data.forEach(function (item) {

            if (!item.lock && item.checkable) {
                var itemData = item.name;
                if (item.name.toLowerCase().includes('date')) {
                    itemData = function (row, type, set) {
                        var depatureDate = new Date(row.depatureDate);
                        if (depatureDate.getFullYear() == 1) {
                            return '';
                        }
                        return depatureDate.toLocaleDateString() + ' ' + depatureDate.toLocaleTimeString();
                    }
                }

                var column = {  
                    title: l(item.text),
                    data: itemData
                };
                columns.push(column);
            }
        });
        columns = columns.concat(otherFixedColumns);
        var col = (columns.length > 1) ? [[1, 'asc']] : [[0, 'asc']];

        dataTable = $('#MblListTable').DataTable(
            abp.libs.datatables.normalizeConfiguration({
                serverSide: true,
                paging: true,
                order: col,
                searching: false,
                scrollX: true,
                processing: true,
                ajax: abp.libs.datatables.createAjax(dolphin.freight.importExport.airExports.airExportMawb.queryList, queryListFilter),
                columnDefs: columns
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
        location.href = 'CreateMawb';
    });

    $('#btnConfiguration').click(function (e) {
        var _configurationModal = new abp.ModalManager({
            viewUrl: abp.appPath + 'Configuration',
            modalClass: 'ConfigurationViewModel'
        });

        _configurationModal.open({
            src: 'AirExport'
        });
    })

});