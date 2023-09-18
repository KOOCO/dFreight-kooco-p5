var l = abp.localization.getResource('Freight');
var dataTable;

var _changeInterval = null;
var queryListFilter = function () {
    return {
        search: $("#Keyword").val(),

        ovearseaAgentId: $("#VesselSchedule_OverseaAgentId").val() == '' ? null : $("#VesselSchedule_OverseaAgentId").val(),
        shipperId: $("#VesselSchedule_ShipperId").val() == '' ? null : $("#VesselSchedule_ShipperId").val(),
        notify: $("#VesselSchedule_NotifyId").val() == '' ? null : $("#VesselSchedule_NotifyAgentId").val(),
        consigneeId: $("#VesselSchedule_ConsigneeId").val() == '' ? null : $("#VesselSchedule_ConsigneeId").val(),
        finalDestId: $("#VesselSchedule_FinalDestId").val() == '' ? null : $("#VesselSchedule_FinalDestId").val(),
        truckerId: $("#VesselSchedule_TruckerId").val() == '' ? null : $("#VesselSchedule_TruckerId").val(),
        svcTermId: $("#VesselSchedule_SvcTermId").val() == '' ? null : $("#VesselSchedule_SvcTermId").val(),
        customerId: $("#VesselSchedule_CustomerId").val() == '' ? null : $("#VesselSchedule_CustomerId").val(),
        porId: $("#VesselSchedule_PorId").val() == '' ? null : $("#VesselSchedule_PorId").val(),
        pod: $("#VesselSchedule_PodId").val() == '' ? null : $("#VesselSchedule_PodId").val(),

        pol: $("#VesselSchedule_PolId").val() == '' ? null : $("#VesselSchedule_PolId").val(),
        del: $("#VesselSchedule_DelId").val() == '' ? null : $("#VesselSchedule_DelId").val(),

        shipModeId: $("#VesselSchedule_ShipModeId").val() == '' ? null : $("#VesselSchedule_ShipModeId").val(),
        saleId: $("#VesselSchedule_SaleId").val() == '' ? null : $("#VesselSchedule_SaleId").val(),
       
        eta: $("#ETA").val() == '' || $("#ETA").val() == null ? null : new Date($("#ETA").val()),
        etd: $("#ETD").val() == '' || $("#ETD").val() == null ? null : new Date($("#ETD").val()),
        delEta: $("#DelEta").val() == '' || $("#DelEta").val() == null ? null : new Date($("#DelEta").val()),
        finalDestEta: $("#FinalDestEta").val() == '' || $("#FinalDestEta").val() == null ? null : new Date($("#FinalDestEta").val()),
        creationDate: $("#CreationDate").val() == '' || $("#CreationDate").val() == null ? null : new Date($("#CreationDate").val()),

    };
};


$(function () {
    dataTable = $('#HblListTable').DataTable(
        abp.libs.datatables.normalizeConfiguration({
            serverSide: true,
            paging: true,
            order: [[2, "asc"]],
            searching: false,
            scrollX: true,
            processing: true,
            responsive: {
                details: {
                    type: 'column'
                }
            },
            ajax: abp.libs.datatables.createAjax(dolphin.freight.importExport.oceanImports.oceanImportHbl.queryList, queryListFilter),
            columnDefs: [
                //{
                //    className: 'dtr-control',
                //    orderable: false,
                //    "defaultContent": ""
                //},
                {
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
                                        location.href = 'EditModal?Id=' + data.record.mblId + '&Hid=' + data.record.id;

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

                                        /*
                                        if (!data.record.isLocked) {
                                            dolphin.freight.importExport.oceanImports.oceanImportMbl
                                                .delete(data.record.id)
                                                .then(function () {
                                                    abp.message.success(l('SuccessfullyDeleted'));
                                                    dataTable.ajax.reload();
                                                });
                                        } else {
                                            abp.message.warn("鎖定不能刪除")
                                        }*/

                                    }
                                }
                            ]
                    }
                },
                {
                    //是否鎖定
                    title: l('IsLocked'),
                    orderable: false,
                    render: function (data, type, row, meta) {
                        if (row.isLocked)
                            return "<a href='javascript:lock(\"" + row.id + "\")' class='btn-lock' id='lock_" + row.id + "'><i class='fa-lg fa-solid fa-lock fa-lg'></i><span>解鎖</span></a>";
                        else
                            return "<a href='javascript:lock(\"" + row.id + "\")' class='btn-lock action' id='lock_" + row.id + "'><i class='fa-lg fa-solid fa-lock-open'></i><span>上鎖</span></a>";
                    }
                },
                {
                    //文件號碼
                    title: l('FilingNo'),
                    data: "filingNo"
                },
                {
                    //HBL號碼
                    title: l('HblNo'),
                    data: "hblNo"
                },
                {
                    //MBL號碼
                    title: l('MblNo'),
                    data: "mblNo"
                },
                {
                    //分站
                    title: l('OfficeId'),
                    data: "officeName"
                },

                {
                    //代理
                    title: l('AgentName'),
                    data: "agentName"
                },
                {
                    //託運人
                    title: l('IHblShipperName'),
                    data: "hblShipperName"
                },
                {
                    //收貨人
                    title: l('MblConsigneeName'),
                    data: "hblConsigneeName"
                },
                {
                    //AR結餘
                    title: l('ARSurplus'),
                    data: "arSurplus"
                },
                {
                    //A/P結餘
                    title: l('APSurplus'),
                    data: "apSurplus"
                },
                {
                    //D/C結餘
                    title: l('DCSurplus'),
                    data: "dcSurplus"
                },
                {
                    //船公司S/O號碼
                    title: l('SoNo'),
                    data: "soNo"
                },
                {
                    //是否扣留
                    title: l('IsHold'),
                    data: function (row, type, set) {
                        if (row.isHold) return "V";
                        else return "";

                    }
                },
                {
                    //放貨
                    title: l('IsReleased'),
                    data: function (row, type, set) {
                        if (row.isReleased) return "V";
                        else return "";

                    }
                },
                {
                    //OBL編號
                    title: l('OblTypeName'),
                    data: "oblTypeName"
                },
                {
                    //客戶名稱
                    title: l('HblCustomerName'),
                    data: "hblCustomerName"
                },
                {
                    //ETD
                    title: l('PolEtd'),
                    data: "polEtd"
                },
                {
                    //ETA
                    title: l('PodEta'),
                    data: "podEta"
                },
                {
                    //收貨地(POR)
                    title: l('PorName'),
                    data: "porName"
                },
                {
                    //收貨地(POR)
                    title: l('PolName'),
                    data: "polName"
                },
                {
                    //卸貨港(POD)
                    title: l('PodName'),
                    data: "podName"
                },
                {
                    //交貨地(DEL)
                    title: l('DelName'),
                    data: "delName"
                },
                {
                    //船公司
                    title: l('MblCarrierName'),
                    data: "mblCarrierName"
                }

            ]
        })
    );

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
        $("select[name ='ShipperId']").val(null).trigger("change");
        $("select[name='VesselSchedule_NotifyId']").val(null).trigger("change");
        $("select[name='VesselSchedule_ConsigneeId']").val(null).trigger("change");
        $("select[name='VesselSchedule_PorId']").val(null).trigger("change");
        $("select[name='VesselSchedule_PolId']").val(null).trigger("change");
        $("select[name='VesselSchedule_PodId']").val(null).trigger("change");
        $("select[name='VesselSchedule_DelId']").val(null).trigger("change");
        $("select[name='VesselSchedule_DeliverToId']").val(null).trigger("change");
        $("select[name='VesselSchedule_ShipModeId']").val(null).trigger("change");
        $("select[name='VesselSchedule_CustomerId']").val(null).trigger("change");
        $("select[name='VesselSchedule_SvcTermId']").val(null).trigger("change");
        $("select[name='VesselSchedule_SaleId']").val(null).trigger("change");
        $("select[name='VesselSchedule_FinalDestId']").val(null).trigger("change");
        $("#PostDate").val(null);
        $("#ETA").val(null);
        $("#ETD").val(null);
        $("#DelEta").val(null);
        $("#FinalDestEta").val(null);
        $("#CreationDate").val(null);
        $("#Vessel").val(null);
        dataTable.ajax.reload();
    });
});

var lock = function (id) {
    var isLock = $('#lock_' + id).find('i').hasClass('fa-lock');
    abp.message.confirm(l(isLock ? 'UnlockConfirmationMessage' : 'LockConfirmationMessage')).then(function (confirmed) {
        if (confirmed) {
            dolphin.freight.importExport.oceanImports.oceanImportHbl.lockedOrUnLockedOceanImportHbl({ HblId: id })
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