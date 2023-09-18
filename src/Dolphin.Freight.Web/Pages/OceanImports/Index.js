var l = abp.localization.getResource('Freight');
var dataTable;
var _changeInterval = null;
var queryListFilter = function () {
    return {
        search: $("#Keyword").val(),

        ovearseaAgentId: $("#VesselSchedule_OverseaAgentId").val() == '' ? null : $("#VesselSchedule_OverseaAgentId").val(),
        carrierId: $("#VesselSchedule_CarrierId").val() == '' ? null : $("#VesselSchedule_CarrierId").val(),
        shippingAgentId: $("#VesselSchedule_ShipperId").val() == '' ? null : $("#VesselSchedule_ShipperId").val(),
        cyLocationId: $("#VesselSchedule_CyLocationId").val() == '' ? null : $("#VesselSchedule_CyLocationId").val(),
        pod: $("#VesselSchedule_PodId").val() == '' ? null : $("#VesselSchedule_PodId").val(),
        pol: $("#VesselSchedule_PolId").val() == '' ? null : $("#VesselSchedule_PolId").val(),
        pol: $("#VesselSchedule_PolId").val() == '' ? null : $("#VesselSchedule_PolId").val(),
        del: $("#VesselSchedule_DelId").val() == '' ? null : $("#VesselSchedule_DelId").val(),
        saleId: $("#VesselSchedule_SaleId").val() == '' ? null : $("#VesselSchedule_SaleId").val(),
       
        postDate: $("#PostDate").val() == '' || $("#PostDate").val() == null ? null : new Date($("#PostDate").val()),


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
                                        dolphin.freight.importExport.oceanImports.oceanImportMbl
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

$(function () {

    dolphin.freight.web.controllers.configuration.getJsonConfig('OceanImports').done(function (data) {
        data.forEach(function (item) {

            if (!item.lock && item.checkable) {
                var column;

                if (item.text.toLowerCase().includes('islocked')) {
                    column = {
                        //是否鎖定
                        title: l('IsLocked'),
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
                ajax: abp.libs.datatables.createAjax(dolphin.freight.importExport.oceanImports.oceanImportMbl.queryList, queryListFilter),
                columnDefs: columns
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
            src: 'OceanImports'
        });
    })
});

var lock = function (id) {
    var isLock = $('#lock_' + id).find('i').hasClass('fa-lock');
    abp.message.confirm(l(isLock ? 'UnlockConfirmationMessage' : 'LockConfirmationMessage')).then(function (confirmed) {
        if (confirmed) {
            dolphin.freight.importExport.oceanImports.oceanImportMbl.lockedOrUnLockedOceanImportMbl({ MbId: id })
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