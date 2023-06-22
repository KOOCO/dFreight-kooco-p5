var l = abp.localization.getResource('Freight');
var dataTable;
$(function () {
    dataTable = $('#HblListTable').DataTable(
        abp.libs.datatables.normalizeConfiguration({
            serverSide: true,
            paging: true,
            order: [[2, "asc"]],
            searching: true,
            scrollX: true,
            responsive: {
                details: {
                    type: 'column'
                }
            },
            ajax: abp.libs.datatables.createAjax(dolphin.freight.importExport.oceanExports.oceanExportHbl.queryList),
            columnDefs: [
                {
                    className: 'dtr-control',
                    orderable: false,
                    "defaultContent": ""
                },
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
                                                        dolphin.freight.importExport.oceanExports.oceanExportHbl
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
                                            dolphin.freight.importExport.oceanExports.oceanExportMbl
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
  
    $('[type=search]').on('keyup', function () {
        dataTable.search(this.value).draw();
    });
});

var lock = function (id) {
    var isLock = $('#lock_' + id).find('i').hasClass('fa-lock');
    abp.message.confirm(l(isLock ? 'UnlockConfirmationMessage' : 'LockConfirmationMessage')).then(function (confirmed) {
        if (confirmed) {
            dolphin.freight.importExport.oceanExports.oceanExportHbl.lockedOrUnLockedOceanExportHbl({ HblId: id })
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