var l = abp.localization.getResource('Freight');
var dataTable;
var _changeInterval = null;
var queryListFilter = function () {
    return {
        search: $("#Keyword").val(),
       
        ovearseaAgentId: $("#VesselSchedule_OverseaAgentId").val() == '' ? null : $("#VesselSchedule_OverseaAgentId").val(),
        carrierId: $("#VesselSchedule_CarrierId").val() == '' ? null : $("#VesselSchedule_CarrierId").val(),
        shippingAgentId: $("#VesselSchedule_ShippingAgentId").val() == '' ? null : $("#VesselSchedule_ShippingAgentId").val(),
        forwardingAgentId: $("#VesselSchedule_ForwardingAgentId").val() == '' ? null : $("#VesselSchedule_ForwardingAgentId").val(),
        pod: $("#VesselSchedule_PodId").val() == '' ? null : $("#VesselSchedule_PodId").val(),
        pol: $("#VesselSchedule_PolId").val() == '' ? null : $("#VesselSchedule_PolId").val(),
        pol: $("#VesselSchedule_PolId").val() == '' ? null : $("#VesselSchedule_PolId").val(),
        del: $("#VesselSchedule_DelId").val() == '' ? null : $("#VesselSchedule_DelId").val(),
        deliverTo: $("#VesselSchedule_DeliverToId").val() == '' ? null : $("#VesselSchedule_DeliverToId").val(),
        shipModeId: $("#VesselSchedule_ShipModeId").val() == '' ? null : $("#VesselSchedule_ShipModeId").val(),
        blCancelled: $("#BlCancelled").val() == '' ? null : $("#BlCancelled").val(),
        postDate: $("#PostDate").val() == '' || $("#PostDate").val() == null ? null : new Date($("#PostDate").val()),
        
        eta: $("#ETA").val() == '' || $("#ETA").val() == null? null : new Date($("#ETA").val()),
        etd: $("#ETD").val() == '' || $("#ETD").val() == null ? null : new Date($("#ETD").val()),
        creationDate: $("#CreationDate").val() == '' || $("#CreationDate").val() == null ? null : new Date($("#CreationDate").val()),
        vessel: $("#Vessel").val(),
    };
};

var columns = [
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
                                            dolphin.freight.importExport.oceanExports.oceanExportMbl
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

    },
 
]

$(function () {
    dolphin.freight.web.controllers.configuration.getJsonConfig('OceanExports').done(function (data) {
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


        dataTable = $('#MblListTable').DataTable(
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
                ajax: abp.libs.datatables.createAjax(dolphin.freight.importExport.oceanExports.oceanExportMbl.queryList, queryListFilter),
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
    $('#Search').click(function (e) {
        dataTable.ajax.reload();
    });
    $('#CancelFilter').click(function (e) {
        $("#Keyword").val(null);
       
        // $("#VesselSchedule_OfficeId").val(null);
        $("select[name='VesselSchedule_OverseaAgentId']").val(null);
        $("select[name = 'VesselSchedule_CarrierId']").val(null).trigger("change");
        $("select[name='VesselSchedule_ShippingAgentId']").val(null).trigger("change");
        $("select[name='VesselSchedule_ForwardingAgentId']").val(null).trigger("change");
        
        $("select[name='VesselSchedule_PolId']").val(null).trigger("change");
        $("select[name='VesselSchedule_PodId']").val(null).trigger("change");
        $("select[name='VesselSchedule_DelId']").val(null).trigger("change");
        $("select[name='VesselSchedule_DeliverToId']").val(null).trigger("change");
        $("select[name='VesselSchedule_ShipModeId']").val(null).trigger("change");
        $("#BlCancelled").val(null).trigger("change");
        $("#PostDate").val(null);
        $("#ETA").val(null);
        $("#ETD").val(null);
        $("#CreationDate").val(null);
        $("#Vessel").val(null);
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
            src: 'OceanExports'
        });
    })

});

var lock = function (id) {
    var isLock = $('#lock_' + id).find('i').hasClass('fa-lock');
    abp.message.confirm(l(isLock ? 'UnlockConfirmationMessage' : 'LockConfirmationMessage')).then(function (confirmed) {
        if (confirmed) {
            dolphin.freight.importExport.oceanExports.oceanExportMbl.lockedOrUnLockedOceanExportMbl({ MbId: id })
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