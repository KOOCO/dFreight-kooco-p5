﻿$(function () {
    var l = abp.localization.getResource('Freight');
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
            incotermsType: $("#IncotermsType").val() == '' ? null : $("#IncotermsType").val(),


            depatureDate: $("#DepatureDate").val() == '' || $("#DepatureDate").val() == null ? null : new Date($("#DepatureDate").val()),
            arrivalDate: $("#ArrivalDate").val() == '' || $("#ArrivalDate").val() == null ? null : new Date($("#ArrivalDate").val()),
            postDate: $("#PostDate").val() == '' || $("#PostDate").val() == null ? null : new Date($("#PostDate").val()),
            creationDate: $("#CreationDate").val() == '' || $("#CreationDate").val() == null ? null : new Date($("#CreationDate").val()),

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

    $('#Search').click(function (e) {
        dataTable.ajax.reload();
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