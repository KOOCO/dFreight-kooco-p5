var l = abp.localization.getResource('Freight');
var dataTable;
var _changeInterval = null;
var queryListFilter = function () {
    return {
        search: $("input[name='Search'").val()
    };
};

function deleteHbl() {
    var ids = [];
    var selectedCheckboxes = $('#HblListTable tbody input.selectCheckbox[type="checkbox"]:checked');
    for (var i = 0; i < selectedCheckboxes.length; i++) {
        debugger;
        var id = selectedCheckboxes[i].attributes[2].value;
        ids.push(id);
        
        abp.message.confirm(l('DeleteConfirmationMessage')).then(function (confirmed) {
            if (confirmed) {
                dolphin.freight.importExport.oceanExports.oceanExportHbl.deleteMultipleHbls(ids)
                    .done(function () {
                        abp.message.success(l('Message:SuccessDelete'));
                        dataTable.ajax.reload();
                    });
            }
        });
    }
}

function selectAllCheckbox(element) {
    var isChecked = $(element).prop('checked');
    $('#HblListTable tbody input.selectCheckbox[type="checkbox"]').prop('checked', isChecked);
}

function selectCheckbox(checkbox) {
    var checkedCheckboxes = $('.selectCheckbox:checked');
    if (checkbox.checked) {
        var isAnyLocked = false;
        var isAnyUnlocked = false
        checkedCheckboxes.each(function (index, checkbox1) {
            $('#deleteBtn').prop('disabled', false);
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
        var checkedCheckboxes = $('.selectCheckbox:checked');
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
        $('#deleteBtn').prop('disabled', true);
        $('#lockId').prop('disabled', !isAnyUnlocked);
        $('#unlockId').prop('disabled', !isAnyLocked);
    }
    if (!$(checkbox).prop('checked')) {
        $('#selectAllCheckbox').prop('checked', false);
    } else {
        var allChecked = true;
        $('#HblListTable tbody input.selectCheckbox[type="checkbox"]').each(function () {
            if (!$(this).prop('checked')) {
                allChecked = false;
                return false;
            }
        });
        $('#selectAllCheckbox').prop('checked', allChecked);
    }
}

function setSelectedLockStatus(isLock) {
    var ids = [];
    var selectedCheckboxes = $('#HblListTable tbody input.selectCheckbox[type="checkbox"]:checked');
    var lockCondition = isLock ? false : true;
    var confirmationMessage = isLock ? l('LockConfirmationMessage') : l('UnlockConfirmationMessage');
    var successMessage = isLock ? l('Message:SuccessLock') : l('Message:SuccessUnlock');

    for (var i = 0; i < selectedCheckboxes.length; i++) {
        var id = selectedCheckboxes[i].attributes[2].value;
        var currentLockStatus = $('#lock_' + id).find('i').hasClass('fa-lock');

        if (currentLockStatus === lockCondition) {
            ids.push(id);
        }
    }

    abp.message.confirm(confirmationMessage).then(function (confirmed) {
        if (confirmed) {
            dolphin.freight.importExport.oceanExports.oceanExportHbl.setLockStatusOnOceanExportHbl(ids, isLock).done(function ()
            {
                abp.message.success(successMessage);
                dataTable.ajax.reload();
            });
        }
    });
}

function lockCheckBox(checkbox) {
    var selectedCheckboxes = $('#HblListTable tbody input.lockUnlockCheckbox[type="checkbox"]:checked');

    var id = checkbox.attributes[2].value;

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
            ajax: abp.libs.datatables.createAjax(dolphin.freight.importExport.oceanExports.oceanExportHbl.queryList, queryListFilter),
            columnDefs: [
                //{
                //    className: 'dtr-control',
                //    orderable: false,
                //    "defaultContent": ""
                //},
                {
                    title: '<input type="checkbox" id="selectAllCheckbox" disable="true" onclick="selectAllCheckbox(this)" style=" cursor: pointer;">',
                    data: null,
                    orderable: false,
                    render: function (data, type, row) {
                        var id = row.id;
                        $('#selectAllCheckbox').prop('checked', false);
                        return '<input type="checkbox" class="selectCheckbox" data-id="' + id + '" onclick="selectCheckbox(this)" style=" cursor: pointer;">';
                    }
                },
                {
                    title: '<div  style=" cursor: pointer;"><span><i class="fa fa-lock"></i></span></div>',
                    orderable: false,
                    "render": function (data, type, row) {
                        debugger;
                        var isCkecked = row.isLocked;
                        var Hblid = row.id;
                        if (isCkecked) {
                            return '<input type="checkbox" class="lockUnlockCheckbox" data-id="' + Hblid + '"  checked="' + isCkecked + '" onclick="lockCheckBox(this)"  style=" cursor: pointer;">';
                        } else {
                            return '<input type="checkbox" class="lockUnlockCheckbox" data-id="' + Hblid + '" onclick="lockCheckBox(this)"   style=" cursor: pointer;">';
                        }
                    }
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
  
    $('#Search').keyup(function () {
        clearInterval(_changeInterval)
        _changeInterval = setInterval(function () {
            dataTable.ajax.reload();
            clearInterval(_changeInterval)
        }, 1000);
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